using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float normalSpeed = 5f;    
    public float boostSpeed = 10f;    
    public float slowSpeed = 2f;      
    public float jumpForce = 5f;      
    public Transform groundCheck;     
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer;     
    private bool isGrounded;          
    private float currentSpeed;       
    private bool isStopped = false;   
    public Animator animator;
    Vector2 mouvement;

    void Start()
    {
        currentSpeed = normalSpeed;   
    }

    void Update()
    {
        
        if (!isStopped)
        {
            mouvement.x = Input.GetAxisRaw("Horizontal");
            mouvement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            mouvement = Vector2.zero; 
        }

        
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);

        
        if (Input.GetKey(KeyCode.C) && !isStopped)
        {
            currentSpeed = slowSpeed;
        }
        
        else if (Input.GetKey(KeyCode.X) && !isStopped)
        {
            currentSpeed = boostSpeed;
        }
        else if (!isStopped)
        {
            currentSpeed = normalSpeed;
        }

        
        if (Input.GetKeyDown(KeyCode.V))
        {
            isStopped = true; 
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            isStopped = false; 
        }

        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isStopped)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
       
        if (!isStopped)
        {
            rb.MovePosition(rb.position + mouvement * currentSpeed * Time.fixedDeltaTime);
        }
    }
}


