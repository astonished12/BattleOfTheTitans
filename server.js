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
var ROOMS = {};
var ControllerPlayer = require("./Server/ControllerPlayer");
var Room = require("./Server/Room.js");
var mapingSocketRoom = {};

io.sockets.on('connection', function(socket){
    console.log('Client connected is '+socket.id);
    
    socket.emit('allRooms',{    
        socket_id : socket.id,
        allRoomsAtCurrentTime: ROOMS
    });
    
    socket.on("newRoom",onNewRoom);
    socket.on("closeRoom",closeRoom);
    socket.on("joinRoom",onJoinRoom);
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
    var player = new ControllerPlayer(this.id,"Player1",-47,0,18.5,true);
    room.PLAYERS[this.id] = player;    
    ROOMS[this.id] = room;
    mapingSocketRoom[this.id] =  ROOMS[this.id];
    this.join(ROOMS[this.id]);
    
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
        ROOMS[data["idRoom"]].currentPlayers++;
        this.join(ROOMS[data["idRoom"]]);
        mapingSocketRoom[this.id] = ROOMS[data["idRoom"]];
        var player = new ControllerPlayer(this.id,"Player1",-47,0,18.5,true);
        ROOMS[data["idRoom"]].PLAYERS[this.id] = player; 
        //console.log(ROOMS[data["idRoom"]].PLAYERS);
        io.sockets.in(ROOMS[data["idRoom"]]).emit("joinSuccesFull",{
               room_id :  data["idRoom"]}); 
    }
   
}
var onPlay = function(){
     console.log(mapingSocketRoom[this.id].PLAYERS);
    this.emit('identify',{     
        x : mapingSocketRoom[this.id].PLAYERS[this.id].x,
        y : mapingSocketRoom[this.id].PLAYERS[this.id].y,
        z :mapingSocketRoom[this.id].PLAYERS[this.id].z,
        name : mapingSocketRoom[this.id].PLAYERS[this.id].name,
        socket_id : this.id,
        allPlayersAtCurrentTime: mapingSocketRoom[this.id].PLAYERS
    });

    /*this.broadcast.in(mapingSocketRoom[this.id]).emit("anotherplayerconnected",{
        //TO DO find free position on map ( grid)
        socket_id:this.id,
        x : mapingSocketRoom[this.id].PLAYERS[this.id].x,
        y : mapingSocketRoom[this.id].PLAYERS[this.id].y,
        z :mapingSocketRoom[this.id].PLAYERS[this.id].z,
        name : mapingSocketRoom[this.id].PLAYERS[this.id].name

    });*/
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
    this.broadcast.in(mapingSocketRoom[this.id]).emit("playerMove",{
        socket_id:this.id,
        x : data["x"],
        y : data["y"],
        z : data["z"],
    });
};

var onFollowClient = function(data){
    console.log("Clientul "+this.id+" urmareste pe "+data["idTarget"]);
    
    this.broadcast.in(mapingSocketRoom[this.id]).emit("followPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
};

var onClientAttack = function(data){
    console.log("Clientul "+this.id+" ataca pe "+data["idTarget"]);
    
    io.sockets.in(mapingSocketRoom[this.id]).emit("attackPlayer",{
        socket_id:this.id,
        target_id:data["idTarget"]
    });
}





