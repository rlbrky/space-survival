using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSC : MonoBehaviour
{
    [SerializeField] GameObject detonate;
    [SerializeField] Rigidbody rb;
    [SerializeField] float bulletLifeTime = 4f;
    [SerializeField] public float projectileSpeed = 8f;
    [SerializeField] ParticleSystem projectile;
    [SerializeField] ParticleSystem projectileBeam;


    public float damage;
    public Vector3 direction;

    float lifeTimeCounter = 0f;

    private void Start()
    {
        transform.forward = direction;
        rb.velocity = transform.forward * projectileSpeed;
    }

    private void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            rb.velocity = Vector3.zero;
            projectile.Stop();
            projectileBeam.Stop();
            detonate.SetActive(true);
            PlayerSC.instance.TakeDamage(damage);
        }
    }
}
