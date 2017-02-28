using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinder : MonoBehaviour {

    public Transform seeker, target;
    private Grid gridTable;

    private void Awake(){
        gridTable = GetComponent<Grid>();
    }

    public void Update()
    {
        AStar(seeker.position, target.position);
    }

    public List<Node> AStar(Vector3 startPosition, Vector3 targetPosition){
        Node start = gridTable.GetNodeFromWorldPoint(startPosition);
        Node target = gridTable.GetNodeFromWorldPoint(targetPosition);
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(start);
        start.gCost = 0;
        start.hCost = gridTable.GetHeuristicDistance(start, target);
        start.fCost = start.gCost + start.hCost;
        while (openList.Count > 0) {
            Node currentNode = GetNodeWithLowestCFost(openList);
            if (currentNode.Equals(target)) {
                return ConstructPath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbors = gridTable.GetNeighborsByNode(currentNode);
            foreach (Node neighbour in neighbors)
            {
                int costToCurrentNodeToHisNeighbour = currentNode.gCost + gridTable.GetHeuristicDistance(currentNode, neighbour);

                if (costToCurrentNodeToHisNeighbour < neighbour.gCost && openList.Contains(neighbour))
                    openList.Remove(neighbour);
                if (closedList.Contains(neighbour) && costToCurrentNodeToHisNeighbour < neighbour.gCost)
                    closedList.Remove(neighbour);

                if (!closedList.Contains(neighbour) && !openList.Contains(neighbour))
                {
                    neighbour.gCost = costToCurrentNodeToHisNeighbour;
                    openList.Add(neighbour);
                    neighbour.hCost = gridTable.GetHeuristicDistance(neighbour, target);
                    neighbour.parent = currentNode;
                }

               

            }
        } 
        
        return new List<Node>();
    }

    public Node GetNodeWithLowestCFost(List<Node> nodes){
        Node nodeWithLowestFCost = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < nodeWithLowestFCost.fCost || nodes[i].fCost == nodeWithLowestFCost.fCost && nodes[i].hCost < nodeWithLowestFCost.hCost)
                    nodeWithLowestFCost = nodes[i];
            
        }
        return nodeWithLowestFCost;
    }

    public List<Node> ConstructPath(Node node)
    {
        List<Node> path = new List<Node>();
        path.Add(node);
        while (node.parent != null)
        {
            node = node.parent;
            path.Add(node);
        }
         path.Reverse();

        gridTable.path = path;
        return path;
    }
}
