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
var     io = require('socket.io')(serv,{});


var playerNo = 0;
var roomNo = 0;
var ROOMS = {};

var mapingSocketRoom = {};


var idTowers = [];
/*var makeSendableRooms = function(rooms){
    var sendRooms = {};
    for(var roomKey in rooms){
        for(var key in rooms[roomKey])
            {
                if( sendRooms[roomKey] === undefined){
                    sendRooms[roomKey] = {};
                }
                    if(key != "io"){
                        sendRooms[roomKey][key] = rooms[roomKey][key];
                    }
            }
    }
}*/
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
    socket.on("minionFollowMinion",onMinionFollowMinion);
    socket.on("attack",onClientAttack);
});

var onNewRoom = function(data){
    var roomName = "Room "+roomNo;
    roomNo++;
    
    var room = new Room(this.id,roomName,2); 
    var player = new ControllerPlayer(this.id,"Player1",-47,0,7,"true");
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
        var player = new ControllerPlayer(this.id,"Player1",47,0,-5,"false");
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

var onMinionFollowMinion = function(data){
    console.log("Minionul "+data["idFollower"]+" urmareste pe "+data["idTarget"]);
    
    io.to(mapingSocketRoom[this.id].name).emit("minionFollowMinion",{
        id_follower : data["idFollower"],
        target_id:data["idTarget"]
    });
}

var onClientAttack = function(data){
    console.log("Clientul "+this.id+" ataca pe "+data["idTarget"]);
    
    io.to(mapingSocketRoom[this.id].name).emit("attackPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
}

setInterval(function(){
   for(var roomId in mapingSocketRoom)
        mapingSocketRoom[roomId].SpawnMinions(io);    
   
},40);



    



