using UnityEngine;

public class EnemyWhale : Enemy
{
    // ==== Rush ====
    [SerializeField] private bool rushing = false;
    [SerializeField] private float stopRushDistance = 3f;
    [SerializeField] private float rushSpped = 10f;
    [SerializeField] private float speed = 30;
    // ==== Rush ====

    // ==== Patrol ====
    [SerializeField] private bool onPatrolOrbit = false;
    [SerializeField] private bool patrolStarted = false;
    [SerializeField] private Transform patrolCenter;
    [SerializeField] private float patrolRadius;
    private Vector3 patrolStartPoint;
    private float patrolAngle = 0;
    [SerializeField] private float patrolAngularVelocity = 30;
    // ==== Patrol ====

    private void Awake()
    {
        patrolStartPoint = patrolCenter.position + Vector3.forward * patrolRadius;
        transform.position = new Vector3(transform.position.x, patrolCenter.position.y, transform.position.z);
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
        animator.SetTrigger("Swim1");

        onPatrolOrbit = Vector3.Distance(transform.position, patrolCenter.position) <= 0.5f + patrolRadius;
        if (!onPatrolOrbit)
            patrolStarted = false;
        

        if (!patrolStarted)
        {
            patrolAngle = 0;
            GoToPatrolStartPoint();
            //Debug.Log("dis = " + Vector3.Distance(transform.position, patrolStartPoint));
            patrolStarted = Vector3.Distance(transform.position, patrolStartPoint) <= 0.5f;
        }
        else
        {
            transform.RotateAround(patrolCenter.position, Vector3.up, patrolAngularVelocity*Time.deltaTime);
        }
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
            //EnemyAgent.SetDestination(new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z));
            //EnemyAgent.speed = rushSpped;
        }
        else
        {
            rushing = false;
            //EnemyAgent.speed = speed;
        }
    }

    private void GoToPatrolStartPoint()
    {
        float angle = Mathf.Atan2(patrolStartPoint.x - transform.position.x, patrolStartPoint.z - transform.position.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle+180, 0f);
   
        transform.eulerAngles = direction;
        transform.position -= speed * Time.deltaTime * transform.forward;
    }

    private void BigSkill() { // big skill
        //circularPatrolCenter = closestPlayer.transform.position;

        //patrolAngle += Time.deltaTime;
        //float x = circularPatrolCenter.x + circularPatrolRadius * Mathf.Cos(patrolAngle);
        //float z = circularPatrolCenter.z + circularPatrolRadius * Mathf.Cos(patrolAngle);
        //transform.position = new Vector3(x, transform.position.y, z);
    }
}
