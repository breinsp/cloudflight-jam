using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerInitializer : MonoBehaviour
{
    private const float ENEMY_SPAWNER_DISTANCE = 80;

    public BuildingEntity enemySpawnerEntity;

    void Start()
    {
        Vector3 enemySpawner1 = new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f)).normalized * ENEMY_SPAWNER_DISTANCE;
        Vector3 enemySpawner2 = new Vector3(Random.Range(0f, 1f), 0, Random.Range(-1f, 0f)).normalized * ENEMY_SPAWNER_DISTANCE;
        Vector3 enemySpawner3 = new Vector3(Random.Range(-1f, 0f), 0, Random.Range(0f, 1f)).normalized * ENEMY_SPAWNER_DISTANCE;
        Vector3 enemySpawner4 = new Vector3(Random.Range(-1f, 0f), 0, Random.Range(-1f, 0f)).normalized * ENEMY_SPAWNER_DISTANCE;

        BuildSystem.instance.SpawnBuilding(enemySpawner1, enemySpawnerEntity);
        BuildSystem.instance.SpawnBuilding(enemySpawner2, enemySpawnerEntity);
        BuildSystem.instance.SpawnBuilding(enemySpawner3, enemySpawnerEntity);
        BuildSystem.instance.SpawnBuilding(enemySpawner4, enemySpawnerEntity);
    }
}
