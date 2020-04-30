using System;
using UnityEditor.UI;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public string name;
    public TextMesh nameTag;

    private bool isMoving;
    private Vector3 direction;
    private Vector3 goal;
    
    // Start is called before the first frame update
    void Start()
    {
        goal = RandomGoal();
        isMoving = true;
        nameTag.text = name;
    }

    // Update is called once per frame
    void Update()
    {
        direction = goal - transform.position;
        direction.y = 0f;
        //Debug.DrawRay(transform.position, direction.normalized, Color.red);
        if (Physics.Raycast(transform.position, direction.normalized, 1f)){
            //Debug.Log(name + " - raycast");
            goal = RandomGoal();
        }
        if (direction.magnitude < 0.01f)
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
        //Debug.Log(name + " collided!!!");
        goal = RandomGoal();
    }

    private Vector3 RandomGoal()
    {
        Vector2 vec = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(5, 20);
        Vector3 newGoal = new Vector3(vec.x, 1, vec.y);
        //Debug.DrawLine(new Vector3(transform.position.x, 1, transform.position.z), newGoal, Color.white, 20f);
        return newGoal;
    }
}
