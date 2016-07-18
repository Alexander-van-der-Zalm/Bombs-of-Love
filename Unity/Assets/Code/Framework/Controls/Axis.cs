using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

[System.Serializable]
public class Axis //: UnityEngine.Object
{
    #region Fields

    public string Name;
    public List<AxisKey> AxisKeys;

    [SerializeField]
    private int lastAxis;
    [SerializeField,ReadOnly]
    private ControlType lastInputType = ControlType.PC;
    [SerializeField]
    private PlayerIndex xbox;

    #endregion

    public Axis(PlayerIndex xbox = PlayerIndex.One, string name = "defaultAxis")
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

    #region Creates

    public void XboxAxis(XboxAxis axis)
    {
        AxisKeys.Add(AxisKey.XboxAxis(axis));
    }

    //public void XboxAxis(string axis)
    //{
    //    AxisKeys.Add(AxisKey.XboxAxis(axis));
    //}

    public void XboxDpad(DirectionInput horizontalOrVertical)
    {
        AxisKeys.Add(AxisKey.XboxDpad(horizontalOrVertical));
    }

    public void PC(string neg, string pos)
    {
        AxisKeys.Add(AxisKey.PC(neg,pos));
    }

    public void PC(KeyCode neg, KeyCode pos)
    {
        AxisKeys.Add(AxisKey.PC(neg, pos));
    }

    public void DefaultInput(DirectionInput horintalOrVertical, PlayerIndex xbox = PlayerIndex.One, string name = "")
    {
        switch (horintalOrVertical)
        {
            case DirectionInput.Horizontal:
                PC("A", "D");
                PC(KeyCode.LeftArrow, KeyCode.RightArrow);
                XboxAxis(global::XboxAxis.LeftX);
                XboxDpad(DirectionInput.Horizontal);
                break;
            case DirectionInput.Vertical:
                PC("S", "W");
                PC(KeyCode.DownArrow, KeyCode.UpArrow);
                XboxAxis(global::XboxAxis.LeftY);
                XboxDpad(DirectionInput.Vertical);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Setup wasd, arrows, dpad, thumbstick
    /// </summary>
    /// <param name="horintalOrVertical"></param>
    /// <param name="xbox"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Axis Default(DirectionInput horintalOrVertical, PlayerIndex xbox = PlayerIndex.One, string name = "")
    {
        Axis ax = new Axis(xbox, name);
        ax.DefaultInput(horintalOrVertical, xbox, name);
        return ax;
    }



    #endregion
}

