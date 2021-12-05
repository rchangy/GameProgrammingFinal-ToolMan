using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyChicken : Enemy
{
    // ==== Rush ====
    [SerializeField] private bool rushing = false;
    [SerializeField] private float stopRushDistance = 3f;
    [SerializeField] private float rushSpped = 10f;
    private float tmpSpeed;
    // ==== Rush ====

    protected override void Awake()
    {
        base.Awake();
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

        //var targetDirection = closestPlayer.position - transform.position;
        //float singleStep = rotateSpeed * Time.deltaTime;
        //var newDir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0f);

        //transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected override void Idle()
    {
        SetAllAnimationFalse(); // Idle aanimation
        if (UnityEngine.Random.Range(0, 1) > 0.7f) // TurnHead animation
            animator.SetBool("TurnHead", true);
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

        SetAllAnimationFalse();
        animator.SetBool("Walk", true);

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            GoToPoint(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 0.1f) walkPointSet = false;
    }

    private void Rush() {
        Debug.Log("Rush Mode");
        rushing = true;

        Vector3 p = GetClosestplayer().transform.position;
        p = new Vector3(p.x, 0, p.z); // No need to consider y
        Vector3 w = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(p, w);

        SetAllAnimationFalse();
        if (distance > stopRushDistance)
        {
            animator.SetBool("Run", true);
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

    private void SetAllAnimationFalse() {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Eat", false);
        animator.SetBool("TurnHead", false);
    }

    private void GoToPoint(Vector3 point)
    {
        float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle, 0f);

        transform.eulerAngles = direction;
        transform.position += speed * Time.deltaTime * transform.forward;
    }
}