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
        });

    }
}

module.exports = DatabaseManager;