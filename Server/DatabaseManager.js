var DatabaseManager = function(){
    var mysql      = require('mysql');
    var connection = mysql.createConnection({
      host     : 'localhost',
      user     : 'root',
      password : '',
      database : 'battleoftitans'
    });

    connection.connect();

    connection.query('SELECT * from user', function(err, rows, fields) {
      if (!err)
        console.log('The solution is: ', rows);
      else
        console.log('Error while performing Query.');
    });

    connection.end();
}

module.exports = DatabaseManager;