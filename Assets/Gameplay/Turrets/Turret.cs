using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Turret : Building
{
    public float attackRange;
    public float rotationSpeed;
    public float attackDeltaTime;

    public float shellSpeed;
    public float shellDamage;
    public string targetTag;
    public float health;

    public AudioClip fireSound;
    public GameObject shellPrefab;
    public Transform head;
    public Transform tubeEnd;

    public Transform targetHolder;
    public Transform target;
    public Transform shellHolder;

    private float lastAttackDeltaTime;
    private AudioSource audioSource;
    private bool placed;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
        shellHolder = new GameObject("shellHolder").transform;
        shellHolder.parent = transform;
        targetHolder = GameManager.instance.enemyHolder;
    }

    // Update is called once per frame
    void Update()
    {
        if (!placed) return;
        if (CheckIfEnemyInRange())
        {
            Vector3 direction = target.position + Vector3.up * 0.5f - head.position;

            float angle = Vector3.Angle(direction, head.forward);
            if (angle <= 2f)
            {
                if (lastAttackDeltaTime >= attackDeltaTime)
                {
                    bool result = Physics.Raycast(tubeEnd.position, tubeEnd.up, out RaycastHit hit, attackRange);
                    //Debug.DrawRay(tubeEnd.position, tubeEnd.up.normalized * attackRange, Color.red);
                    if (!result || result && hit.collider.CompareTag(targetTag))
                    {
                        GunShoot();
                        lastAttackDeltaTime = 0f;
                    }
                }
            }
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            if (head.transform.eulerAngles.x >= 30f)
            {
                Vector3 eulerAngle = head.transform.eulerAngles;
                eulerAngle.x = 30;
                head.transform.eulerAngles = eulerAngle;
            }
        }
        lastAttackDeltaTime += Time.deltaTime;
    }

    private void GunShoot()
    {
        GameObject shellGameObject = Instantiate(shellPrefab, tubeEnd.position, tubeEnd.rotation);
        Shell shell = shellGameObject.GetComponent<Shell>();
        shell.Init(shellSpeed, shellDamage, targetTag);
        shellGameObject.transform.parent = shellHolder;
        audioSource.PlayOneShot(fireSound, 0.4f);
        ScreenShake.instance.SetShakeImpulse(1, 1);
    }

    public bool CheckIfEnemyInRange()
    {
        if (targetHolder != null)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                return true;
            }
            foreach (Transform tr in targetHolder)
            {
                if (tr.CompareTag(targetTag))
                {
                    Vector3 distance = tr.position - transform.position;
                    if (distance.magnitude <= attackRange)
                    {
                        target = tr;
                        return true;
                    }
                }
            }
            target = null;
        }
        return false;
    }

    public override void BuildingPlaced()
    {
        placed = true;
    }
}
