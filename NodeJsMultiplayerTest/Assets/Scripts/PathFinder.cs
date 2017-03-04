using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinder : MonoBehaviour {

    private Grid gridTable;

    private void Start(){
        GameObject ground = GameObject.Find("Ground");

        gridTable = ground.GetComponent<Grid>();
    }    

    public List<Node> AStar(Vector3 startPosition, Vector3 targetPosition){
        gridTable.ResetGridTable();
        Node start = gridTable.GetNodeFromWorldPoint(startPosition);
        Node target = gridTable.GetNodeFromWorldPoint(targetPosition);
        Heap openList = new Heap(100);
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Insert(start);
        start.gCost = 0;
        start.hCost = gridTable.GetHeuristicDistance(start, target);
        start.fCost = start.gCost + start.hCost;
   

        while (!openList.IsEmpty()) {
            Node currentNode = openList.GetRoot();
            if (currentNode.Equals(target)) {
                
                return ConstructPath(currentNode);
            }

            closedList.Add(currentNode);

            List<Node> neighbors = gridTable.GetNeighborsByNode(currentNode);
            foreach (Node neighbour in neighbors)
            {
                if (closedList.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + gridTable.GetHeuristicDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = gridTable.GetHeuristicDistance(neighbour, target);
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                        openList.Insert(neighbour);
                  
                }


            }
        } 

        return new List<Node>();
    }



    public List<Node> ConstructPath(Node node)
    {
        //Debug.Log("DACA MA REPET DE AICI PICA");

        List<Node> path = new List<Node>();
        path.Add(node);
        while (node.parent != null)
        {
            path.Add(node);
            node = node.parent;
            //path.Add(node);
        }


         path.Reverse();

        gridTable.path = path;
        return path;
    }
}
