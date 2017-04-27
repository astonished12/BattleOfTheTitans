
var  Room = function (io,id,name, maxPlayers) {
        this.io = io;
        this.id = id;
        this.maxPlayers = maxPlayers;
        this.name = name;
        this.currentPlayers = 1;
        this.PLAYERS = {};
        this.confirmedCharacters = 0;
        this.towersId = [];
        this.lastTimeSpawn = -999;
        self = this;
    };

Room.prototype.SpawnMinions = function(){
    var timer = new Date();
    if(timer.getTime() - self.lastTimeSpawn > Math.pow(10,4) && self.lastTimeSpawn > 0 )
    {
        self.lastTimeSpawn = timer.getTime();
        console.log("O SA SPAWNEZ MINIONI "+self);
        self.io.sockets.in(self).emit("spawnMinions");
    }
}

module.exports = Room;