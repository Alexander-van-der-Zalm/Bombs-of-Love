﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

[System.Serializable]
public class Axis //: Control
{
    [SerializeField]
    public string Name;

    [SerializeField, HideInInspector]
    protected ControlScheme scheme;
    
    [SerializeField]
    public List<AxisKey> AxisKeys;

    [SerializeField]
    private int lastAxis;

    public Axis(ControlScheme scheme, string name = "defaultAxis")
        //: base(scheme, name)
    {
        AxisKeys = new List<AxisKey>();

        this.scheme = scheme;
        this.Name = name;
    }

    public float Value()
    {
        float value = 0;
        int curAxis = 0;

        // Handles priority on last active 
        for(int i = 0; i < AxisKeys.Count; i++)
        {
            float v = AxisKeys[i].Value(scheme.controllerID);
            if (v != 0 && (value == 0 || (value != 0 && lastAxis == i)))
            {
                value = v;
                curAxis = i;
                scheme.InputType = ControlHelper.AxisKeyToControl(AxisKeys[i].Type);
            } 
        }
        lastAxis = curAxis;
        return value;
    }
}

