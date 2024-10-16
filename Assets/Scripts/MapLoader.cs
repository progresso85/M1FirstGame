using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapLoader : MonoBehaviour
{
    [SerializeField] GameObject road;
    [SerializeField] GameObject sidewalk;
    [SerializeField] GameObject grass;

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
        HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

        foreach (TileData tileData in map.properties.tiles)
        {
            // Choose prefab with its type
            if (tileData.type == "Road")
            {
                Vector3 position = tileData.properties.position;
                Instantiate(road, position, Quaternion.Euler(0, tileData.properties.angle, 0));
                occupiedPositions.Add(position);
            }
            else if (tileData.type == "Sidewalk")
            {
                Vector3 position = tileData.properties.position;
                Instantiate(sidewalk, position, Quaternion.Euler(0, tileData.properties.angle, 0));
                occupiedPositions.Add(position);

            }

            // Add grass on the unoccupied tiles
            for (int x = map.properties.size.Xmin; x <= map.properties.size.Xmax; x++)
            {
                for (int y = map.properties.size.Ymin; y <= map.properties.size.Ymax; y++)
                {
                    Vector3 grassPosition = new Vector3(x, y, 0);
                    if (!occupiedPositions.Contains(grassPosition))
                    {
                        Instantiate(grass, grassPosition, Quaternion.identity);
                    }
                }
            }
        }
    }
}
