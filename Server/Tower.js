var  Tower = function () {
    this.maxHealth = 100;
    this.currentHealth = 100;
    this.isAlive = true;
};

Tower.prototype.getDamage = function(x){
    this.currentHealth -= x;
}
Tower.prototype.checkIsAlive = function(){
    if(this.currentHealth<0){
        this.isAlive = false;
    }
    return this.isAlive;
}

module.exports = Tower;