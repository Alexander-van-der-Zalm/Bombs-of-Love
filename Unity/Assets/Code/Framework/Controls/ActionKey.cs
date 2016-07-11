using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum ControlType
{
    PC,
    Xbox
}

[System.Serializable]
public class ActionKey
{
    #region Fields

    public ControlType Type;
    public string KeyValue;

    #endregion

    public ActionKey(ControlType type = ControlType.PC, string value = "A")
    {
        Type = type;
        KeyValue = value;
    }

    #region Down,Pressed,Released

    public bool IsDown(PlayerIndex xbox = PlayerIndex.One)
    {
        switch (Type)
        {
            case ControlType.PC:
                return Input.GetKey(ControlHelper.ReturnKeyCode(KeyValue));
            case ControlType.Xbox:
                return XboxControllerState.ButtonDown(ControlHelper.ReturnXboxButton(KeyValue), xbox);
            default:
                return false;
        }
    }

    public bool IsPressed(PlayerIndex xbox = PlayerIndex.One)
    {
        switch (Type)
        {
            case ControlType.PC:
                return Input.GetKeyDown(ControlHelper.ReturnKeyCode(KeyValue));
            case ControlType.Xbox:
                return XboxControllerState.ButtonPressed(ControlHelper.ReturnXboxButton(KeyValue), xbox);
            default:
                return false;
        }
    }

    public bool IsReleased(PlayerIndex xbox = PlayerIndex.One)
    {
        switch (Type)
        {
            case ControlType.PC:
                return Input.GetKeyUp(ControlHelper.ReturnKeyCode(KeyValue));
            case ControlType.Xbox:
                return XboxControllerState.ButtonReleased(ControlHelper.ReturnXboxButton(KeyValue), xbox);
            default:
                return false;
        }
    }

    #endregion

    #region Creates

    public static ActionKey XboxButton(XboxButton btn)
    {
        return XboxButton(btn.ToString());
    }

    public static ActionKey XboxButton(string btn)
    {
        return new ActionKey(ControlType.Xbox, btn);
    }

    public static ActionKey PCKey(KeyCode kc)
    {
        return PCKey(kc.ToString());
    }

    public static ActionKey PCKey(string kc)
    {
        return new ActionKey(ControlType.PC, kc);
    }

    #endregion
}
