using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

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
    public PlayerIndex Player;

    //public UpdateTypeE UpdateType = UpdateTypeE.FixedUpdate;

    public ControlType InputType = ControlType.PC;

    public Axis Horizontal;
    public Axis Vertical;

    [SerializeField]
    public List<Action> Actions = new List<Action>();

    public List<Axis> AnalogActions = new List<Axis>();

    public bool XboxSupport { get { return     Horizontal.AxisKeys.Any(k => ControlHelper.AxisKeyToControl(k.Type) == ControlType.Xbox) 
                                            || Vertical.AxisKeys.Any(k => ControlHelper.AxisKeyToControl(k.Type) == ControlType.Xbox) 
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
            controlScheme.Horizontal.AxisKeys.Add(AxisKey.XboxAxis(XboxAxis.LeftX.ToString()));
            controlScheme.Vertical.AxisKeys.Add(AxisKey.XboxAxis(XboxAxis.LeftY.ToString()));
        }
        if (xboxDPad)
        {
            controlScheme.Horizontal.AxisKeys.Add(AxisKey.XboxDpad(DirectionInput.Horizontal));
            controlScheme.Vertical.AxisKeys.Add(AxisKey.XboxDpad(DirectionInput.Vertical));
        }
        if (wasd)
        {
            controlScheme.Horizontal.AxisKeys.Add(AxisKey.PC(KeyCode.A, KeyCode.D));
            controlScheme.Vertical.AxisKeys.Add(AxisKey.PC(KeyCode.S, KeyCode.W));
        }
        if (arrows)
        {
            controlScheme.Horizontal.AxisKeys.Add(AxisKey.PC(KeyCode.LeftArrow, KeyCode.RightArrow));
            controlScheme.Vertical.AxisKeys.Add(AxisKey.PC(KeyCode.DownArrow, KeyCode.UpArrow));
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

        Actions = new List<Action>();

        string[] names = Enum.GetNames(type);

        for (int i = 0; i < names.Length; i++)
        {
            Actions.Add(new Action(Player, names[i]));
        }
    }    
}
