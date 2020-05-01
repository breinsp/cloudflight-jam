using System;
using UnityEditor.UI;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    private Vector3 direction;
    private Vector3 goal;

    // Start is called before the first frame update
    void Start()
    {
        goal = RandomGoal();
    }

    // Update is called once per frame
    void Update()
    {
        direction = goal - transform.position;
        direction.y = 0f;
        //Debug.DrawRay(transform.position, direction.normalized, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, direction.normalized, 1f))
        {
            //Debug.Log(name + " - raycast");
            goal = RandomGoal();
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        goal = RandomGoal();
    }

    private Vector3 RandomGoal()
    {
        Vector2 vec = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(10f, 40f);
        Vector3 newGoal = new Vector3(vec.x, 0, vec.y);
        //Debug.DrawLine(new Vector3(transform.position.x, 1, transform.position.z), newGoal, Color.white, 20f);
        return newGoal;
    }
}
