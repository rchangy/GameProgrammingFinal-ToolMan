using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemySlimeRabbit : Enemy
{
    protected override void Awake()
    {
        base.Awake();
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

    protected override void Idle()
    {
        //SetAllAnimationFalse();
        animator.SetBool("Idle", true);
    }

    protected override void ChasePlayer()
    {
        Debug.Log("Chase Mode");
        //SetAllAnimationFalse();
        animator.SetBool("Move", true);
        //animator.SetTrigger("Move");

        Vector3 p = GetClosestplayer().position;
        GoToPoint(p);
    }

    protected override void Patrol()
    {
        Debug.Log("Patrol Mode");
        //SetAllAnimationFalse();
        animator.SetBool("Move", true);

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            GoToPoint(walkPoint);
        walkPoint = new Vector3(walkPoint.x, transform.position.y, walkPoint.z);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SetAllAnimationFalse()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Move", false);
        animator.SetBool("Damage", false);
        animator.SetBool("Death", false);
    }

    private void GoToPoint(Vector3 point)
    {
        float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle, 0f);

        transform.eulerAngles = direction;
        transform.position += speed * Time.deltaTime * transform.forward;
    }
}