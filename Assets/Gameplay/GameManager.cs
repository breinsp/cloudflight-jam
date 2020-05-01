using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform minionHolder;

    [HideInInspector]
    public List<Minion> minions;

    public int MinionCount { get { return minions.Count; } }

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        minionHolder = new GameObject("Minions").transform;
        minionHolder.transform.parent = transform;
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

    public void PlayAudio(AudioClip clip, float volume, float pitchMin, float pitchMax)
    {
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip, volume);
    }
}
