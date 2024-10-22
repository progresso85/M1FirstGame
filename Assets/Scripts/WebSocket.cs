using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnityEditor.VersionControl;
using System;
using UnityEngine.SceneManagement;

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
        socket = new SocketIOUnity("https://project-maker-staging-c392e96b4ded.herokuapp.com/", options);

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
     
    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Map generated") 
        {
            Player player = mapLoader.player.gameObject.GetComponent<Player>();

            PlayerPayload payload = new PlayerPayload();
            payload.x = player.rb.position.x;
            payload.y = player.rb.position.y;
            String message = JsonUtility.ToJson(payload);
            Debug.Log(message);

            socket.Emit("unity-state", message);
        }

        socket.On("gamestate", data =>
        {
            var gamestate = JsonUtility.FromJson<GameState>(data.ToString());
            var map = JsonUtility.FromJson<Map>(gamestate.map.ToString());

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
                        mapLoader.LoadMap(map);
                    }
                    break;
                case "PLAYING":
                    timer.SetTimer(gamestate.timer);
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
