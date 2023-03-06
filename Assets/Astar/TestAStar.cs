using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public Transform seeker, target;
    public GameObject ground;
    public void FindPath()
    {
        AStar.grid = ground.GetComponent<Grid>();
        AStar.FindPath(seeker.position, target.position);
    }
}
