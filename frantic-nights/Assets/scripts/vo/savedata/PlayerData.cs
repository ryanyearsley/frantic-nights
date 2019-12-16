[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerNumber;
    public int lapsCompleted;

    public PlayerData (string name, int number, int laps)
    {
        playerName = name;
        playerNumber = number;
        lapsCompleted = laps;
    }
}
