using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class AStar : MonoBehaviour
{
    [HideInInspector]
    public Grid grid;
    public void StartFindPath(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FindPath(startPoint, endPoint));
    }
    IEnumerator FindPath(Vector3 startPosition, Vector3 taregtPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        bool pathSuccess = false;
        Vector3[] waypoints = new Vector3[0];
        if (grid != null)
        {
            Node startNode = grid.NodeFromWorldPoint(startPosition);
            Node taregtNode = grid.NodeFromWorldPoint(taregtPosition);
            if (startNode.walkable && taregtNode.walkable)
            {
                Heap<Node> openSet = new Heap<Node>(100);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == taregtNode)
                    {
                        sw.Stop();
                        UnityEngine.Debug.Log("find time = " + sw.ElapsedMilliseconds);
                        pathSuccess = true;
                        openSet.ResetIndexes();
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                        int newMovementCostToNeighbour = currentNode.gCost + grid.GetDistance(currentNode, neighbour);
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = grid.GetDistance(neighbour, taregtNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                        }
                    }
                }
            }
            yield return null;
            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, taregtNode);
            }
        }
        PathRequestManager.instance.FinishedProcessPath(waypoints, pathSuccess);
        yield return null;
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
}
