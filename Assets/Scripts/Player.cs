using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    public float normalSpeed = 5f;    
    public float boostSpeed = 10f;   
    public float slowSpeed = 2f;      
    private float currentSpeed;

    // boost (X)
    public float boostCastingTime = 2f;    
    public float boostDuration = 3f;       
    public float boostCooldownTime = 5f;   
    private bool isBoostOnCooldown = false;
    private bool isCastingBoost = false;

    // toggle (V)
    public float toggleCastingTime = 1f;    
    public float toggleCooldownTime = 3f;   
    private bool isToggleOnCooldown = false;
    private bool isStopped = false;         

    // slow (C)
    public float slowCastingTime = 1f;    
    public float slowDuration = 4f;       
    public float slowCooldownTime = 5f;   
    private bool isSlowOnCooldown = false;
    private bool isCastingSlow = false;
    private bool isSlowed = false;

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

        
        if (Input.GetKeyDown(KeyCode.X) && !isBoostOnCooldown && !isCastingBoost)
        {
            StartCoroutine(CastBoost());
        }

        
        if (Input.GetKeyDown(KeyCode.V) && !isToggleOnCooldown && !isStopped)
        {
            StartCoroutine(CastToggle());
        }

        
        if (Input.GetKeyDown(KeyCode.C) && !isSlowOnCooldown && !isCastingSlow && !isSlowed)
        {
            StartCoroutine(CastSlow());
        }
    }

    void FixedUpdate()
    {
        
        if (!isStopped)
        {
            rb.MovePosition(rb.position + mouvement * currentSpeed * Time.fixedDeltaTime);
        }
    }

    
    IEnumerator CastBoost()
    {
        isCastingBoost = true;
        Debug.Log("Boost en cours de casting...");

        
        yield return new WaitForSeconds(boostCastingTime);

        
        ActivateBoost();
        isCastingBoost = false;
    }

    void ActivateBoost()
    {
        Debug.Log("Boost activé !");
        currentSpeed = boostSpeed;

        
        StartCoroutine(DisableBoostAfterTime(boostDuration));

        
        StartCoroutine(BoostCooldown());
    }

    IEnumerator DisableBoostAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentSpeed = normalSpeed;
        Debug.Log("Boost terminé, retour à la vitesse normale.");
    }

    IEnumerator BoostCooldown()
    {
        isBoostOnCooldown = true;
        Debug.Log("Cooldown du boost en cours...");

        
        yield return new WaitForSeconds(boostCooldownTime);

        isBoostOnCooldown = false;
        Debug.Log("Cooldown terminé, boost disponible !");
    }

    
    IEnumerator CastToggle()
    {
        Debug.Log("Toggle en cours de casting...");

        
        yield return new WaitForSeconds(toggleCastingTime);

        
        isStopped = true;
        Debug.Log("Joueur stoppé !");

       
        StartCoroutine(DisableToggleAfterCooldown());
    }

    
    IEnumerator DisableToggleAfterCooldown()
    {
        isToggleOnCooldown = true;
        Debug.Log("Cooldown du toggle en cours...");

        
        yield return new WaitForSeconds(toggleCooldownTime);

        
        isStopped = false;
        isToggleOnCooldown = false;
        Debug.Log("Cooldown terminé, joueur relancé automatiquement !");
    }

    
    IEnumerator CastSlow()
    {
        isCastingSlow = true;
        Debug.Log("Slow en cours de casting...");

       
        yield return new WaitForSeconds(slowCastingTime);

        
        ActivateSlow();
        isCastingSlow = false;
    }

    void ActivateSlow()
    {
        Debug.Log("Slow activé !");
        currentSpeed = slowSpeed;
        isSlowed = true;

        
        StartCoroutine(DisableSlowAfterTime(slowDuration));

        
        StartCoroutine(SlowCooldown());
    }

    IEnumerator DisableSlowAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentSpeed = normalSpeed;
        isSlowed = false;
        Debug.Log("Slow terminé, retour à la vitesse normale.");
    }

    IEnumerator SlowCooldown()
    {
        isSlowOnCooldown = true;
        Debug.Log("Cooldown du slow en cours...");

        
        yield return new WaitForSeconds(slowCooldownTime);

        isSlowOnCooldown = false;
        Debug.Log("Cooldown terminé, slow disponible !");

    }
}

