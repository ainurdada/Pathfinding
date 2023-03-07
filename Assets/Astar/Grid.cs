using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool DisplayPathGizmos;
    public bool DisplayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiametr;
    int gridSizeX, gridSizeY;

    public void CreateGrid()
    {
        nodeDiametr = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiametr);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiametr);

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldbottomLeft +
                    Vector3.right * (x * nodeDiametr + nodeRadius) +
                    Vector3.forward * (y * nodeDiametr + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
        Debug.Log("Grid is ready");
    }

    public Node NodeFromWorldPoint(Vector3 worldPoint)
    {
        float percentX = (worldPoint.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPoint.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return distX > distY ?
            14 * distY + 10 * (distX - distY) :
            14 * distX + 10 * (distY - distX);
    }

    public List<Node> path;
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

    //    if (grid != null)
    //    {
    //        foreach (Node node in grid)
    //        {
    //            Gizmos.color = node.walkable ? Color.white : Color.red;
    //            if (path != null && path.Contains(node) && DisplayPathGizmos) Gizmos.color = Color.black;
    //            else if (!DisplayGridGizmos) continue;
    //            Gizmos.DrawCube(node.worldPosition, new Vector3(nodeDiametr * 0.8f, 1, nodeDiametr * 0.8f));
    //        }
    //    }
    //}
}
