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
        if (GUILayout.Button("Generate Blocks"))
            grid.GenerateBlocks();
        if (GUILayout.Button("Destroy Grid"))
            Grid.DeleteChildren(grid.transform);
        if (GUILayout.Button("Debug Grid"))
            grid.DebugArray();
        if (GUILayout.Button("Restore Grid"))
            grid.RestoreReferences();
    }
}
