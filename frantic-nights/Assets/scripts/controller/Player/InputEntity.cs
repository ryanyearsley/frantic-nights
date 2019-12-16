using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputEntity
{
    public Player input;
    public int inputIndex;
    public bool isOccupied = false;
    public int playerNumber = 0;

    public InputEntity(int inputIndex)
    {
        input = ReInput.players.GetPlayer(inputIndex);
        this.inputIndex = inputIndex;
    }

    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
        inputIndex = joinPlayerInfo.inputIndex;
        Debug.Log("joining player input entity. index: " + inputIndex);
        playerNumber = joinPlayerInfo.playerNumber;
        isOccupied = true;
    }

    public void leavePlayer(int inputIndex)
    {
        Debug.Log("LeavePlayerInputEntity " + inputIndex);
        playerNumber = 0;
        isOccupied = false;
    }
    public void endScene(int inputIndex)
    {
        Debug.Log("input entity - end scene. index: " + inputIndex);
    }
    public void reportPlayerInputEntity()
    {
        
    }
}
