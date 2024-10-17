using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    protected string name;
    protected string type;
    protected string description;
    protected int positionX;
    protected int positionY;
    protected int positionZ;
    protected int angle;

    public void setName(string name) {  this.name = name; }
    public void setType(string type) { this.type = type; }
    public void setDescription(string description) { this.description = description; }
    public void setPositionX(int x) { this.positionX = x; }
    public void setPositionY(int y) { this.positionY = y; }
    public void setPositionZ(int z) { this.positionZ = z; }
    public void setAngle(int angle) { this.angle = angle; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
