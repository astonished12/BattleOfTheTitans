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
            if(this.PLAYERS[player].isOwner==='false'){
                playerName = this.PLAYERS[player].name;
            }
    }
    return playerName;
}
Room.prototype.GetOwnerName = function(){
    var playerName;
    for(var player in this.PLAYERS){
            if(this.PLAYERS[player].isOwner==='true'){
                playerName = this.PLAYERS[player].name;
            }
    }
    return playerName;
}

Room.prototype.CheckWin = function(_name){
    var result = false;
     for(var player in this.PLAYERS){
            if(this.PLAYERS[player].win === true && this.PLAYERS[player].name === _name){
                result = true;
            }
    }
    return result;
}

Room.prototype.GetTotalDamageByName = function(_name){
 var total = 0;
     for(var player in this.PLAYERS){
            if(this.PLAYERS[player].name === _name){
                total = this.PLAYERS[player].totalDamage;
            }
    }
    return total;
}

Room.prototype.CloseRoom = function(dbManager){
        console.log(this);
        var ownerName = this.GetOwnerName();
        var statusOwner = this.CheckWin(ownerName);
        var totalDamageOwner = this.GetTotalDamageByName(ownerName);
        var oponentName = this.GetOponentName();
        var statusOponent = this.CheckWin(oponentName);
        var oponenetDamage = this.GetTotalDamageByName(oponenetDamage);
        console.log(ownerName+" "+oponentName+" "+statusOwner+" "+statusOponent);
        dbManager.InsertIntoStatistics(ownerName, oponentName,true,statusOwner, totalDamageOwner,function(err) {
            if(err)
                console.log(err);
            
        });
        dbManager.InsertIntoStatistics(oponentName, ownerName,false,statusOponent,oponenetDamage, function(err) {
            if(err)
                console.log(err);
            
        });
               
}
module.exports = Room;