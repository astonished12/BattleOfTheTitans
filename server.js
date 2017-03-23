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
var PLAYERS = {};
var SOCKET_LIST = {};
var ControllerPlayer = require("./Server/ControllerPlayer");
    
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
    socket.on("newRoom",onNewRoom);
    socket.on("play",onPlay); 
    socket.on("disconnect",onSocketDisconnect)
	socket.on("move",onMoveClient);
    socket.on("follow",onFollowClient);
    socket.on("attack",onClientAttack);
});

var onNewRoom = function(){
    this.broadcast.emit("newRoom");
}
var onPlay = function(){
     var player = new ControllerPlayer(this.id,"Player1",-47,0,18.5);
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
	this.broadcast.emit('playerLeft', {
	    socket_id : this.id,
	    name: PLAYERS[this.id].name
	});
    console.log("Clinet id "+this.id+" disconnected.");
	delete PLAYERS[this.id]
	playerNo --;
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





