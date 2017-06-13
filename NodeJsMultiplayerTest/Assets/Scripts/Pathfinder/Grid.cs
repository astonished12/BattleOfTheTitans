using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    private GameObject player;
    public GameObject listOfCharacter;

    Node[,] gridTable;

    int gridSizeX, gridSizeY;

    public int costOfNonDiagonalMovement = 10;
    public int costOfDiagonalMovement = 14;

    void Awake()
    {
        player = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y);
        MakeGrid();
    }

    void MakeGrid()
    {
        gridTable = new Node[gridSizeX, gridSizeY];
        /// 0,0,0 - 1,0,0* max(x)/2 - 0,0,1 * max(y)
        Vector3 worldBottomLeft = transform.position - new Vector3(1,0,0) * gridWorldSize.x / 2 - new Vector3(0,0,1) * gridWorldSize.y / 2;

         for (int x = 0; x < gridSizeX; x++)
         {
             for (int y = 0; y < gridSizeY; y++)
             {
                Vector3 pointInWorldMap = worldBottomLeft + new Vector3(1, 0, 0) * (x+0.5f) + new Vector3(0, 0, 1) * (y+0.5f);
                bool checkIsWalkble = !(Physics.CheckSphere(pointInWorldMap, 0.5f, unwalkableMask));
                gridTable[x, y] = new Node(checkIsWalkble, pointInWorldMap, new Vector2(x,y));
            }
         }
    }


    //This heuristic is used for 8-way movement when the cost of diagonal movement differs from the non-diagonal cost. 
    // Remember that the cost of diagonal distance doesn’t need to be exact and is usually worth it to use a constant 
    //  multiplier rather than the square root as the square root operation is quite expensive.
    internal int GetHeuristicDistance(Node start, Node target)
    {
        //x10 work with int 

        int distanceOfXCoordinate = (int)Mathf.Abs(start.gridPosition.x - target.gridPosition.x);
        int distanceOfYCoordinate = (int)Mathf.Abs(start.gridPosition.y - target.gridPosition.y);

        if (distanceOfXCoordinate > distanceOfYCoordinate)
            return costOfDiagonalMovement * distanceOfYCoordinate + 10 * (distanceOfXCoordinate - distanceOfYCoordinate);

        return costOfDiagonalMovement * distanceOfXCoordinate + costOfNonDiagonalMovement * (distanceOfYCoordinate - distanceOfXCoordinate);
    }


    public Node GetNodeFromWorldPoint(Vector3 positionOnWorldMap)
    {
        float percentX = (positionOnWorldMap.x + gridWorldSize.x / 2) / gridWorldSize.x;
        //3d to 2d z = y . Y nu se modifica;
        float percentY = (positionOnWorldMap.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        if (gridTable[x, y].walkable)
            return gridTable[x, y];
        else
            return GetNeighborsByNode(gridTable[x, y])[0];
    }


    public List<Node> GetNeighborsByNode(Node node){

        List<Node> neighbors = new List<Node>();

        for (int i=-1;i<2;i++)
        {
            for(int j=-1;j<2;j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int auxPosX = i+(int)node.gridPosition.x;
                int auxPosY = j + (int)node.gridPosition.y;
                if (CheckIsPositionAreValid(auxPosX, auxPosY) && gridTable[auxPosX,auxPosY].walkable)
                        neighbors.Add(gridTable[auxPosX,auxPosY]);
            }
        }
    
        return neighbors;
    }

    public bool CheckIsPositionAreValid(int x,int y)
    {
        return x >= 0 && x <= gridSizeX - 1 &&  y >= 0 && y <= gridSizeY - 1;
    }

    public bool CheckIsADiagonalNeighbour(Node node,Node testNode){
            try
            {
                return gridTable[(int)node.gridPosition.x - 1, (int)node.gridPosition.y - 1] == testNode || gridTable[(int)node.gridPosition.x + 1, (int)node.gridPosition.y + 1] == testNode || gridTable[(int)node.gridPosition.x + 1, (int)node.gridPosition.y] == testNode || gridTable[(int)node.gridPosition.x, (int)node.gridPosition.y + 1] == testNode;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        return false;
        }

    public void ResetGridTable()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                gridTable[x, y].hCost = 0;
                gridTable[x, y].gCost = 0;
                gridTable[x, y].fCost = 0;
                gridTable[x, y].parent = null;
            }
        }
    }
    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));


        if (gridTable != null)
        {
            foreach (Node n in gridTable)
            {
                if (n == GetNodeFromWorldPoint(player.transform.position))
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (1));
                }
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (1));
            }
        }
    }
}
