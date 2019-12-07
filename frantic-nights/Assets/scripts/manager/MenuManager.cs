using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    MenuUIManager menuUIManager;

    private void Start()
    {
        menuUIManager = GameObject.Find("MenuUIManager").GetComponent<MenuUIManager>();
        menuUIManager.initialize();
    }

    //Menu Panel Button Events
    public void quitGame()
    {
        Application.Quit();
    }
   
    public void loadLevelPointToPoint()
    {
        SceneManager.LoadScene("PointToPoint01");
    }

    public void loadLevelHotLap()
    {
        SceneManager.LoadScene("HotLap01");
    }

}
