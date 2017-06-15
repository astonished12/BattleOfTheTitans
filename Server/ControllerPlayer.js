/**
 * Created by dan.cehan on 2/21/2017.
 */
  var  ControllerPlayer = function (id, name, _x,_y,_z,isOwner) {
        this.id = id;
        this.name = name;
        this.destination = {
            x : 0,
            y : 0,
            z : 0
        }
        this.lastPosition = {
            x : _x,
            y : _y,
            z : _z
        }

        this.totalDamage = 0;
        //TO POSITION ON MOVE UPDATE        
        this.isOwner = isOwner;
        this.characterNumber = -99;
        this.lastMoveTime = 0;
    };

ControllerPlayer.prototype.updateDestination = function(data){
    this.destination.x = data["x"];
    this.destination.y = data["y"];
    this.destination.z = data["z"];
};
ControllerPlayer.prototype.updateLastPosition = function(data){
    this.lastPosition.x = data["x"];
    this.lastPosition.y = data["y"];
    this.lastPosition.z = data["z"];
}

module.exports = ControllerPlayer;
