using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public float spawnDeltaTime;
    public float lastSpawnDeltaTime;
    public Transform enemySpawnerHolder;

    private void Start()
    {
        enemySpawnerHolder = GameManager.instance.enemySpawnerHolder;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastSpawnDeltaTime >= spawnDeltaTime)
        {
            SpawnEnemy();
            lastSpawnDeltaTime = 0;
        }
        lastSpawnDeltaTime += Time.deltaTime;
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
