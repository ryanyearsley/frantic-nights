using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    MenuUIManager menuUIManager;

    private List<PlayerData> profiles;

    public PlayerData playerOne;
    public PlayerData playerTwo;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        initialize();
    }
    private void initialize()
    {
        playerOne = retrievePlayerData(0);
        menuUIManager = GameObject.Find("MenuUIManager").GetComponent<MenuUIManager>();
        menuUIManager.initialize();
        //profiles = loadAllProfiles();
    }

    public List<PlayerData> loadAllProfiles()
    {
        List<PlayerData> profileList = new List<PlayerData>();
        for (int i = 0; i < 10; i++)
        {
            try
            {
                profileList.Add(SaveGameManager.loadPlayer(i));
            }
            catch (FileNotFoundException fnfe)
            {
                Debug.Log("File not found when attempting to retrieve profile from save slot " + i);
            }
        }
        return profileList;
    }

    public PlayerData retrievePlayerData(int saveSlot)
    {
        try
        {
            Debug.Log("Player found in slot " + saveSlot);
            return SaveGameManager.loadPlayer(saveSlot);
        }catch (FileNotFoundException fnfe){ 
            Debug.Log("No available profile in that save slot: " + saveSlot);
            return null;
        }
    }
    //Menu Panel Button Events
    public void quitGame()
    {
        Application.Quit();
    }

    public void loadLevelIsland02()
    {
        SceneManager.LoadScene("Island02");
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
