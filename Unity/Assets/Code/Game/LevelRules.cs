using UnityEngine;
using System.Collections;

public class LevelRules : ScriptableObject
{
    public int Lives = 3;
    public float RoundTime = 60;
    public bool LoseOnTimeUp = false;
    public bool SuddenDeathOnTimeUp = false;
    public bool RandomBlockStart = false;
    public bool LoseUpgradesOnNewRound = true;
    public int StartBombs = 1;
    public int StartBonusRange = 0;
    public int StartBonusDamage = 0;
}
