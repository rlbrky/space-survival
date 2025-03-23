using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    [SerializeField] float bulletLifeTime = 4f;
    [SerializeField] float speed = 7f;

    float lifeTimeCounter = 0f;


    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTimeCounter += Time.deltaTime;
        if(lifeTimeCounter >= bulletLifeTime)
        {
            Destroy(transform.root.gameObject);
        }
    }

    public void OnEnemyHit(IEnemyAI enemy)
    {
        if (PlayerSC.instance.isLifeStealing)
        {
            PlayerSC.instance.health += 0.5f;
            PlayerSC.instance.healthbar.value = PlayerSC.instance.health;
        }
        float random = Random.value;
        if (random < PlayerSC.instance.critChance)
        {
            enemy.GetHit(PlayerSC.instance.damage * PlayerSC.instance.critDamageMultiplier, PlayerSC.instance.knockbackForce);
        }
        else
            enemy.GetHit(PlayerSC.instance.damage, PlayerSC.instance.knockbackForce);

       
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEnemyAI enemy))
            OnEnemyHit(enemy);
    }
}
