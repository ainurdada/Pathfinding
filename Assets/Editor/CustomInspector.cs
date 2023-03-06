using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            Grid grid = (Grid)target;
            grid.CreateGrid();
        }
    }
}

[CustomEditor(typeof(TestAStar))]
public class TestAStarInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Find Path"))
        {
            TestAStar AStar = (TestAStar)target;
            AStar.FindPath();
        }
    }
}
