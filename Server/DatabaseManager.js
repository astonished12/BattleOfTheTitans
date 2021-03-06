var DatabaseManager = function(){
    var self = this;
    this.mysql      = require('mysql');
    this.connection = this.mysql.createConnection({
        host     : 'localhost',
        user     : 'root',
        password : '',
        database : 'battleoftitans'
    });

    this.connection.connect();
    this.InserIntoUsers = function(_username, _password, _email, cb){
        this.connection.query('SELECT * from users where username=?',_username, function(err, rows, fields) {
            if (err)
                cb(err);

            console.log('The solution is: ', rows);
            if(rows.length==0)
            {
                console.log("Am intrat aici");
                var myUser = { username: _username, password: _password, email : _email, isOnline : false };
                self.connection.query('INSERT INTO users SET ?', myUser, function(err,res){
                    if(err) throw err;
                    succes = true;
                    console.log('Last insert ID:', res.insertId);
                    cb();
                });          
            }
            else
            {
                cb("duplicate");
            }
        });
    }
    this.CheckLogin = function(_username, _password, cb){
        var myUser = {username: _username, password: _password};
        this.connection.query('SELECT * from users where username=? and password=?',[_username,_password], function(err, rows, fields) {
            if (err)
                cb(err);

            console.log('The solution is: ', rows);
            if(rows.length==0)
            {
                console.log("Datele introduse nu exista");
                cb("fail")
            }
            else
            {
                console.log("Good login "+rows[0]["idUser"]);
                cb("succes",rows[0]["idUser"]);
            }
        });
    }
    this.MakeLoginOnOff = function(_username, status){
        this.connection.query('UPDATE users SET isOnline = ? WHERE username = ?', [status, _username]);
    }
    this.InsertFriend = function(myId, nameFriend, cb){
        var _idUser, _idFriend;
        console.log(" Idu meu este "+myId);
        this.GetIdFromUserByName(nameFriend, function(param){
            if(param!=-1){
                _idFriend = param;
                console.log(myId +" "+_idFriend);

                var newRow = { idUser: myId, idFriend : _idFriend };
                var newRow1 = { idUser: _idFriend, idFriend : myId };

                self.connection.query('INSERT INTO friends SET ?', newRow, function(err,res){
                    if(err) throw err;
                    succes = true;
                    console.log('Last insert ID:', res.insertId);
                });

                self.connection.query('INSERT INTO friends SET ?', newRow1, function(err,res){
                    if(err) throw err;
                    succes = true;
                    console.log('Last insert ID:', res.insertId);
                });     
            }
        });        
    } 
    this.RemoveFriend = function(myId, nameFriend, cb){
        var _idUser, _idFriend;
        console.log(" Idu meu este "+myId);
        this.GetIdFromUserByName(nameFriend, function(param){
            if(param!=-1){
                _idFriend = param;
                console.log(myId +" "+_idFriend);

                var newRow = { idUser: myId, idFriend : _idFriend };
                var newRow1 = { idUser: _idFriend, idFriend : myId };

                self.connection.query('DELETE from friends where idUser=? and idFriend=?', [myId,_idFriend], function(err,res){
                    if(err) throw err;
                    succes = true;
                });

                self.connection.query('DELETE from friends where idUser=? and idFriend=?', [_idFriend,myId], function(err,res){
                    if(err) throw err;
                    succes = true;                    
                });     
            }
        });        
    } 
    this.GetListOfFriendById = function(myId, cb){
        this.connection.query('Select distinct idFriend,username,isOnline from users join friends on users.idUser = friends.idFriend where users.idUser In (SELECT idFriend FROM users u natural join friends f where u.idUser = ?) and users.idUser = friends.idFriend',myId, function(err, rows, fields) {
            if (err)
                cb(err);

            console.log('The solution is: ', rows);
            if(rows.length==0)
            {
                cb("noFriends")
            }
            else
            {                
                cb("Friends",rows);
            }
        });
    }
    this.GetIdFromUserByName = function(nameUser, callback){
        var complete = false;
        this.connection.query('SELECT * from users where username=?',nameUser, function(err, rows, fields) {
            if (err)
            {
                callback(err);
                throw err;
            }
            if(rows.length!=0)
            {
                idUser = rows[0]["idUser"];
                complete = true;
                console.log(idUser);
                callback(idUser);
            }
            else
            {
                idUser = -1;
                callback(-1);
            }           
        });
    }

    this.InsertIntoStatistics = function(_playerId, _namePlayer, _oponentID, _nameOponent, _checkOwner, _status, _totalDamage,cb){
        var newRowStatistics = { idPlayer: _playerId,namePlayer:_namePlayer, oponentId : _oponentID,nameOponent:_nameOponent, owner : _checkOwner, status: _status, totalDamage : _totalDamage };

        self.connection.query('INSERT INTO statistics SET ?', newRowStatistics, function(err){
                if(err) {
                cb(err);
                throw err;
            }
        });
    }

     this.GetListMatchesById = function(myId, cb){
        this.connection.query('SELECT * FROM statistics where idPlayer = ?',myId, function(err, rows, fields) {
            if (err)
                cb(err);

            console.log('The solution is: ', rows);
            if(rows.length==0)
            {
                cb("noMatches")
            }
            else
            {                
                cb("Matches",rows);
            }
        });
    }

}

//Select distinct idFriend,username,isOnline from users join friends on users.idUser = friends.idFriend where users.idUser In (SELECT idFriend FROM users u natural join friends f where u.idUser = 5) and users.idUser = friends.idFriend ;



module.exports = DatabaseManager;