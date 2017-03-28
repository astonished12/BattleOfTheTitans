var express = require('express');
var app = express();
var serv = require('http').Server(app);

app.get('/',function(req, res) {
    res.sendFile(__dirname + '/Client/Index.html');
});
app.use('/Client',express.static(__dirname + '/Client'));

serv.listen(process.env.PORT||3000);

console.log("Server started.");


var io = require('socket.io')(serv,{});


var playerNo = 0;
var roomNo = 0;
var PLAYERS = {};
var ROOMS = {};
var SOCKET_LIST = {};
var ControllerPlayer = require("./Server/ControllerPlayer");
var Room = require("./Server/Room.js");

io.sockets.on('connection', function(socket){
    console.log('Client connected is '+socket.id);
    /*if(playerNo==1)
    {
        var player = new ControllerPlayer(socket.id,"Player1",-47,0,18.5);
        PLAYERS[socket.id] = player;
        SOCKET_LIST[socket.id] = socket;
        playerNo += 1;
    }
    else{
       var player = new ControllerPlayer(socket.id,"Player1",47,0,-18.5);
        PLAYERS[socket.id] = player;
        SOCKET_LIST[socket.id] = socket;
        playerNo += 1;
    }*/
    
    socket.emit('allRooms',{    
        socket_id : socket.id,
        allRoomsAtCurrentTime: ROOMS
    });
    
    socket.on("newRoom",onNewRoom);
    socket.on("closeRoom",closeRoom);
    socket.on("play",onPlay); 
    socket.on("disconnect",onSocketDisconnect)
	socket.on("move",onMoveClient);
    socket.on("follow",onFollowClient);
    socket.on("attack",onClientAttack);
});

var onNewRoom = function(data){
    var roomName = "Room "+roomNo;
    roomNo++;
    var room = new Room(this.id,roomName,2);
    ROOMS[this.id] = room;
    this.broadcast.emit("newRoom",{
         socket_id : ROOMS[this.id].id,
         maxPlayers : ROOMS[this.id].maxPlayers,
         currentPlayers : ROOMS[this.id].currentPlayers,
         name : ROOMS[this.id].name
    });
}

var closeRoom = function(){
     delete ROOMS[this.id];
     roomNo--;
     this.broadcast.emit("closeRoom",{
         socket_id : this.id,
    });
}
var onPlay = function(){
     var player = new ControllerPlayer(this.id,"Player1",-47,0,18.5,true);
    PLAYERS[this.id] = player;
    this.emit('identify',{     
        x : PLAYERS[this.id].x,
        y : PLAYERS[this.id].y,
        z :PLAYERS[this.id].z,
        name : PLAYERS[this.id].name,
        socket_id : this.id,
        allPlayersAtCurrentTime: PLAYERS
    });

    this.broadcast.emit("anotherplayerconnected",{
        //TO DO find free position on map ( grid)
        socket_id:this.id,
        x : PLAYERS[this.id].x,
        y : PLAYERS[this.id].y,
        z :PLAYERS[this.id].z,
        name : PLAYERS[this.id].name

    });
}

var onSocketDisconnect = function(){
    //TO DO PLAYER IN ROOM CHECKER
     if(PLAYERS[this.id])
    {
        this.broadcast.emit('playerLeft', {
            socket_id : this.id,
            name: PLAYERS[this.id].name
        });
        console.log("Clinet id "+this.id+" disconnected.");
        delete PLAYERS[this.id]
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
    //data.id = this.id;
    console.log(PLAYERS[this.id].id+" is moving to "+JSON.stringify(data));
    PLAYERS[this.id].updatePositions(data);
    this.broadcast.emit("playerMove",{
        socket_id:this.id,
        x : data["x"],
        y : data["y"],
        z : data["z"],
    });
};

var onFollowClient = function(data){
    console.log("Clientul "+this.id+" urmareste pe "+data["idTarget"]);
    
    this.broadcast.emit("followPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
};

var onClientAttack = function(data){
    console.log("Clientul "+this.id+" ataca pe "+data["idTarget"]);
    
    io.sockets.emit("attackPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
}





