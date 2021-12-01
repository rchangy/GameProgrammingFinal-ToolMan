using UnityEngine;

public class EnemyWhale : Enemy
{
    // ==== Rush ====
    [SerializeField] private bool rushing = false;
    [SerializeField] private float stopRushDistance = 3f;
    [SerializeField] private float rushSpped = 10f;
    private float speed;
    // ==== Rush ====

    private Vector3 circularPatrolCenter = Vector3.zero;
    private float patrolAngle = 0;
    [SerializeField] float circularPatrolRadius = 10f;

    private void Awake()
    {
        speed = EnemyAgent.speed;
    }

    protected override void RandomBehavior()
    {
        // When player in attack range
        Debug.Log("Attack Mode");
        if (!isAction)
        {
            act = GetRandType(weight);
            ActionLastTime = Random.Range(MinActionTime, MaxActionTime);
            isAction = true;
        }

        if (rushing)
            Rush();

        else
        {
            switch (act)
            {
                case 0: // attack
                    RandomAttackBehavior();
                    break;
                case 1:
                    Patrol();
                    break;
                case 2:
                    Rush();
                    break;
                default:
                    break;
            }
        }

        var targetDirection = closestPlayer.position - transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;
        var newDir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0f);

        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected override void Patrol()
    {
        // Circular patrol route
        Debug.Log("Patrol Mode");
        animator.SetTrigger("Swim1");
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            EnemyAgent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void Rush() {
        Debug.Log("Rush Mode");
        rushing = true;

        Vector3 p = GetClosestplayer().transform.position;
        p = new Vector3(p.x, 0, p.z); // No need to consider y
        Vector3 w = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(p, w);
        if (distance > stopRushDistance)
        {
            animator.SetTrigger("Swim2");
            EnemyAgent.SetDestination(new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z));
            EnemyAgent.speed = rushSpped;
        }
        else
        {
            rushing = false;
            EnemyAgent.speed = speed;
        }
    }

    private void BigSkill() { // big skill
        circularPatrolCenter = closestPlayer.transform.position;

        patrolAngle += Time.deltaTime;
        float x = circularPatrolCenter.x + circularPatrolRadius * Mathf.Cos(patrolAngle);
        float z = circularPatrolCenter.z + circularPatrolRadius * Mathf.Cos(patrolAngle);
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
