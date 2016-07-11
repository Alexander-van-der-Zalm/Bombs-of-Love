using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(ControlScheme))]
public class ControlSchemePropertyDrawer : PropertyDrawer
{
    private ControlScheme scheme;
    private ReorderableList horAxis, verAxis;
    private List<ReorderableList> actions;

    private void Init(SerializedProperty prop)
    {


        if (actions == null)
            actions = new List<ReorderableList>();


    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop);

        EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight), prop);
    }

    #region Height

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 1 * EditorGUIUtility.singleLineHeight;

        for (int i = 0; actions != null && i < actions.Count; i++)
            height += listHeight(actions[i]);

        return height + listHeight(horAxis) + listHeight(verAxis);
    }

    private float listHeight(ReorderableList l)
    {
        if (l != null)
            return l.GetHeight();
        return 0;
    }

    #endregion
}
