using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAIMelee : MonoBehaviour, IEnemyAI
{
    [Header("General")]
    [SerializeField] private float _speed;
    [SerializeField] private GameObject expCube;
    [SerializeField] private GameObject key;
    [SerializeField] private Slider healthBar;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private TextMeshProUGUI damageNumbers;
    [SerializeField] private NavMeshAgent agent;

    [Header("Combat")]
    public float attackRange;
    public float attackCD;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private ShiftColorForDMG _damageTakeEffect;

    [HideInInspector]
    public float currentHealth;

    private Collider _collider;
    private Animator animator;
    private Rigidbody rb;

    private Vector3 damageNumberStartPos;
    private Vector3 textSpeed = new Vector3(0, 3f, 0);
    private bool isAlive;
    private float attackCounter = 0;
    private int movementHash;
    private float _damage;
    private float _maxHealth;
    private int choosingPathFrame;
    private float _projectileSpeed;

    public float damage { get => _damage; set => _damage = value; }
    public float maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float health { get => currentHealth; set => currentHealth = value; }
    public float projectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }
    public float speed { get => _speed; set => _speed = value; }
    private void Start()
    {
        maxHealth = 12;
        damage = 10;
        speed = 7;
        movementHash = Animator.StringToHash("isMoving");
        attackHitbox.SetActive(false);
        currentHealth = maxHealth;
        isAlive = true;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        damageNumberStartPos = damageNumbers.transform.localPosition;
        damageNumbers.gameObject.SetActive(false);
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        choosingPathFrame = Random.Range(10, 31);
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        UpdateDamageTextPos();

        if(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), PlayerSC.instance.transform.position) <= attackRange && attackCounter >= attackCD)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            animator.SetBool(movementHash, false);
            attackCounter = 0f;
            animator.SetTrigger("Attack");
            return;
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool(movementHash, true);
            //transform.LookAt(new Vector3(PlayerSC.instance.transform.position.x, transform.position.y, PlayerSC.instance.transform.position.z));
            if (Time.frameCount % choosingPathFrame == 0) //Set Destination every x frames.
                agent.SetDestination(PlayerSC.instance.transform.position);
        }
        attackCounter += Time.deltaTime;
    }

    public void AttackAnimEvent()
    {
        attackHitbox.SetActive(true);
    }

    public void EndAttackAnimEvent()
    {
        attackHitbox.SetActive(false);
    }

    public void GetHit(float incDamage, float explosionForce)
    {
        _damageTakeEffect.StartShiftingColor();
        if(PlayerSC.instance.hasKnockback)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            rb.AddExplosionForce(explosionForce, transform.forward, 1);
            StartCoroutine(ResetSpeed());
        }

        currentHealth -= incDamage;
        healthBar.value = currentHealth;
        damageNumbers.gameObject.SetActive(true);
        damageNumbers.transform.localPosition = damageNumberStartPos;
        if(incDamage == PlayerSC.instance.damage * PlayerSC.instance.critDamageMultiplier)
        {
            damageNumbers.text = "<color=#F6440B>" + incDamage.ToString("0.0") + "</color>";
            PlayerSC.instance.critHitSFX.Play();
        }
        else
            damageNumbers.text = incDamage.ToString("0.0");
        StartCoroutine(CloseUICoroutine());
        if(currentHealth <= 0 && isAlive)
        {
            Destroy(rb);
            isAlive = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            healthBar.gameObject.SetActive(false);
            Destroy(_collider);
            StartCoroutine(DeathCoroutine());
        }
    }

    private void UpdateDamageTextPos()
    {
        damageNumbers.transform.position += textSpeed * Time.deltaTime;
    }

    IEnumerator DeathCoroutine()
    {
        animator.SetTrigger("Die");
        deathEffect.Play();
        yield return new WaitForSeconds(2f);
        Destroy(agent);
        GameManager.instance.UpdateWave();
        for(int i = 0; i < Random.Range(1, 6); i++) //Spawns exp randomly.
        {
            var pf = Instantiate(expCube, transform.position, Quaternion.identity);
            var pfSC = pf.GetComponent<EXP>();
            pfSC.expValue = GameManager.instance.expValue;
            pfSC.speed = GameManager.instance.expNgemTravelSpeed;
            pfSC.expDropCollider.radius = GameManager.instance.expNgemPickupRadius;

            pf.GetComponent<Rigidbody>().AddExplosionForce(3f, transform.position, 2f);
        }
        for (int i = 0; i < Random.Range(0, 6); i++) //Spawns key randomly.
        {
            var pf = Instantiate(key, transform.position, Quaternion.identity);
            var pfSC = pf.GetComponent<GemSC>();
            pfSC.speed = GameManager.instance.expNgemTravelSpeed;
            pfSC.gemDropCollider.radius = GameManager.instance.expNgemPickupRadius;

            pf.GetComponent<Rigidbody>().AddExplosionForce(3f, transform.position, 2f);
        }
        Destroy(gameObject);
    }

    IEnumerator CloseUICoroutine()
    {
        yield return new WaitForSeconds(1f);
        damageNumbers.gameObject.SetActive(false);
    }

    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        agent.speed = _speed;
    }
}
