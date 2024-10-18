using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnityEditor.VersionControl;
using System;

public class WebSocket : MonoBehaviour
{

    private SocketIOUnity socket;
    [SerializeField] MapLoader mapLoader;

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

        socket.OnConnected += (sender, e) => {
            Debug.Log("Connected!");
            Debug.Log("Sending Hello");

            SignupPayload payload = new SignupPayload();
            payload.name = "Un client";
            payload.type = "UNITY";

            String message = JsonUtility.ToJson(payload);
            socket.Emit("signup", message);

            socket.On("go", data => {
                Debug.Log(data);
            });
        };
        socket.Connect();
    }

    // Update is called once per frame
    void Update()
    {   
        Player player = mapLoader.player.gameObject.GetComponent<Player>();

        PlayerPayload payload = new PlayerPayload();
        payload.x = player.rb.position.x;
        payload.y = player.rb.position.y;
        String message = JsonUtility.ToJson(payload);

        socket.Emit("message", message);
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
