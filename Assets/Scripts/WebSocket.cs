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
    private Scene scene;
    private Queue<Action> mainThreadActions = new Queue<Action>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();

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

     
    // Update is called once per frame
    void Update()
    {

        while (mainThreadActions.Count > 0)
        {
            Action action;
            lock (mainThreadActions)
            {
                action = mainThreadActions.Dequeue();
            }
            action.Invoke();
        }

        if (scene.name == "Map generated")
        {
            mapLoader = GameObject.Find("GenerateMap").GetComponent<MapLoader>();
            Player player = mapLoader.player.GetComponent<Player>();

            PlayerPayload payload = new PlayerPayload();
            payload.x = player.rb.position.x;
            payload.y = player.rb.position.y;
            Debug.Log(payload.x + ", " + payload.y);
            String message = JsonUtility.ToJson(payload);

            socket.Emit("player:unity", message);
        }

        socket.On("go", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            GameManager.Instance.mapToGenerate = JsonConvert.DeserializeObject<UnityMap>(jsonArray[0]);
        });

        socket.On("gamestate", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            GameState gamestate = JsonConvert.DeserializeObject<GameState>(jsonArray[0]);

            GamestateLoader(gamestate);
        });

        socket.On("error", error =>
        {
            var errordata = JsonUtility.FromJson<Error>(error.ToString());
            Debug.Log(errordata.type + " : " + errordata.message);
        });

        socket.On("map", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            UnityMap map = JsonConvert.DeserializeObject<UnityMap>(jsonArray[0]);

            NewMap(map);
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

    public void GamestateLoader(GameState gamestate)
    {
        EnqueueMainThreadAction(() =>
        {
            scene = SceneManager.GetActiveScene();
            switch (gamestate.status)
            {
                case "LOBBY":
                    if (scene.name != "Menu")
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    break;
                case "PLAYING":
                    if (scene.name != "Map generated")
                    {
                        SceneManager.LoadScene("Map generated");
                    }
                    break;
                case "FINISHED":
                    Debug.Log("Finished");
                    if (scene.name != "Menu")
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    // Add victory/lose screen
                    break;
            }
        });
    }

    public void NewMap(UnityMap map)
    {
        SceneManager.LoadScene("Map generated", LoadSceneMode.Single);
        GameManager.Instance.mapToGenerate = map;
    }

    private void EnqueueMainThreadAction(Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }

}
