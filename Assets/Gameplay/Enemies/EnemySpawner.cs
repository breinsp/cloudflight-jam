using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Building
{
    public GameObject enemyPrefab;
    public float spawnRadius;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Vector2 rand = Random.insideUnitCircle;
        float distance = transform.position.magnitude;
        Vector3 newPos = transform.position + new Vector3(rand.x, 0, rand.y) * spawnRadius;
        newPos = newPos.normalized * distance;

        GameObject enemy = Instantiate(enemyPrefab, newPos, Quaternion.identity);
        enemy.transform.parent = GameManager.instance.enemyHolder;
        GameManager.instance.AddEnemy(enemy.GetComponent<Enemy>());
    }

    public override void BuildingPlaced()
    {
    }
}
