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
    var player = new ControllerPlayer(socket.id,"Player1",1000);
    PLAYERS[socket.id] = player;
    SOCKET_LIST[socket.id] = socket;
    playerNo += 1;
    //console.log(PLAYERS);
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
        x : 64,
        y : 64,
        name : PLAYERS[socket.id].name

    });

    socket.on("ActionKey",onMoveClient);
    socket.on("disconnect",onSocketDisconnect)
	

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
	//console.log(PLAYERS[this.id]);
	if(data.keyPressed=="S") {
	    PLAYERS[this.id].input.down = true;
	}
	if(data.keyPressed=="W") {
	    PLAYERS[this.id].input.up = true;
	}
	if(data.keyPressed=="A") {
	    PLAYERS[this.id].input.left = true;
	}
	if(data.keyPressed=="D") {
	    PLAYERS[this.id].input.right = true;
	}
}
/*
setInterval(function() {
var pack = [];
for (var i in PLAYERS) {
    var player = PLAYERS[i];
    //player.updatePosition();
    pack.push({
        socket_id : player.id,
        cursors: player.input
    });
    player.input = {
        left : false,
        right : false,
        up : false,
        down : false
    }
}
for (var i in SOCKET_LIST) {
    var socket = SOCKET_LIST[i];
    socket.emit('handleKeys', pack);
}
},40)*/


