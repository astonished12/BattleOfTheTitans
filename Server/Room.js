var shortid = require('shortid');

var  Room = function (id,name, maxPlayers) {
        //this.io = io;
        this.id = id;
        this.maxPlayers = maxPlayers;
        this.name = name;
        this.currentPlayers = 1;
        this.PLAYERS = {};
        this.confirmedCharacters = 0;
        this.towersId = [];
        this.minionsId = [];
        this.lastTimeSpawn = -999;
        this.spawnCheck = false;
        //self = this;
    };

Room.prototype.SpawnMinions = function(io){
    var timer = new Date();
    if(timer.getTime() - this.lastTimeSpawn > Math.pow(2,4)*1000 && this.lastTimeSpawn > 0 && this.spawnCheck===false)
    {
        this.lastTimeSpawn = timer.getTime();
        console.log("O SA SPAWNEZ MINIONI IN CAMERA "+this.name);
        this.GenerateIdForMinions();  
        io.to(this.name).emit("spawnMinions",{
                minionsId : this.minionsId
        });
        this.spawnCheck = true;  
    }
}

Room.prototype.GenerateIdForMinions = function(){
    this.minionsId = [];
    this.minionsId.push(shortid.generate());
    this.minionsId.push(shortid.generate());
    this.minionsId.push(shortid.generate());
    this.minionsId.push(shortid.generate());
    this.minionsId.push(shortid.generate());
    this.minionsId.push(shortid.generate());
}

module.exports = Room;