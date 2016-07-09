using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GridPrefabList))]
public class GridPrefabListEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public static void DrawGridPrefabList()
    {

    }
}
