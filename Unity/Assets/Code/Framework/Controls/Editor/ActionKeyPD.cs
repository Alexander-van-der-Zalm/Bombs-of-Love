using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomPropertyDrawer(typeof(ActionKey))]
public class ActionKeyPD : PropertyDrawer
{
    [SerializeField]
    private XboxButton m_XboxEnum;
    [SerializeField]
    private KeyCode m_KeyCode;
    [SerializeField]
    private string m_IncompleteKeyCode;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        //EditorGUI.BeginProperty(pos, GUIContent.none, prop);

        SerializedProperty tp = prop.FindPropertyRelative("Type");
        SerializedProperty kp = prop.FindPropertyRelative("KeyValue");
        
        // Prefix Label
        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float splitWidth = (pos.width - 45 - 3)/3;

        // Rects
        Rect typeRect       = new Rect(pos.x                            , pos.y, 45, EditorGUIUtility.singleLineHeight);
        Rect valueRect      = new Rect(pos.x + 45 + 2                   , pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
        Rect textRect       = new Rect(pos.x + 45 + 3 + splitWidth      , pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
        Rect enumRect       = new Rect(pos.x + 45 + 4 + 2 * splitWidth  , pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
        Rect xboxEnumRext   = new Rect(pos.x + 45 + 3 + splitWidth      , pos.y, 2 * splitWidth, EditorGUIUtility.singleLineHeight);

        // Draw
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(typeRect, tp, GUIContent.none);
        if(EditorGUI.EndChangeCheck())
        {
            kp.stringValue = "";// Switched type
        }
        ControlType type = (ControlType)Enum.Parse(typeof(ControlType), tp.enumNames[tp.enumValueIndex], true);
        
        if (type == ControlType.PC)
        {
            // If just switched type
            if (kp.stringValue == "")
            {
                kp.stringValue = ((KeyCode)0).ToString();
            }
            // If just loaded
            if (m_KeyCode.ToString() != kp.stringValue)
            {
                m_KeyCode = ControlHelper.ReturnKeyCode(kp.stringValue);
                m_IncompleteKeyCode = kp.stringValue;
            }
           
            // Enum change check & resolve
            EditorGUI.BeginChangeCheck();
            m_KeyCode = (KeyCode)EditorGUI.EnumPopup(enumRect, m_KeyCode);
            if (EditorGUI.EndChangeCheck())
            {
                kp.stringValue = m_KeyCode.ToString();
                m_IncompleteKeyCode = kp.stringValue;
            }

            // Text field change check & resolve
            EditorGUI.BeginChangeCheck();
            m_IncompleteKeyCode = EditorGUI.TextField(textRect, m_IncompleteKeyCode);
            // Check if it is a valid input
            if (EditorGUI.EndChangeCheck())
            {
                if(!Enum.IsDefined(typeof(KeyCode), m_IncompleteKeyCode))
                {
                    Debug.Log("Not correct");
                }
                else
                {
                    kp.stringValue = m_IncompleteKeyCode;
                    m_KeyCode = ControlHelper.ReturnKeyCode(m_IncompleteKeyCode);
                }
            }
        }
        else
        {
            // When just switched from type or new
            if (kp.stringValue == "")
            {
                kp.stringValue = ((XboxButton)0).ToString();
            }
            // If just loaded
            if (m_XboxEnum.ToString() != kp.stringValue)
            {
                m_XboxEnum = ControlHelper.ReturnXboxButton(kp.stringValue);
            }

            EditorGUI.BeginChangeCheck();
            m_XboxEnum = (XboxButton)EditorGUI.EnumPopup(xboxEnumRext, m_XboxEnum);
            if (EditorGUI.EndChangeCheck())
                kp.stringValue = m_XboxEnum.ToString();
        }
        EditorGUI.PropertyField(valueRect, kp, GUIContent.none);
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        //EditorGUI.EndProperty();

    }
}
