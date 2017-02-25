using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public GameObject player;
    Node[,] gridTable;

    int gridSizeX, gridSizeY;

    void Start()
    {
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
                gridTable[x, y] = new Node(checkIsWalkble, pointInWorldMap);
            }
         }
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
        return gridTable[x, y];
    }



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
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (1));
            }
        }
    }
}
