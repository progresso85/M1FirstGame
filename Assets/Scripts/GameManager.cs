using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public UnityMap mapToGenerate = null;
    public float timer = 0;
    public int loops = 0;
    public bool hasRegeneratedMap = false;
    public Item[] items;
    public Spell spell;
    public Vector3 playerPosition;
    public bool isDead = false;

    private static GameManager instance = null;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}