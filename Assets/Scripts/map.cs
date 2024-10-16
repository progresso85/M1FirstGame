using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWebService;

public class map : MonoBehaviour
{

    // public String url = "" // url du serveur
    public JSON mapJson = 
    {
        "name": "route",
        "type": "route",
        "properties": {
            "position": {
                "x": 0,
                "y": 0,
                "z": 0
            },
            "angle": 0
        },
        "name": "route",
        "type": "route",
        "properties": {
            "position": {
                "x": 0.5,
                "y": 0,
                "z": 0
            },
            "angle": 0
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var tiles = GameObject.FindGameObjectsWithTag("Tile");
        var tileCount = tiles.Length;
        var tileName;
        var tilePosition;
        foreach (var tile in tiles) 
        {
            tileName = tile.name;
            tilePosition = tile.transform.position;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
