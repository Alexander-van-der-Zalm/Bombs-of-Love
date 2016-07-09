using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomPropertyDrawer(typeof(GridData))]
public class GridDataDrawer : PropertyDrawer
{
    private SerializedObject so;

    private void Init(SerializedProperty prop)
    {
        if (so == null)
            so = new SerializedObject(prop.objectReferenceValue);
    }

    //public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    //{
    //    Init(prop);

    //    return 500;
    //}

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 10;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop);
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + 5 ,pos.width,EditorGUIUtility.singleLineHeight), prop);

        Editor ed = Editor.CreateEditor(prop.objectReferenceValue);
        ed.DrawDefaultInspector();
        
        //Debug.Log(so.FindProperty())
        //EditorGUI.PropertyField(pos, so.);
    }
}
