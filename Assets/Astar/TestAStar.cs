using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public Transform seeker, target;
    public bool DrawWay = false;
    public Vector3 DrawSize;

    Vector3[] way;

    public void FindPath()
    {
        
        PathRequestManager.RequestPath(seeker.position, target.position, Callback);
    }

    public void Callback(Vector3[] waypoints, bool succes)
    {
        if(succes)
        {
            Debug.Log("Success to request path");
            way = waypoints;
        }
        else
        {
            Debug.LogError("Fail to request path");
        }
    }

    private void OnDrawGizmos()
    {
        if(DrawWay && way != null && way.Length > 0)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(seeker.position, way[0]);
            Gizmos.DrawCube(way[0], DrawSize);
            for (int i = 1; i < way.Length; i++)
            {
                Gizmos.DrawCube(way[i], DrawSize);
                Gizmos.DrawLine(way[i-1], way[i]);
            }
        }
    }
}
