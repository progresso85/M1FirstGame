using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
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
    public PlayerData playerData = null;

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
            payload.x = (player.rb.position.x - (float)0.25)*2;
            payload.y = (player.rb.position.y - (float)0.25)*2;
            if (playerData != null) {
                payload.id = playerData.id;
            }

            String message = JsonUtility.ToJson(payload);
            socket.Emit("player:position", message);
        }

        socket.On("signupsuccess", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            playerData = JsonConvert.DeserializeObject<PlayerData>(jsonArray[0]);
        });

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

        socket.On("unity:map", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            GameManager.Instance.mapToGenerate = JsonConvert.DeserializeObject<UnityMap>(jsonArray[0]);

            GameManager.Instance.hasRegeneratedMap = true;
        });

        socket.On("unity:position", data =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(data.ToString());
            GameManager.Instance.playerPosition = JsonConvert.DeserializeObject<Vector3>(jsonArray[0]);
            GameManager.Instance.isDead = true;
        });

        socket.On("cast:spell", spell =>
        {
            string[] jsonArray = JsonConvert.DeserializeObject<string[]>(spell.ToString());
            GameManager.Instance.spell = JsonConvert.DeserializeObject<Spell>(jsonArray[0]);
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
                    GameManager.Instance.items = gamestate.items;
                    GameManager.Instance.timer = gamestate.timer;
                    GameManager.Instance.loops = gamestate.loops;
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

    private void EnqueueMainThreadAction(Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }

}
