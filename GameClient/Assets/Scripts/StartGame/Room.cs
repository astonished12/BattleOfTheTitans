using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public int currentNumberOfPlayers;
    public int maxNumberOfPlayers;
    public string nameRoom;
    public string idRoom;
    public Room(int _currentNumberOfPlayers, int _maxNumberOfPlayers, string _nameRoom,string _idRoom)
    {
        currentNumberOfPlayers = _currentNumberOfPlayers;
        maxNumberOfPlayers = _maxNumberOfPlayers;
        nameRoom = _nameRoom;
        idRoom = _idRoom;
    }

}
