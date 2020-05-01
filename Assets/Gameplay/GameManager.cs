using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Button sacrificeButton;
    public Transform minionHolder;

    [HideInInspector]
    public List<Minion> minions;

    public int MinionCount { get { return minions.Count; } }

    void Awake()
    {
        instance = this;
        minionHolder = new GameObject("Minions").transform;
        minionHolder.transform.parent = transform;
        sacrificeButton.onClick.AddListener(SacrificeMinion);
    }

    private void SacrificeMinion()
    {
        if (minions.Count == 0) return;
        int randomIndex = Random.Range(0, minions.Count);
        Minion minion = minions[randomIndex];
        minions.RemoveAt(randomIndex);
        minion.Sacrifice();
    }

    public void AddMinion(Minion minion)
    {
        minions.Add(minion);
    }

    public void SacrificeMinions(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SacrificeMinion();
        }
    }
}
