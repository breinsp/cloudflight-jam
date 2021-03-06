﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MinionAnimation))]
public class Minion : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Attacker attacker;
    public GameObject explosionEffect;
    public MinionAudio minionAudio;

    private Vector3 goal;

    private bool isSacrificed;
    private bool isPraising;
    private MinionAnimation minionAnimation;
    private float waitSeconds;

    // Start is called before the first frame update
    void Start()
    {
        minionAnimation = GetComponent<MinionAnimation>();
        minionAnimation.SetState(MinionState.normal);
        attacker = GetComponent<Attacker>();
        attacker.Init(Attacker_Die, GameManager.instance.enemyHolder, "Enemy", moveSpeed, rotationSpeed);
        attacker.OnBeginAttack += BeginAttack;
        attacker.OnStopAttack += StopAttack;
        goal = RandomGoal();
    }

    private void Attacker_Die()
    {
        GameManager.instance.RemoveMinion(this);
        StartExplosion(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacker.attackMode)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, (goal - transform.position).normalized, 1f))
            {
                Collide();
            }

            if (isSacrificed)
            {
                HandleSacrifice();
            }
            else
            {
                HandleIdle();
            }
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void HandleIdle()
    {
        if (waitSeconds > 0)
        {
            waitSeconds -= Time.deltaTime;
            return;
        }
        else
        {
            minionAnimation.SetState(MinionState.normal);
        }

        Vector3 direction = goal - transform.position;
        direction.y = 0f;
        if (direction.magnitude < 0.1f)
        {
            goal = RandomGoal();
        }
        else
        {
            MoveInDirection(direction);
        }
    }

    void HandleSacrifice()
    {
        Vector3 direction = goal - transform.position;
        if (direction.magnitude < 3f)
        {
            StartExplosion(2);
        }
        else
        {
            MoveInDirection(direction);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collide();
    }

    private void Collide()
    {
        if (!isSacrificed)
        {
            goal = RandomGoal();
        }
    }

    private Vector3 RandomGoal()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.2f)
        {
            //wait
            waitSeconds = UnityEngine.Random.Range(1f, 4f);
            minionAnimation.SetState(MinionState.wait);
            return transform.position;
        }
        else
        {
            Vector2 vec = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(20f, 40f);
            Vector3 newGoal = new Vector3(vec.x, 0, vec.y);
            return newGoal;
        }
    }

    public void Sacrifice()
    {
        if (isSacrificed) return;
        minionAnimation.SetState(MinionState.sacrificing);
        goal = Vector3.zero;
        isSacrificed = true;
        moveSpeed *= 2;
    }

    public void StartExplosion(float duration)
    {
        if (isPraising) return;
        isPraising = true;
        minionAnimation.SetState(MinionState.praising);
        StartCoroutine(HandleExplosion(duration));
    }

    public IEnumerator HandleExplosion(float duration)
    {
        float time = duration;
        float start = Time.time;
        float delta = 0;

        float minSize = 1;
        float maxSize = 1.3f;

        while (delta < time)
        {
            delta = Time.time - start;
            float lerp = delta / time;

            float sizeLerp = Mathf.Clamp01(lerp - 0.9f) / (1f - 0.9f);
            sizeLerp = Mathf.Pow(sizeLerp, 4f);
            transform.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, sizeLerp);

            yield return null;
        }
        Explode();
        yield return null;
    }

    public void Explode()
    {
        minionAudio.PlayExplosionSound();
        var instance = Instantiate(explosionEffect);
        instance.transform.position = transform.position;
        var destroyAfter = gameObject.AddComponent<DestroyAfter>();
        destroyAfter.afterSeconds = 1f;
        ScreenShake.instance.SetShakeImpulse(10f, 1f);
        minionAnimation.meshRenderer.enabled = false;
    }

    public void BeginAttack()
    {
        minionAnimation.SetState(MinionState.fighting);
    }

    public void StopAttack()
    {
        minionAnimation.SetState(MinionState.normal);
    }
}
