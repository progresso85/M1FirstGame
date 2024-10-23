using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnityEditor.VersionControl;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class WebSocket : MonoBehaviour
{

    private SocketIOUnity socket;
    public MapLoader mapLoader;
    public Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        var options = new SocketIOOptions
        {
            Reconnection = false,
            ReconnectionAttempts = 5,
            ReconnectionDelay = 5000,
        };
        socket = new SocketIOUnity("http://localhost:3001/", options);

        socket.OnConnected += (sender, e) => 
        {
            Debug.Log("Connected!");
            Debug.Log("Sending Hello");

            SignupPayload payload = new SignupPayload();
            payload.name = "Un client";
            payload.type = "UNITY";

            String message = JsonUtility.ToJson(payload);
            socket.Emit("signup", message);
        };
        socket.Connect();
    }

    void Awake()
    {
        mapLoader = FindObjectOfType<MapLoader>();
    }
     
    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        Debug.Log(scene.name);
        if (scene.name == "Map generated") 
        {
            Debug.Log("ok");
            Player player = mapLoader.player.gameObject.GetComponent<Player>();

            PlayerPayload payload = new PlayerPayload();
            payload.x = player.rb.position.x;
            payload.y = player.rb.position.y;
            String message = JsonUtility.ToJson(payload);
            Debug.Log(message);
            socket.Emit("unity-state", message);
        }

        socket.On("go", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            UnityMap mapArray = JsonConvert.DeserializeObject<UnityMap>(jsonArray[0]);
            mapLoader.LoadMap(mapArray);
        });

        socket.On("gamestate", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            GameState gamestate = JsonConvert.DeserializeObject<GameState>(jsonArray[0]);

            switch (gamestate.status)
            {
                case "WAITING":
                    if (scene.name != "Menu")
                    {
                        SceneManager.LoadScene(0, LoadSceneMode.Single);
                    }
                    break;
                case "STARTING":
                    if (scene.name != "Map generated")
                    {
                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                    break;
                case "PLAYING":
                    //int timePlayed = Convert.ToInt32(Math.Round(gamestate.timer, 0));
                    //timer.SetTimer(timePlayed);
                    //mapLoader.UpdateMap(map);
                    break;
                case "FINISHED":
                    // Add victory/lose screen
                    break;
            }
        });

        socket.On("error", error =>
        {
            var errordata = JsonUtility.FromJson<Error>(error.ToString());
            Debug.Log(errordata.type + " : " + errordata.message);
        });
    }

    void OnDestroy()
    {
        if (socket != null)
        {
            socket.Disconnect();
            Debug.Log("Connexion Socket fermée (OnDestroy).");
        }
    }

    void OnApplicationQuit()
    {
        if (socket != null)
        {
            socket.Disconnect();
            Debug.Log("Connexion Socket fermée (OnApplicationQuit).");
        }
    }

}
