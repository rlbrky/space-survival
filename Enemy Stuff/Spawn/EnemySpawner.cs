using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] public GameObject prefabToSpawn;

    [Header("Variables")]
    [SerializeField] private float spawnRate = 1f;
    
    private float spawnCounter = 0;

    // Update is called once per frame
    void Update()
    {
        if(MassSpawnManager.instance.waveSpawnCount < MassSpawnManager.instance.waveMaxSpawns)
        {
            spawnCounter += Time.deltaTime;

            if (spawnCounter > spawnRate)
            {
                spawnCounter = 0;
                Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                MassSpawnManager.instance.waveSpawnCount++;
            }
        }
    }
}
