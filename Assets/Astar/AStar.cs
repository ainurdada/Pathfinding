using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class AStar
{
    public static Grid grid;
    public static void FindPath(Vector3 startPosition, Vector3 taregtPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        if (grid == null) return;

        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node taregtNode = grid.NodeFromWorldPoint(taregtPosition);

        Heap<Node> openSet = new Heap<Node>(100);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == taregtNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log("find time = " + sw.ElapsedMilliseconds);
                RetracePath(startNode, currentNode);
                openSet.ResetIndexes();
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

        grid.path = path;
    }
}
