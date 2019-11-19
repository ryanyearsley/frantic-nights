using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum MenuState
{
    main,
    levelSelect
}


public class MenuManager : MonoBehaviour
{
    private MenuState currentMenuState;
    private GameObject canvas;
    private GameObject mainPanel;
    private GameObject levelSelectPanel;
    //private GameObject optionsPanel;
    //private GameObject thanksForPlayingPanel;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        mainPanel = canvas.transform.Find("mainPanel").gameObject;
        levelSelectPanel = canvas.transform.Find("levelSelectPanel").gameObject;

    }

    //Menu Panel Button Events
    public void quitGame()
    {
        Application.Quit();
    }
    public void setPanelMain()
    {
        setPanel(MenuState.main);
    }
    public void setPanelLevelSelect()
    {
        setPanel(MenuState.levelSelect);
    }

    public void loadLevelPointToPoint()
    {
        SceneManager.LoadScene("PointToPoint01");
    }

    public void loadLevelHotLap()
    {
        SceneManager.LoadScene("HotLap01");
    }



    public void setPanel(MenuState menuState)
    {
        currentMenuState = menuState;
        switch (menuState)
        {
            case MenuState.main:
                mainPanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                break;
            case MenuState.levelSelect:
                mainPanel.SetActive(false);
                levelSelectPanel.SetActive(true);
                break;
            default:
                mainPanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                break;
        } 
                
    }
}
