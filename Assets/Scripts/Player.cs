using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public TrailRenderer tr;

    public float normalSpeed = 2f;
    public float boostSpeed = 10f;
    public float slowSpeed = 2f;
    private float currentSpeed;

    public float boostDuration = 5f;
    public float toggleDuration = 5f;
    public float slowDuration = 5f;
    public float slowCastingTime = 1f;
    public float boostCastingTime = 1f;
    public float toggleCastingTime = 1f;

    private bool isStopped = false;
    private bool isSlowed = false;
    private bool isCastingBoost = false;
    private bool isCastingSlow = false;

    // Mode ivresse
    public float drunkCastingTime = 1f;
    public float drunkDuration = 5f;
    private bool isDrunk = false;

    // Dash
    private bool canDash = true;
    private bool isDashing = false;
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 3f;

    public Animator animator;
    private Vector2 mouvement;

    // Référence à l'AudioManager
    private AudioManager audioManager;

    public Image imageComponent;
    public Sprite[] renderSprite;

    private GameObject boostObject;
    private GameObject stunObject;
    private GameObject slowObject;
    private GameObject drunkObject;


    void Start()
    {
        currentSpeed = normalSpeed;

        // Recherche et assignation de l'AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        DontDestroyOnLoad(audioManager);
    }

    void Update()
    {
        if (!isStopped && !isDashing)
        {
            mouvement.x = isDrunk ? -Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal");
            mouvement.y = isDrunk ? -Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical");

            //Détection du mouvement pour jouer / arrêter le son de marche
                if (mouvement.magnitude > 0 && !audioManager.IsWalkSoundPlaying())
            {
                audioManager.PlayWalkSound();
            }
        }
        else
        {
            mouvement = Vector2.zero;
            audioManager.StopWalkSound(); // Arrêter le son si le joueur est stoppé
        }

        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);

        if (slowObject == null)
        {
            SetDisplaySpell();
            Debug.Log(slowObject);
        }

        StartCoroutine(SpellCast(GameManager.Instance.spell));

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (GameManager.Instance.speedCoin == "COIN")
        {
            ActivateBoost();
            GameManager.Instance.speedCoin = "";
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
                if (boostObject != null)
                {
                    boostObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("L'objet Boost n'a pas été trouvé.");
                }
        Debug.Log("Boost terminé, retour à la vitesse normale.");
    }

    IEnumerator DisableToggleAfterCooldown()
    {
        yield return new WaitForSeconds(toggleDuration);
        isStopped = false;
                if (stunObject != null)
                {
                    stunObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("L'objet Stun n'a pas été trouvé.");
                }
        Debug.Log("Joueur relancé automatiquement !");
    }

    void ActivateSlow()
    {
        Debug.Log("Slow activé !");

        currentSpeed = slowSpeed;
        isSlowed = true;
                if (slowObject != null)
                {
                    slowObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("L'objet Slow n'a pas été trouvé.");
                }
        StartCoroutine(DisableSlowAfterDuration(slowDuration));
    }

    public void ActivateBoost()
    {
        Debug.Log("Boost activé !");
        currentSpeed = boostSpeed;
        Debug.Log(boostObject);
        if (boostObject != null)
                {
                    boostObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("L'objet Boost n'a pas été trouvé.");
                }
        StartCoroutine(DisableBoostAfterTime(boostDuration));
    }

    void ActivateDrunk()
    {
        Debug.Log("Drunk activé !");
        isDrunk = true;
                if (drunkObject != null)
                {
                    drunkObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("L'objet Drunk n'a pas été trouvé.");
                }
        StartCoroutine(DisableDrunkAfterTime(drunkDuration));
    }

    IEnumerator DisableSlowAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentSpeed = normalSpeed;
        isSlowed = false;
        Debug.Log(slowObject);
                if (slowObject != null)
                {
                    slowObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("L'objet Slow n'a pas été trouvé.");
                }
        Debug.Log("Slow terminé, retour à la vitesse normale.");
    }

    IEnumerator DisableDrunkAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDrunk = false;
        Debug.Log(drunkObject);
        if (drunkObject != null)
                {
                    drunkObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("L'objet Drunk n'a pas été trouvé.");
                }
        Debug.Log("Vous n'êtes plus ivre !");
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
                Debug.Log(stunObject);
                if (stunObject != null)
                {
                    stunObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("L'objet Stun n'a pas été trouvé.");
                }
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

    void SetDisplaySpell()
    {
        boostObject = GameObject.Find("Boost");
        if (boostObject != null)
        {
            boostObject.SetActive(false);
        }
        stunObject = GameObject.Find("Stun");
        if (stunObject != null)
        {
            stunObject.SetActive(false);
        }
        drunkObject = GameObject.Find("Drunk");
        if (drunkObject != null)
        {
            drunkObject.SetActive(false);
        }
        slowObject = GameObject.Find("Slow");
        if (slowObject != null)
        {
            slowObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class Spell
{
    public string name;
}

