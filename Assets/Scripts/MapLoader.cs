using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    [SerializeField] GameObject grass;

    HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    void Start()
    {
        string jsonFilePath = Path.Combine(Application.dataPath, "./Scripts/map.json");

        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);

            // Iterate the JSON to save the data in Map object
            Map map = JsonUtility.FromJson<Map>(json);
            LoadMap(map);
        }
    }


    private void LoadMap(Map map)
    {
        
        foreach (TileData tileData in map.properties.tiles)
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
                Instantiate(sidewalk, position, Quaternion.Euler(0, 0, 0));
                occupiedPositions.Add(position);

            }
            else if (tileData.type == "Crosswalk")
            {
                switch (tileData.properties.angle % 360)
                {
                    case 0 :
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
        }
        // Add grass on the unoccupied tiles
        for (int x = 0; x <= map.properties.size.Xmax; x+= 1)
        {
            for (int y = 0; y <= map.properties.size.Ymax; y+= 1)
            {
                Vector3 grassPosition = new Vector3(((float)x / 2) + (float)0.25, ((float)y / 2) + (float)0.25, 0);
                if (!occupiedPositions.Contains(grassPosition))
                {
                    Instantiate(grass, grassPosition, Quaternion.identity);
                }
            }
        }
    }
}
