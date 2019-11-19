using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController[] playerControllers;

    public GameObject gameMessageWidget;

    private Text gameMessageText;

    private void Awake()
    {
        if (instance != null)
            GameObject.Destroy(instance);
        else
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        initializeGameManager();
    }

    protected virtual void initializeGameManager()
    {
        GameObject widget = Instantiate(gameMessageWidget, GameObject.FindObjectOfType<Canvas>().transform);
        gameMessageText = widget.transform.Find("GameMessage").gameObject.GetComponent<Text>();

        instance = this;
        playerControllers = FindObjectsOfType<PlayerController>();
    }

    public void transmitGameMessage(string message, int duration)
    {
        StartCoroutine(TransmitGameMessageRoutine(message, duration));
    }


    public IEnumerator TransmitGameMessageRoutine(string message, int messageDuration)
    {
        gameMessageText.text = message;

        yield return new WaitForSeconds(messageDuration);
        gameMessageText.text = "";
    }

    public virtual void resetObjective()
    {
        print("game manager reset");
    }
    public virtual void resetAllPlayers()
    {
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.resetPlayer();
        }
    }

    public void returnToMainMenu()
    {
        print("returning to main menu");
        SceneManager.LoadScene("mainmenu");
    }

}
