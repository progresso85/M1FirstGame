using UnityEngine;

public class SpriteSpawner : MonoBehaviour
{
    public Sprite sprite;  

    void Start()
    {
        GameObject spriteObject = new GameObject("New Sprite");
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Positionner le sprite dans la map
        spriteObject.transform.position = new Vector3(2, 0, 0); ;  
    }
}
