using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(GridPrefabList))]
public class GridPrefabListDrawer : PropertyDrawer
{
    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    return base.GetPropertyHeight(property, label) + 5;
    //}

    //public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    //{
    //    EditorGUI.PropertyField(new Rect(pos.x, pos.y + 5, pos.width, EditorGUIUtility.singleLineHeight), prop);
    //    Editor e = Editor.CreateEditor(prop.objectReferenceValue);//.DrawDefaultInspector();
    //    e.DrawDefaultInspector();
    //}


    private ReorderableList prefabList;
    private ReorderableList layerList;
    private SerializedObject so;

    private int sy = 5;

    private void Init(SerializedProperty prop)
    {
        if (so == null)
            so = new SerializedObject(prop.objectReferenceValue);

        if (prefabList == null)
            prefabList = GridPrefabListEditor.InitializePrefabList(so);

        if (layerList == null)
            layerList = GridPrefabListEditor.InitializeLayerList(so);
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        Init(prop);

        return prefabList.GetHeight() + layerList.GetHeight() + EditorGUIUtility.singleLineHeight + sy * 2;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        so.Update();
        Init(prop);
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy, pos.width, EditorGUIUtility.singleLineHeight), prop);
        layerList.DoList((new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight, pos.width, layerList.GetHeight())));
        prefabList.DoList(new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight + layerList.GetHeight(), pos.width, prefabList.GetHeight()));
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