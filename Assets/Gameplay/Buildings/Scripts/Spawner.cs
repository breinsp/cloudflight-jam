﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
    public GameObject minionPrefab;
    public float spawnRadius;
    public float spawnTimeDelta;

    private float lastSpawnTimeDelta;

    void Update()
    {
        lastSpawnTimeDelta += Time.deltaTime;
        if (lastSpawnTimeDelta >= spawnTimeDelta && GameManager.instance.RemainingPop > 0)
        {
            SpawnMinion();
            lastSpawnTimeDelta = 0;
        }
    }

    private void SpawnMinion()
    {
        float angle = UnityEngine.Random.Range(0, 2*Mathf.PI);
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);
        Vector3 newPos = new Vector3(x * spawnRadius + transform.position.x, transform.position.y, z * spawnRadius + transform.position.z);
        GameObject minion = Instantiate(minionPrefab, newPos, Quaternion.identity);
        minion.transform.parent = GameManager.instance.minionHolder;
        GameManager.instance.AddMinion(minion.GetComponent<Minion>());
    }

    public override void BuildingPlaced()
    {
    }
}
