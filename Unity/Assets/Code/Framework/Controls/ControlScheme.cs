using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

// Make generic
[Serializable][CreateAssetMenu]
public class ControlScheme:ScriptableObject
{
    public enum UpdateTypeE
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

#region Fields

    public string Name;
    //public int controllerID = 1;
    //public int playerID = 1;
    public int Player;

    //public UpdateTypeE UpdateType = UpdateTypeE.FixedUpdate;

    public ControlType InputType = ControlType.PC;

    public InputAxis Horizontal;
    public InputAxis Vertical;

    [SerializeField]
    public List<InputAction> Actions = new List<InputAction>();

    public List<InputAxis> AnalogActions = new List<InputAxis>();

    public bool XboxSupport { get { return     Horizontal.AxisKeys.Any(k => InputHelper.AxisKeyToControl(k.Type) == ControlType.Xbox) 
                                            || Vertical.AxisKeys.Any(k => InputHelper.AxisKeyToControl(k.Type) == ControlType.Xbox) 
                                            || Actions.Any(a => a.Keys.Any(k => k.Type == ControlType.Xbox)); } }

#endregion

    public static ControlScheme CreateDefaultScheme<T>(ControlScheme controlScheme, bool xboxLeftStick = true, bool xboxDPad = true, bool arrows = true, bool wasd = true) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        controlScheme.Name = typeof(T).ToString();
        //controlScheme.UpdateType = updateType;
        controlScheme.SetActionsFromEnum<T>();

        //controlScheme.Horizontal = new Axis(controlScheme, "Horizontal");
        //controlScheme.Vertical = new Axis(controlScheme, "Vertical");

        if (xboxLeftStick)
        {
            controlScheme.Horizontal.AxisKeys.Add(InputAxisKey.XboxAxis(XboxAxis.LeftX));
            controlScheme.Vertical.AxisKeys.Add(InputAxisKey.XboxAxis(XboxAxis.LeftY));
        }
        if (xboxDPad)
        {
            controlScheme.Horizontal.AxisKeys.Add(InputAxisKey.XboxDpad(DirectionInput.Horizontal));
            controlScheme.Vertical.AxisKeys.Add(InputAxisKey.XboxDpad(DirectionInput.Vertical));
        }
        if (wasd)
        {
            controlScheme.Horizontal.AxisKeys.Add(InputAxisKey.PC(KeyCode.A, KeyCode.D));
            controlScheme.Vertical.AxisKeys.Add(InputAxisKey.PC(KeyCode.S, KeyCode.W));
        }
        if (arrows)
        {
            controlScheme.Horizontal.AxisKeys.Add(InputAxisKey.PC(KeyCode.LeftArrow, KeyCode.RightArrow));
            controlScheme.Vertical.AxisKeys.Add(InputAxisKey.PC(KeyCode.DownArrow, KeyCode.UpArrow));
        }

        return controlScheme;
    }

    public void SetActionsFromEnum<T>() where T : struct, IConvertible
    {
        SetActionsFromEnum(typeof(T));
    }

    public void SetActionsFromEnum(Type type)
    {
        if (!type.IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        Actions = new List<InputAction>();

        string[] names = Enum.GetNames(type);

        for (int i = 0; i < names.Length; i++)
        {
            Actions.Add(new InputAction(Player, names[i]));
        }
    }    
}
