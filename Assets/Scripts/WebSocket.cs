using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnityEditor.VersionControl;
using System;

public class WebSocket : MonoBehaviour
{

    private SocketIOUnity socket;
    [SerializeField] private Player player; 

    // Start is called before the first frame update
    void Start()
    {
        socket = new SocketIOUnity("http://localhost:3000/");

        socket.OnConnected += (sender, e) => {
            Debug.Log("Connected!");
            Debug.Log("Sending Hello");

            SignupPayload payload = new SignupPayload();
            payload.name = "Un client";
            payload.type = "UNITY";

            String message = JsonUtility.ToJson(payload);
            socket.Emit("signup", message);
            socket.On("message", data => {
                Debug.Log(data);
            });
        };

        socket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.rb.position.x;
        float y = player.rb.position.y;

        PlayerPayload payload = new PlayerPayload();
        payload.x = x;
        payload.y = y;

        socket.Emit("message", payload);
    }

}
