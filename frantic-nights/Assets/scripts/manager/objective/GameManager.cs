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
    private Text gameMessageSecondaryText;

    private bool controlsCollapsed = false;
    private Text controlsText;
    private string controlsString;
    private string controlsCollapsedString = "Press DPAD UP or C to view Controls.";

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

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            toggleControlsDisplay();
        }

    }

    protected virtual void initializeGameManager()
    {
        GameObject widget = Instantiate(gameMessageWidget, GameObject.FindObjectOfType<Canvas>().transform);
        gameMessageText = widget.transform.Find("GameMessage").gameObject.GetComponent<Text>();
        gameMessageSecondaryText = widget.transform.Find("GameMessageSecondary").gameObject.GetComponent<Text>();
        controlsText = widget.transform.Find("controlsText").gameObject.GetComponent<Text>();
        controlsString = controlsText.text;
        toggleControlsDisplay();

        instance = this;
        playerControllers = FindObjectsOfType<PlayerController>();
    }

    public void transmitGameMessage(string message, int duration)
    {
        StartCoroutine(TransmitGameMessageRoutine(message, duration));
    }
    public void transmitGameMessage(string messagePrimary, string messageSecondary, int duration)
    {
        StartCoroutine(TransmitGameMessageWithSecondaryRoutine(messagePrimary, messageSecondary, duration));
    }

    public void toggleControlsDisplay()
    {

        if (controlsCollapsed)
        {
            controlsText.text = controlsString;
            controlsCollapsed = false;
        }

        else
        {
            controlsText.text = controlsCollapsedString;
            controlsCollapsed = true;
        }
    }

    public IEnumerator TransmitGameMessageRoutine(string message, int messageDuration)
    {
        gameMessageText.text = message;

        yield return new WaitForSeconds(messageDuration);
        gameMessageText.text = "";
    }

    public IEnumerator TransmitGameMessageWithSecondaryRoutine(string messagePrimary, string messageSecondary, int messageDuration)
    {
        gameMessageText.text = messagePrimary;
        gameMessageSecondaryText.text = messageSecondary;
        yield return new WaitForSeconds(messageDuration);
        gameMessageText.text = "";
        gameMessageSecondaryText.text = "";
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


}
