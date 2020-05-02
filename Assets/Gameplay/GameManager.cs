using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int BASE_MAX_POP = 20;
    private const int MAX_HEALTH = 1000;

    public static GameManager instance;

    public Slider healthBarSlider;
    public GameObject gameOverPanel;
    [Space(20)]
    public Transform minionHolder;
    public Transform enemyHolder;
    public Transform sacraficeTable;
    public Transform enemySpawnerHolder;
    public Text popCountText;
    public int health;

    [HideInInspector]
    public List<Minion> minions;
    [HideInInspector]
    public List<Enemy> enemies;

    public int MinionCount { get { return minions.Count; } }
    public int EnemyCount { get { return enemies.Count; } }
    public int RemainingPop { get { return maxPopulation - MinionCount; } }

    public int maxPopulation;

    void Awake()
    {
        instance = this;
        health = MAX_HEALTH;
        minionHolder = new GameObject("Minions").transform;
        minionHolder.transform.parent = transform;
        enemyHolder = new GameObject("Enemies").transform;
        enemyHolder.transform.parent = transform;
        enemySpawnerHolder = new GameObject("EnemySpawnerHolder").transform;
        enemySpawnerHolder.transform.parent = transform;
        maxPopulation = BASE_MAX_POP;
    }

    private void Start()
    {
        SpawnInitialBuildings();
    }

    private void Update()
    {
        popCountText.text = MinionCount + "/" + maxPopulation;
        healthBarSlider.value = health / (float)MAX_HEALTH;
        if (health < 0)
        {
            GameOver();
        }
    }

    private void SacrificeMinion()
    {
        if (minions.Count == 0) return;
        int randomIndex = UnityEngine.Random.Range(0, minions.Count);
        Minion minion = minions[randomIndex];
        minions.RemoveAt(randomIndex);
        minion.Sacrifice();
    }

    private void SpawnInitialBuildings()
    {
        Vector2 rand = UnityEngine.Random.insideUnitCircle;
        Vector3 dir = new Vector3(rand.x, 0, rand.y).normalized * 20;
        BuildSystem.instance.SpawnBuilding(dir, 0);
    }

    public void AddMinion(Minion minion)
    {
        minions.Add(minion);
    }

    internal void RemoveMinion(Minion minion)
    {
        if (minions.Contains(minion))
            minions.Remove(minion);
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void SacrificeMinions(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SacrificeMinion();
        }
    }

    public void IncreasePop(int increaseBy)
    {
        maxPopulation += increaseBy;
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
