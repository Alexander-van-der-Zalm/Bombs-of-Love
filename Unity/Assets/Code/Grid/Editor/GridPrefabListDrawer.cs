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
        //return base.GetPropertyHeight(property, label);
        Init(prop);

        return prefabList.GetHeight()+layerList.GetHeight();
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        if (gpl == null)
            gpl = fieldInfo.GetValue(prop.serializedObject.targetObject) as GridPrefabList;

        //if (prefabList == null)
        //{
        //    prefabList = new ReorderableList(gpl.PrefabList, typeof(List<GridLayer>), true, true, true, true);
        //    //prefabList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(PrefabListDrawElementCB);
        //}
        Init(prop);

        prefabList.DoList(new Rect(pos.x,pos.y,pos.width, prefabList.GetHeight()));
        layerList.DoList((new Rect(pos.x, pos.y + prefabList.GetHeight(), pos.width, layerList.GetHeight())));
        //base.OnGUI(pos, prop, label);
        //EditorGUILayout.LabelField("Text");
        //EditorGUI.Slider(new Rect(pos.x, pos.y + pos.height, pos.width, pos.height),label,  1, 1, 1);
    }

    private void PrefabListDrawElementCB(Rect rect, int index, bool isactive, bool isfocused)
    {
        int lW1 = 20;
        int lW2 = 30;
        int lW3 = 15;

        Debug.Log(prefabList);
        SerializedProperty element = prefabList.serializedProperty.GetArrayElementAtIndex(index);//new UnityEditor.SerializedProperty(gpl.PrefabList[index]);
        EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width - lW1 - 2 * lW2 - lW3, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Prefab"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width - lW1 - 2 * lW2 - lW3, rect.y, lW1, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("GridLayer"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + 2 + rect.width - 2 * lW2 - lW3, rect.y, lW2, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("TileOffsetX"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width - lW2 - lW3, rect.y, lW2, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("TileOffsetY"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width - 1 * lW3 + 2, rect.y, lW3, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("Traversable"), GUIContent.none);
    }
}


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