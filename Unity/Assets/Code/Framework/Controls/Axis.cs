using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

[System.Serializable]
public class Axis //: Control
{
    public string Name;
    
    public List<AxisKey> AxisKeys;

    [SerializeField]
    private int lastAxis;

    [SerializeField]
    private ControlType lastInputType = ControlType.PC;
    private int xbox;

    public Axis(int xbox = 0, string name = "defaultAxis")
    {
        AxisKeys = new List<AxisKey>();
        this.xbox = 0;
        this.Name = name;
    }

    public float Value()
    {
        float value = 0;
        int curAxis = 0;

        // Handles priority on last active 
        for(int i = 0; i < AxisKeys.Count; i++)
        {
            float v = AxisKeys[i].Value(xbox);
            if (v != 0 && (value == 0 || (value != 0 && lastAxis == i)))
            {
                value = v;
                curAxis = i;
                lastInputType = ControlHelper.AxisKeyToControl(AxisKeys[i].Type);
            } 
        }
        lastAxis = curAxis;
        return value;
    }
}

