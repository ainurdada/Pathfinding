using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AStar))]
public class PathRequestManager : MonoBehaviour
{
    public GameObject gridObject;

    Queue<Request> requests = new Queue<Request>();
    Request currentRequest;

    public static PathRequestManager instance;
    AStar pathfinding;

    bool isProccessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<AStar>();

        if (gridObject.TryGetComponent<Grid>(out Grid grid))
        {
            pathfinding.grid = grid;
        }
    }
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        Request newRequest = new Request(pathStart, pathEnd, callback);
        instance.requests.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProccessingPath && requests.Count > 0)
        {
            currentRequest = requests.Dequeue();
            isProccessingPath = true;
            pathfinding.StartFindPath(currentRequest.startPoint, currentRequest.endPoint);
        }
    }

    public void FinishedProcessPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProccessingPath = false;
        TryProcessNext();
    }

    struct Request
    {
        public Vector3 startPoint;
        public Vector3 endPoint;
        public Action<Vector3[], bool> callback;

        public Request(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
        {
            this.startPoint = start;
            this.endPoint = end;
            this.callback = callback;
        }
    }
}
