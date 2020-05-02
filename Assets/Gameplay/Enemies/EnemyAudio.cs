using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    public AudioClip ramSound;
    public AudioClip[] stepSounds;

    private AudioSource audioSource;
    private AudioSource audioSource3d;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource3d = gameObject.AddComponent<AudioSource>();
        audioSource3d.spatialBlend = 1;
        audioSource3d.dopplerLevel = 0;
        audioSource3d.minDistance = 1;
        audioSource3d.maxDistance = 30;
    }

    public void PlayRamSound()
    {
        audioSource.PlayOneShot(ramSound, 0.5f);
    }

    public void PlayStepSound()
    {
        int index = Random.Range(0, stepSounds.Length);
        audioSource3d.pitch = Random.Range(0.8f, 1.2f);
        audioSource3d.PlayOneShot(stepSounds[index], 1f);
    }
}
