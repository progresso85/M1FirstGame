using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    protected string type;
    protected Vector3 position;

    public void setType(string type) { this.type = type; }
    public void setPosition(Vector3 position) { this.position = position; }

}
