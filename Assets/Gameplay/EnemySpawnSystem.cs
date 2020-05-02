using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public int maxEnemyCount;
    public float spawnDeltaTime;
    private float lastSpawnDeltaTime;
    private Transform enemyHolder;
    private Transform enemySpawnerHolder;
    private Transform buildingsHolder;

    private void Start()
    {
        enemyHolder = GameManager.instance.enemyHolder;
        enemySpawnerHolder = GameManager.instance.enemySpawnerHolder;
        buildingsHolder = BuildSystem.instance.buildingsHolder;
    }

    // Update is called once per frame
    void Update()
    {
        RecalculateMaxEnemyCount();
        if(maxEnemyCount >= enemyHolder.childCount && lastSpawnDeltaTime >= spawnDeltaTime)
        {
            SpawnEnemy();
            lastSpawnDeltaTime = 0;
        }
        lastSpawnDeltaTime += Time.deltaTime;
    }

    private void RecalculateMaxEnemyCount()
    {
        int turretCount = 0;
        int houseCount = 0;
        foreach(Transform t in buildingsHolder)
        {
            Building b = t.GetComponent<Building>();
            if(b.name == "Turret")
            {
                turretCount++;
            } 
            else if(b.name == "House")
            {
                houseCount++;
            }
        }
        maxEnemyCount = Mathf.RoundToInt(GameManager.instance.maxPopulation / 6) + turretCount * 3 + houseCount;
        spawnDeltaTime = 20f / maxEnemyCount;
    }

    private void SpawnEnemy()
    {
        int childCount = enemySpawnerHolder.childCount;
        int randomIndex = UnityEngine.Random.Range(0, childCount);
        Transform spawnerTransform = enemySpawnerHolder.GetChild(randomIndex);
        EnemySpawner enemySpawner = spawnerTransform.GetComponent<EnemySpawner>();
        enemySpawner.SpawnEnemy();
    }
}
