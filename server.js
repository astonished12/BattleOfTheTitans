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
    var player = new ControllerPlayer(socket.id,"Player1");
    PLAYERS[socket.id] = player;
    SOCKET_LIST[socket.id] = socket;
    playerNo += 1;
    socket.emit('identify',{
        x : 0,
        y : 0,
        z : 0,
        name : PLAYERS[socket.id].name,
        socket_id : socket.id,
        allPlayersAtCurrentTime: PLAYERS
    });

    socket.broadcast.emit("anotherplayerconnected",{
        //TO DO find free position on map ( grid)
        socket_id:socket.id,
        x : 128,
        y : 0,
        z : 128,
        name : PLAYERS[socket.id].name

    });

    socket.on("disconnect",onSocketDisconnect)
	socket.on("move",onMoveClient);

});


var onSocketDisconnect = function(){
	this.broadcast.emit('playerLeft', {
	    socket_id : this.id,
	    name: PLAYERS[this.id].name
	});
	delete PLAYERS[this.id]
	playerNo --;
}

	
    var onMoveClient = function(data){
        console.log(PLAYERS[this.id]+" is moving to "+JSON.stringify(data));
        this.broadcast.emit("move",data);
    };




