using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static Grid grid;
    public static void FindPath(Vector3 startPosition, Vector3 taregtPosition)
    {
        if (grid == null) return;

        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node taregtNode = grid.NodeFromWorldPoint(taregtPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 0; i < openSet.Count; i++)
            {
                if (currentNode.fCost > openSet[i].fCost || 
                    currentNode.fCost == openSet[i].fCost && currentNode.hCost > openSet[i].hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == taregtNode)
            {
                RetracePath(startNode, currentNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + grid.GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = grid.GetDistance(neighbour,taregtNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }
        }
    }

    static void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }
}
