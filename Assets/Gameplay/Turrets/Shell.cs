using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed;
    private float damage;
    private string targetTag;

    public void Init(float speed, float damage, string targetTag)
    {
        this.speed = speed;
        this.damage = damage;
        this.targetTag = targetTag;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up.normalized * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == targetTag)
        {
            Attacker attacker = collision.collider.GetComponent<Attacker>();
            if(attacker != null)
            {
                attacker.health -= damage;
            }
        }
        Destroy(gameObject);
    }
}
