using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;

public class PlayerSC : MonoBehaviour
{
    public static PlayerSC instance {  get; private set; }
    [Header("Necessary Objs")]
    public GameObject camLookTarget;
    public GameObject riggedVersion;
    public GameObject playerMesh;
    public Rigidbody riggedVersionRB;
    public GameObject playerCanvas;
    public Collider _collider;
    public Slider healthbar;
    public LayerMask whatIsEnemy;
    public AudioSource pickupSFX;
    public ShiftColorForDMG takeDamageEffect;
    public AudioSource getHitBody;
    public AudioSource getHitVocal;
    public AudioSource critHitSFX;
    
    private Animator animator;

    [Header("Stats")]
    public float maxHealth;
    public float damage;
    public float critChance = 0.1f;
    public float critDamageMultiplier = 1.5f;
    public float fireRate = 0.7f;
    public float speed = 5f;
    public float range = 5f;
    public float knockbackForce;
    
    public DroneSC[] spells;
    public int activeSpellCount;

    [Header("Powerup Stats")]
    public float exp; //Controls character level.
    public float expThreshold = 200f; //Will scale.
    public float keys = 0;

    public bool isLifeStealing;
    public bool hasKnockback;

    private int movementHash;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public float health;
    [HideInInspector] public bool isDead;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movementHash = Animator.StringToHash("isMoving");
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
        //Set each drone's values.
        spells = GetComponentsInChildren<DroneSC>();
        activeSpellCount = 1;
        foreach(DroneSC spell in spells)
        {
            spell.whatIsEnemy = whatIsEnemy;
            spell.gameObject.SetActive(false);
        }
        spells[0].gameObject.SetActive(true);
        exp = 0;

        healthbar.maxValue = maxHealth;
        healthbar.value = health;

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        //Movement();
    }

    public void Movement(Vector2 movementInput)
    {
        //animator.SetFloat(movementXHash, movementInput.x);
        //animator.SetFloat(movementYHash, movementInput.y);

        if (movementInput != Vector2.zero)
            animator.SetBool(movementHash, true);
        else
            animator.SetBool(movementHash, false);


        //Vector3 result = fwdRelativeInput + rightRelativeInput;
        Vector3 result = new Vector3(movementInput.x, 0, movementInput.y);

        rb.velocity = result * speed;
    }

    public void LevelUP()
    {
        Time.timeScale = 0f;
        exp = 0;
        expThreshold += 200f;
        PowerUpCardManager.instance.CreateCards_LevelUp();
        maxHealth += 5f;
        //health += maxHealth / 10;
        ExpUI_Manager.instance.characterExpUI.maxValue = expThreshold;
        ExpUI_Manager.instance.playerText.text = exp + " / " + expThreshold;
    }



    public void TakeDamage(float incDamage)
    {
        if(!isDead)
        {
            getHitBody.Play();
            getHitVocal.Play();
            takeDamageEffect.StartShiftingColor();
            health -= incDamage;
            healthbar.value = health;
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        //Play SFX and maybe VFX.
        if (PlayerTouchInputs.instance != null)
        {
            PlayerTouchInputs.instance.DisableTouchStuff();
            PlayerTouchInputs.instance.enabled = false;
        }
        Destroy(takeDamageEffect);
        Destroy(playerCanvas);
        isDead = true;
        Destroy(_collider);
        Destroy(animator);
        Destroy(playerMesh);
        riggedVersion.SetActive(true);
        riggedVersionRB.AddForce(transform.forward * 14, ForceMode.Impulse);
        Destroy(rb);
        StartCoroutine(DeathUICoroutine());
    }

    IEnumerator DeathUICoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        AdManager.instance.ShowInterstitialAd();
        ExpUI_Manager.instance.ActivateDeathUI();
    }
}
