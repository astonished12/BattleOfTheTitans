var express = require('express');
var app = express();
var serv = require('http').Server(app);
var shortid = require('shortid');


app.get('/',function(req, res) {
    res.sendFile(__dirname + '/Client/Index.html');
});
app.use('/Client',express.static(__dirname + '/Client'));

serv.listen(process.env.PORT||3000);

console.log("Server started.");

var ControllerPlayer = require("./Server/ControllerPlayer");
var Room = require("./Server/Room.js");
var DatabaseManager = require("./Server/DatabaseManager.js");
var dbM = new DatabaseManager();

var io = require('socket.io')(serv,{});


var playerNo = 0;
var roomNo = 0;
var ROOMS = {};

var mapingSocketRoom = {};
var globalPlayersLogged = {};
var mapNameInGameIdDatabase = {};
var idTowers = [];

io.sockets.on('connection', function(socket){
    console.log('Client connected is '+socket.id);

    socket.emit('allRooms',{    
        socket_id : socket.id,
        //allRoomsAtCurrentTime: makeSendableRooms(ROOMS)
        allRoomsAtCurrentTime : ROOMS
    });

    socket.on("newRoom",onNewRoom);
    socket.on("closeRoom",closeRoom);
    socket.on("joinRoom",onJoinRoom);
    socket.on("characterId",onCharacterIdSelected);
    socket.on("play",onPlay); 
    socket.on("disconnect",onSocketDisconnect)
    socket.on("move",onMoveClient);
    socket.on("follow",onFollowClient);
    socket.on("followTower",onFollowTower);
    socket.on("followMinion",onFollowMinion);
    socket.on("minionFollowMinion",onMinionFollowMinion);
    socket.on("minionNoTarget",onMinionHasNoTarget);
    socket.on("minionAttackMinion",onMinionAttackMinion);
    socket.on("towerTarget",onTowerTarget);
    socket.on("keyPressed",OnKeyPressed);
    socket.on("attack",onClientAttack);
    socket.on("newMessageGameChat",onNewMessageChat);
    socket.on("register",onRegister);
    socket.on("login",onLogin);
    socket.on("getMyFriends",onRequireFriends);
    socket.on("searchFriend",onSearchFriend);
    socket.on("removeFriend",onRemoveFriend);
});

var onRegister = function(data){
    console.log("Utilizatorul este "+data["username"]+" are parola "+data["password"]+" si emailul "+data["email"]);
    //to DO inser in database check duplicates etc
    var socket = this;
    dbM.InserIntoUsers(data["username"],data["password"],data["email"], function(err) {
        if(err)
            console.log(err);
        if(err==="duplicate")
            socket.emit("usernameExist");
        else
            socket.emit("registerSuccesfull");
    });

}
var checkAlreadyLog = function(name){
    for(var socketId in globalPlayersLogged){
        if(globalPlayersLogged[socketId] === name)
            return true;
    }
    return false;
}

var getUserOnlineSocket = function(name,mySocketId){
    for(var socketId in globalPlayersLogged){
        if(globalPlayersLogged[socketId] === name && socketId!==mySocketId)
            return socketId;
    }
    return -1;
}
var onLogin = function(data){
    var socket = this;
    if(!checkAlreadyLog(data["username"]))
    {
        dbM.CheckLogin(data["username"],data["password"], function(err,id) {
            if(err)
                console.log(err);

            if(err==="fail")
                socket.emit("wrongData");
            else if(err==="succes")
            {
                socket.emit("loginSuccesfull",{
                    username : data["username"]
                });

                globalPlayersLogged[socket.id] = data["username"];
                //to do update databse for log
                //send array of connected frinds
                console.log("Dupa logare idu meu este "+id);
                mapNameInGameIdDatabase[data["username"]] = id;
                dbM.MakeLoginOnOff(data["username"],true);
            }
        });
    }
    else
    {
        socket.emit("alreadyLoged");
    }
}

var onRequireFriends = function(data){
    var id = mapNameInGameIdDatabase[data["message"]];
    var socket = this;
    dbM.GetListOfFriendById(id, function(status,listOfFriends){
        if(status=="noFriends"){
            console.log("No friends for "+id);
        }
        if(status=="Friends"){                      
            console.log("Lista prietenilor este :"+ listOfFriends);
        }

        socket.emit("listFriends",{            
            friends : listOfFriends
        });

    });
}

