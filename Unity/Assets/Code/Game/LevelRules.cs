using UnityEngine;
using System.Collections;

public class LevelRules : ScriptableObject
{
    public int Lives = 3;
    public float RoundTime = 60;
    public bool LoseOnTimeUp = false;
    public bool SuddenDeathOnTimeUp = false;
    public bool RandomBlockStart = false;
}
