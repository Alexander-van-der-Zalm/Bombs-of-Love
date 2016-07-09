using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameMenu : Singleton<GameMenu>
{
    #region Classes

    [System.Serializable]
    public class MenuHookups
    {
        public UnityEvent OnSplash;
        public UnityEvent OnStartMenu;
        public UnityEvent OnLevelSelect;
        public UnityEvent OnStartGameMenu;
        public UnityEvent OnControls;
        public UnityEvent OnSettings;
        public UnityEvent OnGameWon;
        public UnityEvent OnGameDraw;
        public UnityEvent OnGameLost;
    }
    [System.Serializable]
    public class MenuGameObjects
    {
        public GameObject Splash;
        public GameObject StartMenu;
        public GameObject LevelSelect;
        public GameObject StartGameMenu;
        public GameObject Controls;
        public GameObject Settings;
        public GameObject GameWon;
        public GameObject GameDraw;
        public GameObject GameLost;
    }

    #endregion

    #region Fields

    public MenuGameObjects MenuObjects;
    public MenuHookups EventHookups;

    private GameObject CurrentMenu;

    #endregion

    #region Awake

    public void Awake()
    {
        Splash();
    }

    #endregion

    private void SetActive(GameObject newMenu)
    {
        if(CurrentMenu!=null)
            CurrentMenu.SetActive(false);

        CurrentMenu = newMenu;

        if (CurrentMenu != null)
            CurrentMenu.SetActive(true);
    }

    public void HideMenu()
    {
        CurrentMenu.SetActive(false);
    }

    public void Splash()
    {
        Debug.Log("Splash");
        SetActive(MenuObjects.Splash);
        EventHookups.OnSplash.Invoke();
    }
    public void StartMenu()
    {
        Debug.Log("StartMenu");
        SetActive(MenuObjects.StartMenu);
        EventHookups.OnStartMenu.Invoke();
    }
    public void LevelSelect()
    {
        Debug.Log("LevelSelect");
        SetActive(MenuObjects.LevelSelect);
        EventHookups.OnLevelSelect.Invoke();
    }
    public void StartGameMenu()
    {
        Debug.Log("StartGameMenu");
        SetActive(MenuObjects.StartGameMenu);
        EventHookups.OnStartGameMenu.Invoke();
    }
    public void Controls()
    {
        Debug.Log("Controls");
        SetActive(MenuObjects.Controls);
        EventHookups.OnControls.Invoke();
    }
    public void Settings()
    {
        Debug.Log("Settings");
        SetActive(MenuObjects.Settings);
        EventHookups.OnSettings.Invoke();
    }
    public void GameWon()
    {
        Debug.Log("GameWon");
        SetActive(MenuObjects.GameWon);
        EventHookups.OnGameWon.Invoke();
    }
    public void GameDraw()
    {
        Debug.Log("GameDraw");
        SetActive(MenuObjects.GameDraw);
        EventHookups.OnGameDraw.Invoke();
    }
    public void GameLost()
    {
        Debug.Log("GameLost");
        SetActive(MenuObjects.GameLost);
        EventHookups.OnGameLost.Invoke();
    }
}
