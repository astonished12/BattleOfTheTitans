  var  Room = function (id,name, maxPlayers) {
        this.id = id;
        this.maxPlayers = maxPlayers;
        this.name = name;
        this.currentPlayers = 1;
    };



module.exports = Room;