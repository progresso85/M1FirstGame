using UnityEngine;

[System.Serializable]
public class GameState
{
    public int loops;
    public int timer;
    public int startTimer;
    public string status;
    public int[][] map;
}

public class Error
{
    public string type;
    public string message;
}
