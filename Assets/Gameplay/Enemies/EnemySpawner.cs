using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
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
            RaycastHit hit;
            if (!Physics.SphereCast(transform.position, 2f, Vector3.up, out hit, 2f))
            {
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 1.5f,transform.position.z);
        GameObject enemy = Instantiate(enemyPrefab, newPos, Quaternion.identity);
        enemy.transform.parent = GameManager.instance.enemyHolder;
        GameManager.instance.AddEnemy(enemy.GetComponent<Enemy>());
    }
}
