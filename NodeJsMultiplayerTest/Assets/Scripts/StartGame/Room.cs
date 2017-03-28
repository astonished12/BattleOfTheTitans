using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public int currentNumberOfPlayers;
    public int maxNumberOfPlayers;
    public string nameRoom;

    NetworkEntity networkEntity;
    public Room(int _currentNumberOfPlayers, int _maxNumberOfPlayers, string _nameRoom)
    {
        currentNumberOfPlayers = _currentNumberOfPlayers;
        maxNumberOfPlayers = _maxNumberOfPlayers;
        nameRoom = _nameRoom;
    }

}
