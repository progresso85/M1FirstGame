using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MapLoader : MonoBehaviour
{
    [SerializeField] GameObject road;
    [SerializeField] GameObject sidewalk;
    [SerializeField] GameObject sidewalk_left;
    [SerializeField] GameObject sidewalk_right;
    [SerializeField] GameObject sidewalk_top;
    [SerializeField] GameObject sidewalk_bottom;
    [SerializeField] GameObject sidewalk_top_left;
    [SerializeField] GameObject sidewalk_top_right;
    [SerializeField] GameObject sidewalk_bottom_left;
    [SerializeField] GameObject sidewalk_bottom_right;
    [SerializeField] GameObject crosswalk;
    [SerializeField] GameObject crosswalk_vertical;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject[] tree_array;
    [SerializeField] GameObject lake;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject character;
    [SerializeField] GameObject[] house_array;
    [SerializeField] GameObject Bombe;
    [SerializeField] GameObject Coin;
    [SerializeField] GameObject Wall;
    [SerializeField] GameObject AudioManager;

    public GameObject player;

    HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
   

    private bool hasGeneratedMap = false;

    private void Update()
    {
        if (!hasGeneratedMap)
        {
            LoadMap(GameManager.Instance.mapToGenerate);
            GameManager.Instance.mapToGenerate = null;
            hasGeneratedMap = true;
        } 
        if (GameManager.Instance.hasRegeneratedMap)
        {
            NewMap(GameManager.Instance.mapToGenerate);
            GameManager.Instance.hasRegeneratedMap = false;
        }
        ItemMap(GameManager.Instance.items);
        if (GameManager.Instance.isDead)
        {
            PlayerToSpawn(GameManager.Instance.playerPosition);
            GameManager.Instance.isDead = false;
        }
    }

    public void LoadMap(UnityMap map)
    {
        GameManager.Instance.mapToGenerate = null;

        Debug.Log("Chargemnt de la map...");
        foreach (TileData tileData in map.unityMap.properties.tiles)
        {
            // Choose prefab with its type
            Vector3 position = tileData.properties.position;
            position = new Vector3((position.x / 2) + (float)0.25, (position.y / 2) + (float)0.25, position.z);
            if (tileData.type == "Road")
            {
                Instantiate(road, position, Quaternion.Euler(0, 0, 0));
                occupiedPositions.Add(position);
            }
            else if (tileData.type == "Sidewalk")
            {
                switch (tileData.properties.angle % 360)
                {
                    case 0:
                        Instantiate(sidewalk_top, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 45:
                        Instantiate(sidewalk_top_right, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 90:
                        Instantiate(sidewalk_right, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 135:
                        Instantiate(sidewalk_bottom_right, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 180:
                        Instantiate(sidewalk_bottom, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 225:
                        Instantiate(sidewalk_bottom_left, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 270:
                        Instantiate(sidewalk_left, position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 315:
                        Instantiate(sidewalk_top_left, position, Quaternion.Euler(0, 0, 0));
                        break;
                    default:
                        Instantiate(sidewalk, position, Quaternion.Euler(0, 0, 0));
                        break;
                }
                occupiedPositions.Add(position);

            }
            else if (tileData.type == "Crosswalk")
            {
                switch (tileData.properties.angle % 360)
                {
                    case 0:
                        Instantiate(crosswalk, position, Quaternion.Euler(0, 0, 90));
                        break;
                    case 90:
                        Instantiate(crosswalk, position, Quaternion.Euler(0, 0, 0));
                        break;
                }
                occupiedPositions.Add(position);
            }
            else if (tileData.type == "House")
            {

                System.Random random = new System.Random();
                int randomIndex = random.Next(5);
                Instantiate(house_array[randomIndex], position, Quaternion.Euler(0, 0, 0));
                occupiedPositions.Add(position);


            }
            else if (tileData.type == "Tree")
            {

                System.Random random = new System.Random();
                int randomIndex = random.Next(2);
                Instantiate(tree_array[randomIndex], position, Quaternion.Euler(0, 0, 0));
                occupiedPositions.Add(position);

            }
            else if (tileData.type == "Lake")
            {
                Instantiate(lake, position, Quaternion.Euler(0, 0, 0));
                occupiedPositions.Add(position);
            }
            if (tileData.type == "Start")
            {
                Instantiate(spawnPoint, position, Quaternion.Euler(0, 0, 0));
                player = Instantiate(character, position, Quaternion.Euler(0, 0, 0));
                DontDestroyOnLoad(player);
                DontDestroyOnLoad(AudioManager);

                occupiedPositions.Add(position);
            }
            if (tileData.type == "End")
            {
                Instantiate(endPoint, position, Quaternion.Euler(0, 0, 0));
                occupiedPositions.Add(position);
            }
        }

        // Add grass on the unoccupied tiles
        for (int x = 0; x <= map.unityMap.properties.size.Xmax; x += 1)
        {
            for (int y = 0; y <= map.unityMap.properties.size.Ymax; y += 1)
            {
                Vector3 lakeCheck1 = new Vector3((float)1.5, 1, 0);
                Vector3 houseCheck = new Vector3(1, 1, 0);
                Vector3 grassPosition = new Vector3(((float)x / 2) + (float)0.25, ((float)y / 2) + (float)0.25, 0);
                if (!occupiedPositions.Contains(grassPosition))
                {
                    Instantiate(grass, grassPosition, Quaternion.identity);
                }
            }
        }
    }

    public void NewMap(UnityMap map)
    {
        SceneManager.LoadScene(1);
        Destroy(player);
        GameManager.Instance.mapToGenerate = map;
    }

    public void ItemMap(Item[] items)
    {
        ClearItems();
        foreach (Item item in items)
        {
            Vector3 position = item.coords;
            position = new Vector3((position.y / 2) + (float)0.25, (position.x / 2) + (float)0.25, position.z);
            switch(item.type)
            {
                case "COIN":
                    Instantiate(Coin, position, Quaternion.Euler(0, 0, 0));

                    break;

                case "BOMB":
                    Instantiate(Bombe, position, Quaternion.Euler(0, 0, 0));

                    break;

                case "WALL":
                    Instantiate(Wall, position, Quaternion.Euler(0, 0, 0));

                    break;
            }
        }
    }

    private void ClearItems()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Coin"))
        {
            Destroy(item);
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Bomb"))
        {
            Destroy(item);
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Destroy(item);
        }
    }

    public void PlayerToSpawn(Vector3 position)
    {
        position = new Vector3((position.x / 2) + (float)0.25, (position.y / 2) + (float)0.25, position.z);
        Debug.Log(position);
        if (player != null)
        {
            
            player.transform.position = position;
        }
        else
        {
            Debug.Log("Le joueur n'existe pas !");
        }
    }
}
