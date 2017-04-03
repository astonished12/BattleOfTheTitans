/**
 * Created by dan.cehan on 2/21/2017.
 */
  var  ControllerPlayer = function (id, name, x,y,z,Room) {
        this.id = id;
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
        //TO POSITION ON MOVE UPDATE
        this.Room = Room;
        this.characterNumber = -99;
    };

ControllerPlayer.prototype.updatePositions = function(data){
    this.x = data["x"];
    this.y = data["y"];
    this.z = data["z"];
}

module.exports = ControllerPlayer;
