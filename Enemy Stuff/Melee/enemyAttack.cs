using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [SerializeField] EnemyAIMelee enemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerSC.instance.TakeDamage(enemy.damage);
        }
    }
}
