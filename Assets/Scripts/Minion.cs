using System;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    private Vector3 direction;
    private Vector3 goal;

    private bool isSacrificed;

    // Start is called before the first frame update
    void Start()
    {
        goal = RandomGoal();
    }

    // Update is called once per frame
    void Update()
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

    void HandleIdle()
    {
        direction = goal - transform.position;
        direction.y = 0f;
        if (direction.magnitude < 0.1f)
        {
            goal = RandomGoal();
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void HandleSacrifice()
    {
        direction = goal - transform.position;
        if (direction.magnitude < 3f)
        {
            StartExplosion();
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
