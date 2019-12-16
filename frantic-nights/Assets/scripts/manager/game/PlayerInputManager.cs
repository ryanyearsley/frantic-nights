using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //This class...
    //Monitors the inputs of all connected controllers, Keyboard, and mouse.
    //Administrates the joining/leaving of players.
    //
    public static PlayerInputManager instance;
    private int leaderInputIndex;
    public bool joinable;
    public int maxInputDeviceCount = 5;
    public int currentPlayerCount = 0;

    public List<InputEntity> inputEntities = new List<InputEntity>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        initialize();
    }
    public void initialize()
    {
        //5 = keyboard + mouse, and 5 controllers.
        for (int i = 0; i < maxInputDeviceCount; i++)
        {
            inputEntities.Add(new InputEntity(i));
        }
    }
    private void Update()
    {
    }
    private void pollForPlayerJoinInputs()
    {
        foreach (InputEntity inputEntity in inputEntities)
        {
            if (inputEntity.isOccupied == false && inputEntity.input.GetButton("Interact"))
            {
                createPlayer(inputEntity.inputIndex);
            }
        }
    }


    public void createPlayer(int inputIndex)
    {
        JoinPlayerInfo joinPlayerInfo = new JoinPlayerInfo();
        joinPlayerInfo.inputIndex = inputIndex;
        joinPlayerInfo.playerNumber = currentPlayerCount + 1;

        if (joinPlayerInfo.playerNumber == 1)
        {
            leaderInputIndex = inputIndex;
        }

    }

    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
        int inputIndex = joinPlayerInfo.inputIndex;
        currentPlayerCount += 1;
        inputEntities[joinPlayerInfo.inputIndex].joinPlayer(joinPlayerInfo);
        print("current player count: " + currentPlayerCount);
    }

    public void InputEntityReport()
    {
        print("InputEntityReport. current entities: " + inputEntities.Count + ", occupied: " + currentPlayerCount);
        foreach (InputEntity inputEntity in inputEntities)
        {
            inputEntity.reportPlayerInputEntity();
        }
    }
}
