using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MinionAudio : MonoBehaviour
{
    public AudioClip fightSound;
    public AudioClip[] explosionSounds;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFightSound()
    {
        audioSource.pitch = Random.Range(1.2f, 1.5f);
        audioSource.PlayOneShot(fightSound, 0.3f);
    }

    public void PlayExplosionSound()
    {
        int index = Random.Range(0, explosionSounds.Length);
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(explosionSounds[index], 1f);
    }
}
