using System;
using UnityEngine;

[RequireComponent(typeof(MinionAnimation))]
public class Minion : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Attacker attacker;

    private Vector3 goal;

    private bool isSacrificed;
    private MinionAnimation minionAnimation;

    // Start is called before the first frame update
    void Start()
    {
        goal = RandomGoal();
        minionAnimation = GetComponent<MinionAnimation>();
        minionAnimation.SetState(MinionState.normal);
        attacker = GetComponent<Attacker>();
        attacker.Init(Attacker_Die, GameManager.instance.enemyHolder, "Enemy", moveSpeed, rotationSpeed);

    }

    private void Attacker_Die()
    {
        StartExplosion();
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
            StartExplosion();
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
        Vector2 vec = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(20f, 40f);
        Vector3 newGoal = new Vector3(vec.x, 0, vec.y);
        return newGoal;
    }

    public void Sacrifice()
    {
        if (isSacrificed) return;
        minionAnimation.SetState(MinionState.sacrificing);
        goal = Vector3.zero;
        isSacrificed = true;
        moveSpeed *= 3;
    }

    public void StartExplosion()
    {
        Debug.Log("Boom!");
        Destroy(gameObject);
    }
}
