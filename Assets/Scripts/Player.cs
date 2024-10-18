using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float normalSpeed = 5f;    // Vitesse normale
    public float boostSpeed = 10f;    // Vitesse de boost
    public float slowSpeed = 2f;      // Vitesse de ralentissement
    private float currentSpeed;       // Vitesse actuelle
    public Animator animator;
    Vector2 mouvement;

    void Start()
    {
        currentSpeed = normalSpeed;   // Initialisation avec la vitesse normale
    }

    void Update()
    {
        // Capture le mouvement
        mouvement.x = Input.GetAxisRaw("Horizontal");
        mouvement.y = Input.GetAxisRaw("Vertical");

        // Ajuste les paramètres de l'animateur
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);

        // Vérifie si la touche C est enfoncée pour activer le ralentissement
        if (Input.GetKey(KeyCode.C))
        {
            currentSpeed -= slowSpeed;
        }
        // Vérifie si la touche X est enfoncée pour activer le boost
        else if (Input.GetKey(KeyCode.X))
        {
            currentSpeed += boostSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }
    }

    void FixedUpdate()
    {
        // Déplacement du joueur en utilisant la vitesse actuelle
        rb.MovePosition(rb.position + mouvement * currentSpeed * Time.fixedDeltaTime);
    }
}
