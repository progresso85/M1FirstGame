using UnityEngine;

[System.Serializable]
public class Map
{
    public string name;
    public string type;
    public string description;
    public Properties properties;
}

[System.Serializable]
public class Properties
{
    public Size size;
    public TileData[] tiles;
}

[System.Serializable]
public class Size
{
    public int Xmin;
    public int Xmax;
    public int Ymin;
    public int Ymax;
}

[System.Serializable]
public class TileData
{
    public string type;
    public string description;
    public TileProperties properties;
}

[System.Serializable]
public class TileProperties
{
    public Vector3 position;
    public int angle;
}
