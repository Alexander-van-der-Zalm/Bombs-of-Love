using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum ControlType
{
    PC,
    Xbox
}

[System.Serializable]
public class InputActionKey
{
    #region Fields

    public ControlType Type;
    [ReadOnly]
    public string KeyValue;

    #endregion

    public InputActionKey(ControlType type = ControlType.PC, string value = "A")
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
                return Input.GetKey(InputHelper.ReturnKeyCode(KeyValue));
            case ControlType.Xbox:
                return XboxControllerState.ButtonDown(InputHelper.ReturnXboxButton(KeyValue), xbox);
            default:
                return false;
        }
    }

    public bool IsPressed(PlayerIndex xbox = PlayerIndex.One)
    {
        switch (Type)
        {
            case ControlType.PC:
                return Input.GetKeyDown(InputHelper.ReturnKeyCode(KeyValue));
            case ControlType.Xbox:
                return XboxControllerState.ButtonPressed(InputHelper.ReturnXboxButton(KeyValue), xbox);
            default:
                return false;
        }
    }

    public bool IsReleased(PlayerIndex xbox = PlayerIndex.One)
    {
        switch (Type)
        {
            case ControlType.PC:
                return Input.GetKeyUp(InputHelper.ReturnKeyCode(KeyValue));
            case ControlType.Xbox:
                return XboxControllerState.ButtonReleased(InputHelper.ReturnXboxButton(KeyValue), xbox);
            default:
                return false;
        }
    }

    #endregion

    #region Creates

    public static InputActionKey XboxButton(XboxButton btn)
    {
        return XboxButton(btn.ToString());
    }

    public static InputActionKey XboxButton(string btn)
    {
        return new InputActionKey(ControlType.Xbox, btn);
    }

    public static InputActionKey PCKey(KeyCode kc)
    {
        return PCKey(kc.ToString());
    }

    public static InputActionKey PCKey(string kc)
    {
        return new InputActionKey(ControlType.PC, kc);
    }

    #endregion
}
