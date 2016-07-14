using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

[System.Serializable]
public class Action
{
    #region Fields

    public string Name;
    public List<ActionKey> Keys;

    [SerializeField, ReadOnly]
    private ControlType m_LastInputType = ControlType.PC;
    [SerializeField]
    private PlayerIndex m_XboxPlayer;

    //[SerializeField,HideInInspector]
    //protected ControlScheme scheme;

    public ControlType LastInputType { get { return m_LastInputType; } }

    #endregion

    #region CTor

    public Action(PlayerIndex plyr = PlayerIndex.One, string name = "defaultAction")
    {
        Keys = new List<ActionKey>();

        //this.scheme = scheme;
        this.Name = name;
        this.m_XboxPlayer = plyr;
    }

    #endregion

    #region Down, Pressed, Released

    public bool IsDown()
    {
        foreach (ActionKey key in Keys)
        {
            if (key.IsDown(m_XboxPlayer))
                return TrueAndSetInputType(key);
        }
        return false;
    }
    public bool IsPressed()
    {
        foreach (ActionKey key in Keys)
        {
            if (key.IsPressed(m_XboxPlayer))
                return TrueAndSetInputType(key);
        }
        return false;
    }

    public bool IsReleased()
    {
        foreach (ActionKey key in Keys)
        {
            if (key.IsReleased(m_XboxPlayer))
                return TrueAndSetInputType(key);
        }
        return false;
    }

    private bool TrueAndSetInputType(ActionKey key)
    {
        m_LastInputType = key.Type;
        return true;
    }

    #endregion

    #region Creates

    public void AddXboxButton(XboxButton btn)
    {
        Keys.Add(ActionKey.XboxButton(btn));
    }

    public void AddXboxButton(string btn)
    {
        Keys.Add(ActionKey.XboxButton(btn));
    }

    public void AddPCKey(KeyCode kc)
    {
        Keys.Add(ActionKey.PCKey(kc));
    }

    public void AddPCKey(string kc)
    {
        Keys.Add(ActionKey.PCKey(kc));
    }

    public static Action Create(string pc, XboxButton xb = XboxButton.None, PlayerIndex index = PlayerIndex.One, string name = "")
    {
        Action ac = new Action(index, name);
        ac.AddPCKey(pc);
        if (xb != XboxButton.None)
            ac.AddXboxButton(xb);
        return ac;
    }

    public static Action Create(KeyCode pc, XboxButton xb = XboxButton.None, PlayerIndex index = PlayerIndex.One, string name = "")
    {
        Action ac = new Action(index, name);
        ac.AddPCKey(pc);
        if(xb != XboxButton.None)
            ac.AddXboxButton(xb);
        return ac;
    }

    #endregion
}