var onSearchFriend = function(data){
    var socketId = getUserOnlineSocket(data["friendName"],this.id);
    if(socketId!==-1){
        console.log("Jucatorul "+data["friendName"]+" cu "+socketId+" este online");
        this.emit("newFriend",{
            name: globalPlayersLogged[socketId]
        });

        io.to(socketId).emit("newFriend",{
            name: globalPlayersLogged[this.id]
        });


        dbM.InsertFriend(mapNameInGameIdDatabase[globalPlayersLogged[this.id]], data["friendName"]);
    }
    else
    {
        console.log("Jucatorul "+data["friendName"]+ " nu este online ");
        this.emit("playerNotOnline");
    }
}

var onRemoveFriend = function(data){
    var socketId = getUserOnlineSocket(data["friendName"],this.id);

    if(socketId!==-1){
        console.log("Jucatorul "+data["friendName"]+" cu "+socketId+" este online");
        this.emit("removeFriend",{
            name: globalPlayersLogged[socketId]
        });

        io.to(socketId).emit("removeFriend",{
            name: globalPlayersLogged[this.id]
        });

        dbM.RemoveFriend(mapNameInGameIdDatabase[globalPlayersLogged[this.id]], data["friendName"]);
    }
    else
    {
        console.log("Jucatorul "+data["friendName"]+ " nu este online ");
        this.emit("playerNotOnline");
    }
}
var onNewRoom = function(data){
    var roomName = "Room "+roomNo;
    roomNo++;

    var room = new Room(this.id,roomName,2); 
    var player = new ControllerPlayer(this.id,globalPlayersLogged[this.id],-88,0,4.5,"true");
    room.PLAYERS[this.id] = player;

    room.towersId.push(shortid.generate());
    room.towersId.push(shortid.generate());
    room.towersId.push(shortid.generate());
    room.towersId.push(shortid.generate());
    room.towersId.push(shortid.generate());
    room.towersId.push(shortid.generate());

    ROOMS[this.id] = room;

    mapingSocketRoom[this.id] =  ROOMS[this.id];

    this.join(ROOMS[this.id].name);   

    this.broadcast.emit("newRoom",{
        socket_id : ROOMS[this.id].id,
        maxPlayers : ROOMS[this.id].maxPlayers,
        currentPlayers : ROOMS[this.id].currentPlayers,
        name : ROOMS[this.id].name
    });

}

var closeRoom = function(){
    delete ROOMS[this.id];
    delete mapingSocketRoom[this.id];
    roomNo--;
    this.broadcast.emit("closeRoom",{
        socket_id : this.id,
    });
}

var onJoinRoom = function(data){
    if(ROOMS[data["idRoom"]].currentPlayers >= ROOMS[data["idRoom"]].maxPlayers)
    {
        this.emit("roomIsFull");
    }
    else
    {
        io.sockets.emit("roomFull",{
            room_id : data["idRoom"]});

        ROOMS[data["idRoom"]].currentPlayers++;            
        var player = new ControllerPlayer(this.id,globalPlayersLogged[this.id],88,0,-5,"false");
        ROOMS[data["idRoom"]].PLAYERS[this.id] = player; 
        //console.log(ROOMS[data["idRoom"]].PLAYERS);
        mapingSocketRoom[this.id] = ROOMS[data["idRoom"]];        
        this.join(ROOMS[data["idRoom"]].name);

        io.to(ROOMS[data["idRoom"]].name).emit("joinSuccesFull",{
            room_id :  data["idRoom"]});     

    }

}

var onCharacterIdSelected = function(data){
    console.log("Id character selected "+data["idCharacter"]);
    mapingSocketRoom[this.id].PLAYERS[this.id].characterNumber = data["idCharacter"];
    mapingSocketRoom[this.id].confirmedCharacters += 1;
    if(mapingSocketRoom[this.id].confirmedCharacters == 2){
        io.to(mapingSocketRoom[this.id].name).emit("canPlay");
    }
}
var onPlay = function(){  

    //console.log(mapingSocketRoom[this.id].PLAYERS);

    this.emit('identify',{     
        x : mapingSocketRoom[this.id].PLAYERS[this.id].x,
        y : mapingSocketRoom[this.id].PLAYERS[this.id].y,
        z :mapingSocketRoom[this.id].PLAYERS[this.id].z,
        owner : mapingSocketRoom[this.id].PLAYERS[this.id].isOwner,
        name : mapingSocketRoom[this.id].PLAYERS[this.id].name,
        socket_id : this.id,
        allPlayersAtCurrentTime: mapingSocketRoom[this.id].PLAYERS,
        towersId : mapingSocketRoom[this.id].towersId
    });

    var timer = new Date();
    mapingSocketRoom[this.id].lastTimeSpawn = timer.getTime();

}

