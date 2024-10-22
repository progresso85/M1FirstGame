using UnityEngine;

[System.Serializable]
public class GameState
{
    public string status;
    public int timer;
    public int startTimer;
    public int loops;
    public Map map;
}

public class Error
{
    public string type;
    public string message;
}
