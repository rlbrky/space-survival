using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAI
{
    public float health { get; set; }
    public float damage {  get; set; }
    public float maxHealth { get; set; }
    public float speed { get; set; }
    public float projectileSpeed { get; set; }
    public void GetHit(float incDamage, float explosionForce);
}
