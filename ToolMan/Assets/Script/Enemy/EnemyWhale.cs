using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyWhale : Enemy
{
    // ==== Rush ====
    [SerializeField] private bool rushing = false;
    [SerializeField] private float stopRushDistance = 3f;
    [SerializeField] private float rushSpped = 10f;
    [SerializeField] private float speed = 30;
    private float tmpSpeed;
    // ==== Rush ====

    // ==== Patrol ====
    [SerializeField] private bool onPatrolOrbit = false;
    [SerializeField] private bool patrolStarted = false;
    [SerializeField] private Transform patrolCenter;
    [SerializeField] private float patrolRadius;
    [SerializeField] private Vector3 patrolAxis;
    private Vector3 patrolStartPoint;
    private float patrolAngle = 0;
    [SerializeField] private float patrolAngularVelocity = 30;
    // ==== Patrol ====

    protected override void Awake()
    {
        base.Awake();
        patrolStartPoint = patrolCenter.position + Vector3.forward * patrolRadius;
        transform.position = new Vector3(transform.position.x, patrolCenter.position.y, transform.position.z);
        tmpSpeed = speed;
    }

    protected override void Start()
    {
        // get players
        GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int playerNum = PlayerGameObjects.Length;

        Players = new Transform[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            Players[i] = PlayerGameObjects[i].transform;
        }
        AttackRange = InitAttackRange;

        IReadOnlyCollection<string> skillSet = combat.GetCurrentUsingSkillSet();
        _skillSet = (List<string>)skillSet;
        if (skillSet != null && skillSet.Count > 0)
        {
            if (skillWeight.Length > skillSet.Count)
            {
                var tmp = skillWeight;
                skillWeight = new int[skillSet.Count];
                Array.Copy(tmp, skillWeight, skillSet.Count);
            }
        }
        else
        {
            skillWeight = null;
        }
    }

    protected override void RandomBehavior()
    {
        // When player in attack range
        Debug.Log("Random Mode");
        if (!isAction)
        {
            act = GetRandType(weight);
            ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
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
                    Debug.Log("Attack Mode;)");
                    break;
                case 1:
                    Patrol();
                    break;
                case 2:
                    Idle();
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

    protected override void Idle()
    {
        animator.SetTrigger("Swim1");
    }

    protected override void ChasePlayer()
    {
        Debug.Log("Chase Mode");
        // compare two players position and chase the closest
        Rush();
   }

    protected override void Patrol()
    {
        Debug.Log("Patrol Mode");
        animator.SetTrigger("Swim1");

        onPatrolOrbit = Vector3.Distance(transform.position, patrolCenter.position) <= 0.5f + patrolRadius;
        if (!onPatrolOrbit)
            patrolStarted = false;
        

        if (!patrolStarted)
        {
            patrolAngle = 0;
            GoToPoint(patrolStartPoint);
            patrolStarted = Vector3.Distance(transform.position, patrolStartPoint) <= 0.5f;
            float dir = Mathf.Sign(-transform.forward.x * patrolCenter.position.y - transform.right.z * patrolCenter.position.x);
            patrolAxis = Vector3.up;
            if (dir > 0)
                patrolAxis = Vector3.down;
        }
        else
        {
            transform.RotateAround(patrolCenter.position, patrolAxis, patrolAngularVelocity*Time.deltaTime);
            
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
            tmpSpeed = speed;
            speed = rushSpped;
            GoToPoint(p);
        }
        else
        {
            rushing = false;
            speed = tmpSpeed;
        }
    }

    private void GoToPoint(Vector3 point)
    {
        float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
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
