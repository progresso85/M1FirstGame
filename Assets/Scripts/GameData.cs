using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameState
{
    public int loops;
    public float timer;
    public int startTimer;
    public string status;
    public int[][] map;
}

public class Error
{
    public string type;
    public string message;
}