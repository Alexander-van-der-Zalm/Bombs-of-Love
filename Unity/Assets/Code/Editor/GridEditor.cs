using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(Grid))]
public class GridEditor :Editor
{
    Grid grid;

    void OnEnable()
    {
        grid = (Grid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Grid"))
            grid.GenerateGrid();

    }
}
