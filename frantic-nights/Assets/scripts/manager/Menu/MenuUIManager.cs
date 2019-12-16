using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public enum MenuState
    {
        main,
        profileSelect,
        profileCreate,
        levelSelect
    }

    private MenuState currentMenuState;
    private GameObject canvas;

    public List<GameObject> allPanels = new List<GameObject>();
    //index 0
    private GameObject mainPanel;
    //index 1
    private GameObject profileSelectPanel;
    //index 2
    private GameObject profileCreatePanel;
    //index 3
    private GameObject levelSelectPanel;
    //index 4
    private GameObject optionsPanel;

    private Text playerOneName;
    private Text playerOneNumber;
    private Text playerOneLaps;

    private InputField createPlayerNameInputField;
    private InputField createPlayerNumberInputField;

    private Slider masterVolSlider;
    private Slider sfxVolSlider;
    private Slider musicVolSlider;

    // Start is called before the first frame update

    public void initialize()
    {
        canvas = GameObject.Find("Canvas");
        mainPanel = canvas.transform.Find("mainPanel").gameObject;
        allPanels.Add(mainPanel);
        profileSelectPanel = canvas.transform.Find("profileSelectPanel").gameObject;
        allPanels.Add(profileSelectPanel);
        profileCreatePanel = canvas.transform.Find("profileCreatePanel").gameObject;
        allPanels.Add(profileCreatePanel);
        levelSelectPanel = canvas.transform.Find("levelSelectPanel").gameObject;
        allPanels.Add(levelSelectPanel);
        optionsPanel = canvas.transform.Find("optionsPanel").gameObject;
        allPanels.Add(optionsPanel);

        //player select UI Elements
        playerOneName = profileSelectPanel.transform.Find("ProfileLicense1").
            transform.Find("PlayerName").
            GetComponent<Text>();
        playerOneNumber = profileSelectPanel.transform.Find("ProfileLicense1").
            transform.Find("PlayerRaceNumber").
            GetComponent<Text>();
        playerOneLaps = profileSelectPanel.transform.Find("ProfileLicense1").
            transform.Find("LapsCompleted").
            GetComponent<Text>();

        //Player Creation UI Elements
        createPlayerNameInputField = profileCreatePanel.transform.Find("ProfileCreationLicense1").
            transform.Find("InputFieldName").
            GetComponent<InputField>();
        createPlayerNumberInputField = profileCreatePanel.transform.Find("ProfileCreationLicense1").
            transform.Find("InputFieldNumber").
            GetComponent<InputField>();


        //Options UI Elements
        masterVolSlider = optionsPanel.transform.Find("optionsVolumePanel")
            .transform.Find("masterVolSlider").
            GetComponent<Slider>();
        sfxVolSlider = optionsPanel.transform.Find("optionsVolumePanel")
            .transform.Find("sfxVolSlider").
            GetComponent<Slider>();
        musicVolSlider = optionsPanel.transform.Find("optionsVolumePanel")
            .transform.Find("musicVolSlider").
            GetComponent<Slider>();

        setPanel(0);
        setProfileOne(MenuManager.instance.retrievePlayerData(0));
        loadOptionsData(SaveGameManager.loadOptions());

    }
    public void setPanel(int panelIndex)
    {
        foreach (GameObject go in allPanels)
        {
            go.SetActive(false);
        } 

        allPanels[panelIndex].SetActive(true);
        /*
        switch (panelIndex)
        {
            case 0:
                currentMenuState = MenuState.main;
                mainPanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                profileSelectPanel.SetActive(false);
                profileCreatePanel.SetActive(false);
                break;
            case 1:
                currentMenuState = MenuState.profileSelect;
                setProfileOne(MenuManager.instance.retrievePlayerData(0));
                mainPanel.SetActive(false);
                profileSelectPanel.SetActive(true);
                profileCreatePanel.SetActive(false);
                levelSelectPanel.SetActive(false);
                break;
            case 2:
                currentMenuState = MenuState.profileCreate;
                mainPanel.SetActive(false);
                profileSelectPanel.SetActive(false);
                profileCreatePanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                break;
            case 3:
                currentMenuState = MenuState.levelSelect;
                mainPanel.SetActive(false);
                profileSelectPanel.SetActive(false);
                profileCreatePanel.SetActive(false);
                levelSelectPanel.SetActive(true);
                break;
            default:
                currentMenuState = MenuState.main;
                mainPanel.SetActive(true);
                levelSelectPanel.SetActive(false);
                profileSelectPanel.SetActive(false);
                profileCreatePanel.SetActive(false);
                break;
        } 
                */
    }

    

    public void setProfileOne(PlayerData playerData)
    {
        if (playerData != null)
        {
            playerOneName.text = playerData.playerName;
            playerOneNumber.text = playerData.playerNumber.ToString();
            playerOneLaps.text = "Laps Completed: " + playerData.lapsCompleted.ToString();
        }
    }

    public void validateAndCreateNewProfile()
    {
        //parse from fields
        string name = createPlayerNameInputField.text;
        //validate int
        int playerNumber = int.Parse(createPlayerNumberInputField.text);
        PlayerData playerData = new PlayerData(name, playerNumber, 0);
        SaveGameManager.savePlayer(playerData, 0);
        setProfileOne(playerData);
    }

    public void saveOptionsData()
    {
        OptionsData optionsData = new OptionsData();
        optionsData.masterVolume = masterVolSlider.value;
        optionsData.sfxVolume = sfxVolSlider.value;
        optionsData.musicVolume = musicVolSlider.value;
        SaveGameManager.saveOptions(optionsData);
    }

    public void loadOptionsData(OptionsData optionsData)
    {
       masterVolSlider.value = optionsData.masterVolume;
        sfxVolSlider.value = optionsData.sfxVolume;
        musicVolSlider.value = optionsData.musicVolume;
    }
}