var onSocketDisconnect = function(){
    //TO DO PLAYER IN ROOM CHECKER
    if(mapingSocketRoom[this.id])
    {
        this.broadcast.emit('playerLeft', {
            socket_id : this.id,
            name: mapingSocketRoom[this.id].PLAYERS[this.id].name
        });
        console.log("Clinet id "+this.id+" disconnected.");
        delete mapingSocketRoom[this.id].PLAYERS[this.id]
        playerNo --;
    }
    else
    {
        if(ROOMS[this.id]) 
            delete ROOMS[this.id];
        console.log("Clinet id "+this.id+" disconnected.");
    }

    if(globalPlayersLogged[this.id]){
        dbM.MakeLoginOnOff(globalPlayersLogged[this.id],false);
        delete mapNameInGameIdDatabase[globalPlayersLogged[this.id]];
        delete globalPlayersLogged[this.id];
        //to do call dbmanager to unlog the player        
    }
}


var onMoveClient = function(data){
    console.log(mapingSocketRoom[this.id].PLAYERS[this.id].id+" is moving to "+JSON.stringify(data));
    mapingSocketRoom[this.id].PLAYERS[this.id].updatePositions(data);
    this.broadcast.to(mapingSocketRoom[this.id].name).emit("playerMove",{
        socket_id:this.id,
        x : data["x"],
        y : data["y"],
        z : data["z"],
    });
};

var onFollowClient = function(data){
    console.log("Clientul "+this.id+" urmareste pe "+data["idTarget"]);

    this.broadcast.to(mapingSocketRoom[this.id].name).emit("followPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
};

var onFollowTower = function(data){
    console.log("Clientul "+this.id+" urmareste pe "+data["idTarget"]);

    this.broadcast.to(mapingSocketRoom[this.id].name).emit("followTower",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
};

var onFollowMinion = function(data){
    console.log("Clientul "+this.id+" urmareste pe minionul"+data["idTarget"]);

    this.broadcast.to(mapingSocketRoom[this.id].name).emit("followMinion",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
};

var onMinionFollowMinion = function(data){
    console.log("Minionul "+data["idFollower"]+" urmareste pe "+data["idTarget"]);

    io.to(mapingSocketRoom[this.id].name).emit("minionFollowMinion",{
        id_follower : data["idFollower"],
        target_id:data["idTarget"]
    });
}
var onMinionHasNoTarget = function(data){
    //console.log("Minionul "+data["idTarget"]+ " nu are inamici in jur") ;

    io.to(mapingSocketRoom[this.id].name).emit("minionHasNoTarget",{
        target_id:data["idTarget"]
    });
}
var onMinionAttackMinion = function(data){
    console.log("Minionul "+data["idAttacker"]+" ataca pe "+data["idTarget"]);

    io.to(mapingSocketRoom[this.id].name).emit("minionAttackMinion",{
        id_attacker : data["idAttacker"],
        target_id:data["idTarget"]
    });
}


var onClientAttack = function(data){
    console.log("Clientul "+this.id+" ataca tureta "+data["idTarget"]);

    io.to(mapingSocketRoom[this.id].name).emit("attackPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
}

var onTowerTarget = function(data){
    console.log("Tureta "+data["idAttacker"]+" ataca pe "+data["idTarget"]);
    io.to(mapingSocketRoom[this.id].name).emit("towerAttack",{
        id_attacker:data["idAttacker"],
        target_id:data["idTarget"]
    });
}


var OnKeyPressed = function(data){
    console.log("Jucatorul " +this.id + " a apasat pe tasta "+data["key"]+" la pozitia "+JSON.stringify(data));
    io.to(mapingSocketRoom[this.id].name).emit("keyPressed",{
        user_id:this.id,
        key:data["key"],
        x : data["x"],
        y : data["y"],
        z : data["z"],
    });
};

var onNewMessageChat = function(data){
    console.log("Jucatorul "+this.id+" a scris "+data["message"]);
    io.to(mapingSocketRoom[this.id].name).emit("newMessage",{
        socket_id : this.id,
        name : globalPlayersLogged[this.id],
        message:data["message"]
    })
}
setInterval(function(){
    for(var roomId in mapingSocketRoom)
        mapingSocketRoom[roomId].SpawnMinions(io);    

},40);







