var shortid = require('shortid');

var  Room = function (id,name, maxPlayers) {
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
        this.towers = {};
        this.minions = {};
    };

Room.prototype.SpawnMinions = function(io){
    var timer = new Date();
    if(timer.getTime() - this.lastTimeSpawn > 15*1000 && this.lastTimeSpawn > 0 && this.spawnCheck===false)
    {
        this.lastTimeSpawn = timer.getTime();
        console.log("O SA SPAWNEZ MINIONI IN CAMERA "+this.name);
        this.GenerateIdForMinions();  
        io.to(this.name).emit("spawnMinions",{
                minionsId : this.minionsId
        });
        //this.spawnCheck = true;  
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
Room.prototype.GetOponentName = function(){
    var playerName;
    for(var player in this.PLAYERS){
            if(PLAYERS[player].isOwner===false){
                playerName = PLAYERS[player].name;
            }
    }
    return playerName;
}
Room.prototype.GetOwnerName = function(){
    var playerName;
    for(var player in this.PLAYERS){
            if(PLAYERS[player].isOwner===true){
                playerName = PLAYERS[player].name;
            }
    }
    return playerName;
}

Room.prototype.CloseRoom = function(dbManager){
        console.log(this);
        var ownerName = this.GetOwnerName();
        var oponentName = this.GetOponentName();
       
        dbManager.InserIntoUsers(this.PLAYERS[player].name,data["password"],data["email"], function(err) {
            if(err)
                console.log(err);
            if(err==="duplicate")
                socket.emit("usernameExist");
            else
                socket.emit("registerSuccesfull");
        });
    
}
module.exports = Room;