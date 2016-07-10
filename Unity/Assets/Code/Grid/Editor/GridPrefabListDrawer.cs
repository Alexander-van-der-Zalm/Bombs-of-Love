using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(GridPrefabList))]
public class GridPrefabListDrawer : PropertyDrawer
{
    private ReorderableList prefabList;
    private ReorderableList layerList;
    private SerializedObject so;
    private GridPrefabList gpl;

    private int sy = 5;

    private GridPrefabListEditor editor;

    private void Init(SerializedProperty prop)
    {
        if (so == null)
            so = new SerializedObject(prop.objectReferenceValue);

        if (gpl == null)
            gpl = prop.objectReferenceValue as GridPrefabList;

        if(editor == null)
            editor = Editor.CreateEditor(prop.objectReferenceValue) as GridPrefabListEditor;

        if (prefabList == null)
            prefabList = editor.InitializePrefabList(so);
            
        if (layerList == null)
            layerList = editor.InitializeLayerList(so);
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        Init(prop);

        return prefabList.GetHeight() + layerList.GetHeight() + 2* EditorGUIUtility.singleLineHeight + sy * 2;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop);
        so = new SerializedObject(prop.objectReferenceValue);
        so.Update();
        
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy, pos.width, EditorGUIUtility.singleLineHeight), prop);
        layerList.DoList((new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight, pos.width, layerList.GetHeight())));
        prefabList.DoList(new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight + layerList.GetHeight(), pos.width, prefabList.GetHeight()));

        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight + layerList.GetHeight() + prefabList.GetHeight(), pos.width, EditorGUIUtility.singleLineHeight), so.FindProperty("SelectedIndex"));

        so.ApplyModifiedProperties();
    }
}

        //if (gpl == null)
        //    gpl = fieldInfo.GetValue(prop.serializedObject.targetObject) as GridPrefabList;
//public class GridPrefabList : ScriptableObject
//{
//    public List<GridLayer> GridLayers;
//    public List<GridPrefab> PrefabList;
//}

//[System.Serializable]
//public class GridLayer
//{
//    [ReadOnly]
//    public int Layer = 0;
//    public string Name = "Tile";
//    public bool AllowMultiplePerCoord = true;
//}

//[System.Serializable]
//public class GridPrefab
//{
//    public GameObject Prefab;
//    public int GridLayer;
//    public float TileOffsetX;
//    public float TileOffsetY;
//    public bool Traversable;
//}