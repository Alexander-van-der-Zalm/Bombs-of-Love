using UnityEngine;

using XInputDotNetPure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum DirectionInput
{
    Horizontal,
    Vertical
}

[System.Serializable]
public class AxisKey
{
    #region Enums

    public enum AxisKeyType
    {
        PC,
        Dpad,
        Axis
    }

    #endregion

    #region Fields

    public AxisKeyType Type;
    [ReadOnly]
    public string[] keys;

    //[SerializeField,HideInInspector]
    //private int selectedIndex1;
    //[SerializeField, HideInInspector]
    //private int selectedIndex2;

    #endregion

    public AxisKey()
    {
        keys = new string[2];
    }

    #region Creates

    public static AxisKey XboxAxis(XboxAxis axis)
    {
        AxisKey ak = new AxisKey();
        ak.Type = AxisKeyType.Axis;
        ak.keys[0] = axis.ToString();

        //ak.changed();

        return ak;
    }

    public static AxisKey XboxDpad(DirectionInput horintalOrVertical)
    {
        AxisKey ak = new AxisKey();
        ak.Type = AxisKeyType.Dpad;

        if (horintalOrVertical == DirectionInput.Horizontal)
        {
            ak.keys[0] = XboxButton.Left.ToString();
            ak.keys[1] = XboxButton.Right.ToString();
        }
        else
        {
            ak.keys[0] = XboxButton.Down.ToString();
            ak.keys[1] = XboxButton.Up.ToString();
        }

        //ak.changed();

        return ak;
    }

    public static AxisKey PC(string neg, string pos)
    {
        AxisKey ak = new AxisKey();
        ak.Type = AxisKeyType.PC;

        ak.keys[0] = neg;
        ak.keys[1] = pos;

        //ak.changed();

        return ak;
    }

    public static AxisKey PC(KeyCode neg, KeyCode pos)
    {
        return PC(neg.ToString(), pos.ToString());
    }

    #endregion

    #region Value

    public float Value(PlayerIndex xboxController = PlayerIndex.One)
    {
        float v = 0;
        switch (Type)
        {
            case AxisKeyType.PC:
                if (Input.GetKey(ControlHelper.ReturnKeyCode(keys[0])))
                    v--;
                if (Input.GetKey(ControlHelper.ReturnKeyCode(keys[1])))
                    v++;
                break;

            case AxisKeyType.Axis:
                v = XboxControllerState.Axis(ControlHelper.ReturnXboxAxis(keys[0]), xboxController);
                break;

            case AxisKeyType.Dpad:
                if (XboxControllerState.ButtonDown(ControlHelper.ReturnXboxButton(keys[0]), xboxController))
                    v--;
                if (XboxControllerState.ButtonDown(ControlHelper.ReturnXboxButton(keys[1]), xboxController))
                    v++;
                break;

            default:
                return 0;
        }
        return v;
    }

    #endregion

    #region GUI
    #if UNITY_EDITOR

    public static void OnGui(Rect rect, SerializedProperty prop)
    {
        AxisKey key = prop.objectReferenceValue as System.Object as AxisKey;
        EditorGUI.IntField(rect, 100);
    }
    //public void OnGui()
    //{
    //    //EditorGUILayout.BeginHorizontal();

    //    Type = (ControlKeyType)EditorGUILayout.EnumPopup(Type, GUILayout.Width(50.0f));
    //    if (GUI.changed)
    //        changed();

    //    switch(Type)
    //    {
    //        case ControlKeyType.PC:
    //            pcGui();
    //            break;

    //        case ControlKeyType.Xbox:
    //            xboxGui();
    //            break;
    //    }
       
    //    //EditorGUILayout.EndHorizontal();
    //}

    //private void xboxGui()
    //{
    //    xboxAxisType = (XboxAxisType)EditorGUILayout.EnumPopup(xboxAxisType, GUILayout.Width(40.0f));
    //    if (GUI.changed)
    //        changed();

    //    switch(xboxAxisType)
    //    {
    //        case XboxAxisType.axis:
    //            xboxAxisGUI();
    //            break;
    //        case XboxAxisType.dpad:
    //            xboxDpadGUI();
    //            break;
    //    }
    //}

    //#endif

    //private void changed()
    //{
    //    if(keys.Count ==0)
    //        return;

    //    switch (Type)
    //    {
    //        case ControlKeyType.PC:
    //            selectedIndex1 = Enum.GetNames(typeof(KeyCode)).ToList().FindIndex(e => e == keys[0]);
    //            selectedIndex2 = Enum.GetNames(typeof(KeyCode)).ToList().FindIndex(e => e == keys[1]);
    //            break;

    //        case ControlKeyType.Xbox:
    //            switch (xboxAxisType)
    //            {
    //                case XboxAxisType.axis:
    //                    selectedIndex1 = (int)ControlHelper.ReturnXboxAxis(keys[0]);
    //                    break;
    //                case XboxAxisType.dpad:
    //                    selectedIndex1 = (int)ControlHelper.ReturnXboxDPad(keys[0]);
    //                    selectedIndex2 = (int)ControlHelper.ReturnXboxDPad(keys[1]);
    //                    break;
    //            }
    //            break;
    //    }
        
    //}

    //#if UNITY_EDITOR

    //private void xboxDpadGUI()
    //{
    //    while(keys.Count < 2)
    //        keys.Add(ControlHelper.DPadOptions[0]);

    //    EditorGUILayout.LabelField("-", GUILayout.Width(15.0f));
    //    selectedIndex1 = EditorGUILayout.Popup(selectedIndex1, ControlHelper.DPadOptions, GUILayout.Width(60.0f));
    //    keys[0] = ControlHelper.DPadOptions[selectedIndex1];

    //    GUILayout.Space(20.0f);

    //    EditorGUILayout.LabelField("+", GUILayout.Width(15.0f));
    //    selectedIndex2 = EditorGUILayout.Popup(selectedIndex2, ControlHelper.DPadOptions, GUILayout.Width(60.0f));
    //    keys[1] = ControlHelper.DPadOptions[selectedIndex2];
    //}

    //private void xboxAxisGUI()
    //{
    //    while (keys.Count < 1)
    //        keys.Add(ControlHelper.XboxAxixOptions[0]);
    //    GUILayout.Space(20.0f);
    //    //EditorGUILayout.LabelField("Axis", GUILayout.Width(20.0f));
    //    selectedIndex1 = EditorGUILayout.Popup(selectedIndex1, ControlHelper.XboxAxixOptions, GUILayout.Width(80.0f));
    //    keys[0] = ControlHelper.XboxAxixOptions[selectedIndex1];
    //}

    //private void pcGui()
    //{
    //    while (keys.Count < 1)
    //        keys.Add(ControlHelper.KeyCodeOptions[0]);

    //    GUILayout.Space(45.0f);

    //    EditorGUILayout.LabelField("-", GUILayout.Width(15.0f));
    //    selectedIndex1 = EditorGUILayout.Popup(selectedIndex1, ControlHelper.KeyCodeOptions, GUILayout.Width(80.0f));
    //    keys[0] = ControlHelper.KeyCodeOptions[selectedIndex1];
    //    EditorGUILayout.LabelField("+", GUILayout.Width(15.0f));
    //    selectedIndex2 = EditorGUILayout.Popup(selectedIndex2, ControlHelper.KeyCodeOptions, GUILayout.Width(80.0f));
    //    keys[1] = ControlHelper.KeyCodeOptions[selectedIndex2];
    //}
    #endif
    #endregion
}
