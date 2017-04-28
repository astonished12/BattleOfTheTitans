var shortid = require('shortid');

var  Room = function (io,id,name, maxPlayers) {
        this.io = io;
        this.id = id;
        this.maxPlayers = maxPlayers;
        this.name = name;
        this.currentPlayers = 1;
        this.PLAYERS = {};
        this.confirmedCharacters = 0;
        this.towersId = [];
        this.minionsId = [];
        this.lastTimeSpawn = -999;
        this.onceCalled = false;
        self = this;
    };

Room.prototype.SpawnMinions = function(){
    var timer = new Date();
    if(timer.getTime() - self.lastTimeSpawn > Math.pow(2,4)*1000 && self.lastTimeSpawn > 0 )
    {
        self.lastTimeSpawn = timer.getTime();
        console.log("O SA SPAWNEZ MINIONI "+self);
        self.GenerateIdForMinions();
       
            self.io.sockets.in(self).emit("spawnMinions",{
            minionsId : self.minionsId
        });
    
    }
}

Room.prototype.GenerateIdForMinions = function(){
    self.minionsId = [];
    self.minionsId.push(shortid.generate());
    self.minionsId.push(shortid.generate());
    self.minionsId.push(shortid.generate());
    self.minionsId.push(shortid.generate());
    self.minionsId.push(shortid.generate());
    self.minionsId.push(shortid.generate());
}
    
module.exports = Room;