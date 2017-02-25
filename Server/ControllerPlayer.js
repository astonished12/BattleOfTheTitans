/**
 * Created by dan.cehan on 2/21/2017.
 */
  var  ControllerPlayer = function (id, name, coins) {
        this.id = id;
        this.name = name;
        //TO POSITION ON MOVE UPDATE
        this.x = 0;
        this.y = 0;
        this.z = 0;
        this.input = {
            left : false,
            right : false,
            up : false,
            down : false
        }
    };   

module.exports = ControllerPlayer;
