using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public TrailRenderer tr;
    public AudioManager audioManager;

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

        // Assigner l'AudioManager trouvé dans la scène
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // Gérer le mouvement
        if (!isStopped && !isDashing)
        {
            mouvement.x = Input.GetAxisRaw("Horizontal");
            mouvement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            mouvement = Vector2.zero;
        }

        // Détecter le mouvement et jouer le son de marche
        if (mouvement.magnitude > 0 && !isStopped)
        {
            // Le joueur se déplace, jouer le son de marche
            if (audioManager != null)
            {
                audioManager.PlayWalkSound();
            }
        }
        else
        {
            // Le joueur s'arrête, arrêter le son de marche
            if (audioManager != null)
            {
                audioManager.StopWalkSound();
            }
        }

        // Animation du joueur
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);

        // Gestion des autres actions (boost, slow, dash)
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        // Mouvement du joueur
        if (!isStopped && !isDashing)
        {
            rb.MovePosition(rb.position + mouvement * currentSpeed * Time.fixedDeltaTime);
        }
    }

    // Boost
    IEnumerator CastBoost()
    {
        isCastingBoost = true;
        yield return new WaitForSeconds(boostCastingTime);
        ActivateBoost();
        isCastingBoost = false;
    }

    void ActivateBoost()
    {
        currentSpeed = boostSpeed;
        StartCoroutine(DisableBoostAfterTime(boostDuration));
        StartCoroutine(BoostCooldown());
    }

    IEnumerator DisableBoostAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentSpeed = normalSpeed;
    }

    IEnumerator BoostCooldown()
    {
        isBoostOnCooldown = true;
        yield return new WaitForSeconds(boostCooldownTime);
        isBoostOnCooldown = false;
    }

    // Toggle
    IEnumerator CastToggle()
    {
        yield return new WaitForSeconds(toggleCastingTime);
        isStopped = true;
        StartCoroutine(DisableToggleAfterCooldown());
    }

    IEnumerator DisableToggleAfterCooldown()
    {
        isToggleOnCooldown = true;
        yield return new WaitForSeconds(toggleCooldownTime);
        isStopped = false;
        isToggleOnCooldown = false;
    }

    // Slow
    IEnumerator CastSlow()
    {
        isCastingSlow = true;
        yield return new WaitForSeconds(slowCastingTime);
        ActivateSlow();
        isCastingSlow = false;
    }

    void ActivateSlow()
    {
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
    }

    IEnumerator SlowCooldown()
    {
        isSlowOnCooldown = true;
        yield return new WaitForSeconds(slowCooldownTime);
        isSlowOnCooldown = false;
    }

    // Dash
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
}
