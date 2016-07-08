using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameMenu : Singleton<GameMenu>
{
    [System.Serializable]
    public class MenuHookups
    {
        public UnityEvent OnSplash;
        public UnityEvent OnStart;
        public UnityEvent OnLevelSelect;
        public UnityEvent OnStartGameMenu;
        public UnityEvent OnControls;
        public UnityEvent OnSettings;
        public UnityEvent OnGameWon;
        public UnityEvent OnGameDraw;
        public UnityEvent OnGameLost;
    }
    public MenuHookups EventHookups;
}
