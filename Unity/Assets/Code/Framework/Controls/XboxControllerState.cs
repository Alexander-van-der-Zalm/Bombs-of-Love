using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

#region Enums

public enum XboxButton
{
    A, B, X, Y, Up,Down,Left,Right, RightShoulder, LeftShoulder, LeftStick, RightStick, Back, Start, Guide
}

public enum XboxAxis
{
    LeftX,LeftY,RightX,RightY,LeftTrigger,RightTrigger
}

#endregion

public class XboxControllerState : Singleton<XboxControllerState>
{
    public GamePadState[] CurrentState;
    public GamePadState[] LastState;

    public ControlScheme.UpdateTypeE UpdateType;

    public void Awake()
    { 
        Debug.Log("Awake");
        CurrentState = new GamePadState[4];
        LastState = new GamePadState[4];
    }

    #region Updates

    public void LateUpdate()
    {
        if (UpdateType != ControlScheme.UpdateTypeE.LateUpdate)
            return;

        UpdateAllStates();
    }

    public void FixedUpdate()
    {
        if (UpdateType != ControlScheme.UpdateTypeE.FixedUpdate)
            return;

        UpdateAllStates();
    }

    public void Update()
    {
        if (UpdateType != ControlScheme.UpdateTypeE.Update)
            return;

        UpdateAllStates();
    }

    private void UpdateAllStates()
    {
        if (LastState == null || CurrentState == null)
            Awake();

        for (int i = 0; i < 4; i++)
        {
            LastState[i] = CurrentState[i];
            CurrentState[i] = GamePad.GetState((PlayerIndex)i);
        }
    }

    #endregion

    #region Axis

    public static float Axis(XboxAxis axis, PlayerIndex index)
    {
        switch (axis)
        {
            default:
            case XboxAxis.LeftX:
                return Instance.CurrentState[(int)index].ThumbSticks.Left.X;
            case XboxAxis.LeftY:
                return Instance.CurrentState[(int)index].ThumbSticks.Left.Y;
            case XboxAxis.RightX:
                return Instance.CurrentState[(int)index].ThumbSticks.Right.X;
            case XboxAxis.RightY:
                return Instance.CurrentState[(int)index].ThumbSticks.Right.Y;

            case XboxAxis.LeftTrigger:
                return Instance.CurrentState[(int)index].Triggers.Left;
            case XboxAxis.RightTrigger:
                return Instance.CurrentState[(int)index].Triggers.Right;

        }
    }

    #endregion

    #region Button

    public static bool ButtonPressed(XboxButton button, PlayerIndex index)
    {
        return !ButtonPressed(button, Instance.LastState[(int)index]) && ButtonPressed(button, Instance.CurrentState[(int)index]);
    }

    public static bool ButtonReleased(XboxButton button, PlayerIndex index)
    {
        return ButtonPressed(button, Instance.LastState[(int)index]) && !ButtonPressed(button, Instance.CurrentState[(int)index]);
    }

    public static bool ButtonDown(XboxButton button, PlayerIndex index)
    {
        return ButtonPressed(button, Instance.CurrentState[(int)index]);
    }

    public static bool ButtonPressed(XboxButton button, GamePadState state)
    {
        switch (button)
        {
            default:
            case XboxButton.A:
                return state.Buttons.A == ButtonState.Pressed;
            case XboxButton.B:
                return state.Buttons.B == ButtonState.Pressed;
            case XboxButton.X:
                return state.Buttons.X == ButtonState.Pressed;
            case XboxButton.Y:
                return state.Buttons.Y == ButtonState.Pressed;
            case XboxButton.RightShoulder:
                return state.Buttons.RightShoulder == ButtonState.Pressed;
            case XboxButton.LeftShoulder:
                return state.Buttons.LeftShoulder == ButtonState.Pressed;
            case XboxButton.LeftStick:
                return state.Buttons.LeftStick == ButtonState.Pressed;
            case XboxButton.RightStick:
                return state.Buttons.RightStick == ButtonState.Pressed;
            case XboxButton.Back:
                return state.Buttons.Back == ButtonState.Pressed;
            case XboxButton.Start:
                return state.Buttons.Start == ButtonState.Pressed;
            case XboxButton.Guide:
                return state.Buttons.Guide == ButtonState.Pressed;
            case XboxButton.Up:
                return state.DPad.Up == ButtonState.Pressed;
            case XboxButton.Down:
                return state.DPad.Down == ButtonState.Pressed;
            case XboxButton.Left:
                return state.DPad.Left == ButtonState.Pressed;
            case XboxButton.Right:
                return state.DPad.Right == ButtonState.Pressed;
        }
    }

    #endregion

    public static PlayerIndex GetFirstConnected()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Instance.CurrentState[i].IsConnected)
                return (PlayerIndex)i;
        }
        return PlayerIndex.One;
    }
}
