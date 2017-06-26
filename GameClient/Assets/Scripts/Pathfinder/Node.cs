using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{

    public bool walkable;
    public Vector3 worldPosition;
    public Vector2 gridPosition;


    public Node parent;
    public int gCost;
    public int hCost;
    public int fCost;

    public int heapIndex { get; set; }
 

    public Node(bool _walkable,Vector3 _worldPosition,Vector2 _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridPosition = _gridPosition;
    }


    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
