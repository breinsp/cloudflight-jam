using UnityEngine;

public class Attacker : MonoBehaviour
{
    public float attackRange;
    public float health;
    public float damagePerHit;
    public float attackDeltaTime;

    private float movingSpeed;
    private float rotationSpeed;
    private float lastAttackDeltaTime;

    private Transform target;
    private Transform targetHolder;
    private string targetTag;

    public bool attackMode = false;

    public delegate void DieEventHandler();
    public event DieEventHandler OnDeath;

    public void Init(DieEventHandler die, Transform targetHolder, string targetTag, float movingSpeed, float rotationSpeed)
    {
        this.OnDeath += die;
        this.targetHolder = targetHolder;
        this.targetTag = targetTag;
        this.movingSpeed = movingSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            OnDeath();
            return;
        }
        attackMode = CheckIfEnemyInRange();
        if (attackMode)
        {
            MoveToTarget();
        }
        lastAttackDeltaTime += Time.deltaTime;
    }

    public void MoveToTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        if (direction.magnitude >= 1.2f)
        {
            MoveInDirection(direction);
        }
        else if (lastAttackDeltaTime >= attackDeltaTime)
        {
            float angleBetween = Vector3.Angle(direction, transform.forward);
            if (angleBetween <= 5f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
            Attack();
        }
    }

    private void Attack()
    {
        Attacker attacker = target.GetComponent<Attacker>();
        attacker.health -= damagePerHit;
        lastAttackDeltaTime = 0;
    }

    void MoveInDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * movingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public bool CheckIfEnemyInRange()
    {
        if (targetHolder != null)
        {
            foreach (Transform tr in targetHolder)
            {
                if (tr.tag == targetTag)
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
}
