using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public TrailRenderer tr;

    public string id;
    public float normalSpeed = 2f;
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

    // drunk
    public float drunkCastingTime = 1f;
    public float drunkDuration = 5f;
    private bool isDrunk = false;

    // dash
    private bool canDash = true;
    private bool isDashing = false;
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 3f;

    public Animator animator;
    Vector2 mouvement;

    void Start()
    {
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        
        if (!isStopped && !isDashing)
        {
            mouvement.x = isDrunk ? -Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal");
            mouvement.y = isDrunk ? -Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical");
        }
        else
        {
            mouvement = Vector2.zero;
        }

       
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);

        StartCoroutine(SpellCast(GameManager.Instance.spell));
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        
        if (!isStopped && !isDashing)
        {
            rb.MovePosition(rb.position + mouvement * currentSpeed * Time.fixedDeltaTime);
        }
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

    IEnumerator DisableToggleAfterCooldown()
    {
        isToggleOnCooldown = true;
        Debug.Log("Cooldown du toggle en cours...");
        yield return new WaitForSeconds(toggleCooldownTime);
        isStopped = false;
        isToggleOnCooldown = false;
        Debug.Log("Cooldown terminé, joueur relancé automatiquement !");
    }

    void ActivateSlow()
    {
        Debug.Log("Slow activé !");
        currentSpeed = slowSpeed;
        isSlowed = true;
        StartCoroutine(DisableSlowAfterTime(slowDuration));
    }

    void ActivateBoost()
    {

        Debug.Log("Boost activé !");
        currentSpeed = boostSpeed;

        StartCoroutine(DisableBoostAfterTime(boostDuration));
    }

    void ActivateDrunk()
    {
        Debug.Log("Drunk activé !");
        isDrunk = true;
        StartCoroutine(DisableDrunkAfterTime(drunkDuration));

    }

    IEnumerator DisableSlowAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentSpeed = normalSpeed;
        isSlowed = false;
        Debug.Log("Slow terminé, retour à la vitesse normale.");
    }

    IEnumerator DisableDrunkAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDrunk = false;
        Debug.Log("Vous n'êtes plus bourré !");
    }

    IEnumerator SlowCooldown()
    {
        isSlowOnCooldown = true;
        Debug.Log("Cooldown du slow en cours...");
        yield return new WaitForSeconds(slowCooldownTime);
        isSlowOnCooldown = false;
        Debug.Log("Cooldown terminé, slow disponible !");
    }

    
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;  
        rb.gravityScale = 0f;  
        rb.velocity = new Vector2(mouvement.x * dashingPower, mouvement.y * dashingPower);  
        tr.emitting = true;  
        yield return new WaitForSeconds(dashingTime);  
        tr.emitting = false;  
        rb.gravityScale = originalGravity;  
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);  
        canDash = true;
    }

    public IEnumerator SpellCast(Spell spell)
    {
        switch (spell.name)
        {
            case "Slow Mode":
                isCastingSlow = true;
                spell.name = "";
                Debug.Log("Slow en cours de casting...");
                yield return new WaitForSeconds(slowCastingTime);
                ActivateSlow();
                isCastingSlow = false;
                break;
            case "Quickness":
                isCastingBoost = true;
                spell.name = "";
                Debug.Log("Boost en cours de casting...");

                yield return new WaitForSeconds(boostCastingTime);
                ActivateBoost();
                isCastingBoost = false;
                break;
            case "Sudden Stop":
                Debug.Log("Toggle en cours de casting...");
                spell.name = "";
                yield return new WaitForSeconds(toggleCastingTime);
                isStopped = true;
                Debug.Log("Joueur stoppé !");
                StartCoroutine(DisableToggleAfterCooldown());
                break;
            case "Drunk Mode":
                Debug.Log("Drunk en cours de casting...");
                spell.name = "";
                yield return new WaitForSeconds(drunkCastingTime);
                ActivateDrunk();
                break;
        }
    }
}

[System.Serializable]
public class Spell
{
    public string name;
}

