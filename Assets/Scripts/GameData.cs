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
    public Item[] items;
}

public class Error
{
    public string type;
    public string message;
}

public class Item
{
    public string id;
    public string name;
    public string type;
    public float duration;
    public string description;
    public Vector3 coords;
};