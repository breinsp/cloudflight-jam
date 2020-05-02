using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Attacker attacker;
    public Animator animator;

    public GameObject explosionPrefab;

    void Awake()
    {
        attacker = GetComponent<Attacker>();
        attacker.Init(Attacker_Die, GameManager.instance.minionHolder, "Minion", moveSpeed, rotationSpeed);
        attacker.OnBeginAttack += () => animator.Play("Fighting");
        attacker.OnStopAttack += () => animator.Play("Walking");
    }

    private void Attacker_Die()
    {
        var instance = Instantiate(explosionPrefab);
        instance.transform.position = transform.position;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacker.attackMode && !attacker.attackingAltar)
        {
            Vector3 direction = -transform.position;
            direction.y = 0;
            MoveInDirection(direction);
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
