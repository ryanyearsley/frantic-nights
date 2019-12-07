using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public enum MenuState
    {
        main,
        levelSelect
    }

    private MenuState currentMenuState;
    private GameObject canvas;
    //index 0
    private GameObject mainPanel;
    //index 1
    private GameObject levelSelectPanel;

    // Start is called before the first frame update

   public void initialize()
    {
        canvas = GameObject.Find("Canvas");
        mainPanel = canvas.transform.Find("mainPanel").gameObject;
        levelSelectPanel = canvas.transform.Find("levelSelectPanel").gameObject;
    }
    public void setPanel(int panelIndex)
    {
        switch (panelIndex)
        {
            case 0:
                currentMenuState = MenuState.main;
                mainPanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                break;
            case 1:
                currentMenuState = MenuState.levelSelect;
                mainPanel.SetActive(false);
                levelSelectPanel.SetActive(true);
                break;
            default:
                currentMenuState = MenuState.main;
                mainPanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                break;
        } 
                
    }

}
