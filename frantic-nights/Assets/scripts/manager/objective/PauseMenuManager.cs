using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    public void returnToMainMenu()
    {
        print("returning to main menu");
        GameManager.instance.savePlayerProgress();
        SceneManager.LoadScene("mainmenu");
    }
}
