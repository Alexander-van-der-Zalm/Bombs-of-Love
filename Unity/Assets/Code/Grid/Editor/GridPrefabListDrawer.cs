using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(GridPrefabList))]
public class GridPrefabListDrawer : PropertyDrawer
{
    private ReorderableList prefabList;
    private ReorderableList layerList;
    private GridPrefabList gpl;
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

        return prefabList.GetHeight()+layerList.GetHeight()+ EditorGUIUtility.singleLineHeight + sy * 2;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop);
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy, pos.width, EditorGUIUtility.singleLineHeight), prop);
        prefabList.DoList(new Rect(pos.x,pos.y+ sy*2 + EditorGUIUtility.singleLineHeight, pos.width, prefabList.GetHeight()));
        layerList.DoList((new Rect(pos.x, pos.y+ sy *2+ EditorGUIUtility.singleLineHeight + prefabList.GetHeight(), pos.width, layerList.GetHeight())));
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