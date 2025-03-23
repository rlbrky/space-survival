using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpawnManager : MonoBehaviour
{
    public static MassSpawnManager instance { get; private set; }

    public float enemyDamageScale;
    public float enemyHealthScale;
    public float enemyProjectileScale;
    public float enemySpeedScale;
    public int nextWaveVolume;

    public int currentWave = 1;
    public int waveMaxSpawns = 10;
    public int waveSpawnCount = 0;

    public List<EnemySpawner> enemySpawners;

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

    private void Start()
    {
        enemySpawners = new List<EnemySpawner>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out EnemySpawner enemy))
                enemySpawners.Add(enemy);
        }
    }
}
