using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(BMGrid))]
public class BMGridEditor :Editor
{
    BMGrid grid;

    void OnEnable()
    {
        grid = (BMGrid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Grid"))
            grid.GenerateGrid();
        if (GUILayout.Button("Generate Blocks"))
            grid.GenerateBlocks();
        if (GUILayout.Button("Destroy Grid"))
            BMGrid.DeleteChildren(grid.transform);
        if (GUILayout.Button("Debug Grid"))
            grid.DebugArray();
        if (GUILayout.Button("Restore Grid"))
            grid.RestoreReferences();
    }
}
