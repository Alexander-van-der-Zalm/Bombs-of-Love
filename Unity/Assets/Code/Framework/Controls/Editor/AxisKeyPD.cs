using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(AxisKey))]
public class AxisKeyPD : PropertyDrawer
{
    KeyCodeEditorGUI m_KCNeg, m_KCPos;
    XboxAxisEnumEditorGUI m_XANeg, m_XAPos;

    private void Init(SerializedProperty keys)
    {
        if (keys.arraySize == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                keys.arraySize++;
                keys.GetArrayElementAtIndex(i).stringValue = "";
            }
        }

        if (m_KCNeg == null)
            m_KCNeg = new KeyCodeEditorGUI();
        if (m_KCPos == null)
            m_KCPos = new KeyCodeEditorGUI();

        if (m_XANeg == null)
            m_XANeg = new XboxAxisEnumEditorGUI();
        if (m_XAPos == null)
            m_XAPos = new XboxAxisEnumEditorGUI();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight *3;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        // Draw prefix label - if not a list item
        bool listItem = label.text.Contains("Element");
        if (!listItem)
            pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        // Get relevant properties
        SerializedProperty keys = prop.FindPropertyRelative("keys");
        SerializedProperty type = prop.FindPropertyRelative("Type");
        SerializedProperty k0 = keys.GetArrayElementAtIndex(0);
        SerializedProperty k1 = keys.GetArrayElementAtIndex(1);

        Init(keys);


        var indent = EditorGUI.indentLevel;
        if(!listItem)
            EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(new Rect(pos.x,pos.y,pos.width,EditorGUIUtility.singleLineHeight), type,GUIContent.none);
        if(!listItem)
            EditorGUI.indentLevel = indent;


        pos.y += EditorGUIUtility.singleLineHeight;
        if (k0 != null)
            m_KCNeg.OnGUI(pos, k0, "+", !listItem);
        pos.y += EditorGUIUtility.singleLineHeight;
        if (k1 != null) 
            m_XAPos.OnGUI(pos, k1, "-", !listItem);

    }
}
