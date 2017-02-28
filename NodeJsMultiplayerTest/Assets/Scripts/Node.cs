using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {

    public bool walkable;
    public Vector3 worldPosition;
    public Vector2 gridPosition;


    public Node parent;
    public int gCost;
    public int hCost;
    public int fCost;
    
    public Node(bool _walkable,Vector3 _worldPosition,Vector2 _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridPosition = _gridPosition;
    }
}
