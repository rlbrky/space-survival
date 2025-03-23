using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] TextMeshProUGUI waveCounterText;
    [SerializeField] TextMeshProUGUI waveAnnouncerText;
    [SerializeField] TextMeshProUGUI waveLevelText;

    public EXP expPrefab;
    public GemSC gemPrefab;

    public float expValue;
    public float expNgemPickupRadius;
    public float expNgemTravelSpeed;

    public float waveCounter = 5; //5 secs.

    public int waveExp; //Fills as you kill enemies and when you hit the threshold things will get stronger.

    private int waveLevel = 1;
    private bool startCounting;

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
        expValue = expPrefab.expValue;
        expNgemTravelSpeed = expPrefab.speed;
        expNgemPickupRadius = expPrefab.expDropCollider.radius;

        waveCounterText.gameObject.SetActive(false);
        waveAnnouncerText.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(startCounting)
        {
            //waveCounterText.gameObject.SetActive(true);
            //waveAnnouncerText.gameObject.SetActive(true);
            waveCounter -= Time.deltaTime;
            waveCounterText.text = waveCounter.ToString("0");
        }

        if(waveCounter < 0)
        {
            startCounting = false;
            waveCounter = 5;
            NextWave();
        }
    }

    public void UpdateWave()
    {
        waveExp++;

        if (MassSpawnManager.instance.waveSpawnCount == MassSpawnManager.instance.waveMaxSpawns && waveExp >= MassSpawnManager.instance.waveMaxSpawns)
        {
            waveCounterText.gameObject.SetActive(true);
            waveAnnouncerText.gameObject.SetActive(true);
            startCounting = true;
        }

        ExpUI_Manager.instance.waveExpUI.value = waveExp;
        ExpUI_Manager.instance.waveText.text = waveExp + " / " + MassSpawnManager.instance.waveMaxSpawns;
    }

    public void NextWave()
    {
        waveCounterText.gameObject.SetActive(false);
        waveAnnouncerText.gameObject.SetActive(false);

        foreach (EnemySpawner enemy in MassSpawnManager.instance.enemySpawners)
        {
            var _enemy = enemy.prefabToSpawn.GetComponent<IEnemyAI>();
            _enemy.maxHealth *= MassSpawnManager.instance.enemyHealthScale;
            _enemy.damage *= MassSpawnManager.instance.enemyDamageScale;
            _enemy.speed *= MassSpawnManager.instance.enemySpeedScale;
            _enemy.projectileSpeed *= MassSpawnManager.instance.enemyProjectileScale;
        }
        MassSpawnManager.instance.currentWave++;
        MassSpawnManager.instance.waveSpawnCount = 0;
        waveExp = 0;
        MassSpawnManager.instance.waveMaxSpawns += MassSpawnManager.instance.nextWaveVolume;
        ExpUI_Manager.instance.waveExpUI.maxValue = MassSpawnManager.instance.waveMaxSpawns;

        ExpUI_Manager.instance.waveExpUI.value = waveExp;
        ExpUI_Manager.instance.waveText.text = waveExp + " / " + MassSpawnManager.instance.waveMaxSpawns;

        if (MassSpawnManager.instance.currentWave % 4 == 0)
        {
            AdManager.instance.ShowInterstitialAd();
            waveLevel++;
            MassSpawnManager.instance.currentWave = 1;
        }
        waveLevelText.text = waveLevel + "-" + MassSpawnManager.instance.currentWave;
    }
}
