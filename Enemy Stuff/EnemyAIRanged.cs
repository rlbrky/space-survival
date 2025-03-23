using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAIRanged : MonoBehaviour, IEnemyAI
{
    [Header("General")]
    [SerializeField] private float _speed; 
    [SerializeField] private GameObject expCube;
    [SerializeField] private GameObject key;
    [SerializeField] private Slider healthBar;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private TextMeshProUGUI damageNumbers;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform firingPoint;

    [Header("Combat")]
    public float attackRange;
    public float attackCD;
    private float _projectileSpeed = 15f;
    [SerializeField] private GameObject bullet;
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

    public float damage { get => _damage; set => _damage = value; }
    public float maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float health { get => currentHealth; set => currentHealth = value; }
    public float speed { get => _speed; set => _speed = value; }
    public float projectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 10;
        damage = 30;
        speed = 5;
        projectileSpeed = 15;
        movementHash = Animator.StringToHash("isMoving");
        currentHealth = _maxHealth;
        isAlive = true;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        damageNumberStartPos = damageNumbers.transform.localPosition;
        damageNumbers.gameObject.SetActive(false);
        healthBar.maxValue = _maxHealth;
        healthBar.value = currentHealth;
        choosingPathFrame = Random.Range(10, 31);
        agent.stoppingDistance = attackRange;
        agent.speed = speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        UpdateDamageTextPos();

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), PlayerSC.instance.transform.position) <= attackRange && attackCounter >= attackCD)
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
            animator.SetBool(movementHash, true);
            transform.LookAt(new Vector3(PlayerSC.instance.transform.position.x, transform.position.y, PlayerSC.instance.transform.position.z));
            //rb.velocity = transform.forward * speed;
            if (Time.frameCount % choosingPathFrame == 0) //Set Destination every x frames.
                agent.SetDestination(PlayerSC.instance.transform.position);
        }
        attackCounter += Time.deltaTime;
    }

    public void GetHit(float incDamage, float explosionForce)
    {
        _damageTakeEffect.StartShiftingColor();
        if (PlayerSC.instance.hasKnockback)
        {
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            rb.AddExplosionForce(explosionForce, transform.forward, 1);
            StartCoroutine(ResetSpeed());
        }

        currentHealth -= incDamage;
        healthBar.value = currentHealth;
        damageNumbers.gameObject.SetActive(true);
        damageNumbers.transform.localPosition = damageNumberStartPos;
        if (incDamage == PlayerSC.instance.damage * PlayerSC.instance.critDamageMultiplier)
        {
            damageNumbers.text = "<color=#F6440B>" + incDamage.ToString("0.0") + "</color>";
            PlayerSC.instance.critHitSFX.Play();
        }
        else
            damageNumbers.text = incDamage.ToString("0.0");
        StartCoroutine(CloseUICoroutine());
        if (currentHealth <= 0 && isAlive)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            healthBar.gameObject.SetActive(false);
            isAlive = false;
            Destroy(_collider);
            Destroy(rb);
            StartCoroutine(DeathCoroutine());
        }
    }

    public void AttackEvent()
    {
        var _bullet = Instantiate(bullet, firingPoint.position, Quaternion.identity);
        EnemyBulletSC bulletSC = _bullet.GetComponent<EnemyBulletSC>();
        bulletSC.projectileSpeed = _projectileSpeed;
        bulletSC.direction = transform.forward;
        bulletSC.damage = _damage;
    }

    public void AttackStarter()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void AttackEnd()
    {
        agent.isStopped = false;
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
        for (int i = 0; i < Random.Range(1, 6); i++) //Spawns exp randomly.
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
