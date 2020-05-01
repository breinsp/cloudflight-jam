using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float afterSeconds;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if(Time.time - startTime > afterSeconds)
        {
            Destroy(gameObject);
        }    
    }
}
