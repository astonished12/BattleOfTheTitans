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
        this.connection.query('SELECT * from user where username=?',_username, function(err, rows, fields) {
            if (err)
                cb(err);

            console.log('The solution is: ', rows);
            if(rows.length==0)
            {
                console.log("Am intrat aici");
                var myUser = { username: _username, password: _password, email : _email };
                self.connection.query('INSERT INTO user SET ?', myUser, function(err,res){
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
        this.connection.query('SELECT * from user where username=? and password=?',[_username,_password], function(err, rows, fields) {
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

    this.InsertFriend = function(myId, nameFriend, cb){
        var _idUser, _idFriend;
        console.log(" Idu meu este "+myId);
        this.GetIdFromUserByName(nameFriend, function(param){
            if(param!=-1){
                _idFriend = param;
                console.log(myId +" "+_idFriend);
            }
        });
        
        
      
        
        /*var newFriendship = { idUser: _idUser, idFriend: _idFriend};
        this.connection.query('INSERT INTO friends SET ?', newFriendship, function(err,res){
            if(err) throw err;
            succes = true;
            //console.log('Last insert ID:', res.insertId);
            cb();
        }); */         
    } 
    
    this.GetIdFromUserByName = function(nameUser, callback){
         var complete = false;
         this.connection.query('SELECT * from user where username=?',nameUser, function(err, rows, fields) {
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

}




module.exports = DatabaseManager;