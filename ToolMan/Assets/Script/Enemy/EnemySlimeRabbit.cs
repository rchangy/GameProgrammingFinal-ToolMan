using UnityEngine;
using System;
using System.Collections.Generic;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Combat;

public class EnemySlimeRabbit : Enemy
{
    // ==== Actions ====
    // Follows other enemies
    // Escapes when player approaches
    // ==== Actions ====

    [SerializeField] private float escapeRange;
    [SerializeField] private float _buffRange;

    // ==== Follow other enemies ====
    [SerializeField] private Enemy followTarget = null;
    //[SerializeField] Vector3 followOffset; 
    [SerializeField] private bool targetInSightRange = false;
    [SerializeField] private bool targetInAttackRange = false;
    // ==== Follow other enemies ====

    [SerializeField] private State state = State.Idle;

    [SerializeField] private List<ScriptableBuff> _buffs;


    protected override void Start()
    {
        base.Start();

        // Get enemy to follow
        SetFollowTarget();
        //if (followTarget != null)
        //    transform.position = followTarget.transform.position + followOffset;
    }

    private void SetFollowTarget() {
        if (followTarget != null)
            return;

        // set closest one
        float distance = float.MaxValue;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies)
        {
            if (e.GetComponent<EnemySlimeRabbit>() != null)
                continue;

            Vector3 ePos = new Vector3(e.transform.position.x, transform.position.y, e.transform.position.z);
            float d = Vector3.Distance(ePos, transform.position);
            if (d <= distance)
            {
                followTarget = e.GetComponent<Enemy>();
                distance = d;
            }
        }

        if (distance == float.MaxValue)
            followTarget = null;
    }

    protected override void Update()
    {
        base.Update();

        // If no follow target, set a new one
        if (followTarget == null)
            SetFollowTarget();

        // check sight and attack range
        targetInSightRange = EnemyInRange(SightRange);
        targetInAttackRange = EnemyInRange(AttackRange);
        
        AddBuffToOthers();
    }

    protected override void ManageBehavior()
    {
        if (isAction)
        {
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime <= 0)
            {
                isAction = false;
                walkPointSet = false;
            }
            RandomBehavior();
        }
        else
        {
            ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
            isAction = true;

            if (combat.Attacking) Idle();
            else
            {
                if (PlayerInSightRange) Escape();
                else if (targetInAttackRange) RandomBehavior();
                else if (targetInSightRange) Follow();
                else
                {
                    if (followTarget == null)
                        Patrol();
                    else
                        Follow();
                }
            }
        }
    }

    private bool EnemyInRange(float range) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies)
        {
            if (e.GetComponent<EnemySlimeRabbit>() != null)
                continue;

            Vector3 ePos = new Vector3(e.transform.position.x, transform.position.y, e.transform.position.z);
            if (Vector3.Distance(ePos, transform.position) <= range)
                return true;
        }
        return false;
    }

    protected override void RandomBehavior()
    {
        // When player in attack range
        //Debug.Log("Random Mode");
        if (!isAction)
        {
            act = GetRandType(weight);
            ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
            isAction = true;
        }

        switch (act)
        {
            case 0:
                Patrol();
                break;
            case 1:
                Patrol();
                break;
            case 2:
                Idle();
                break;
            default:
                Follow();
                break;
        }
    }

    protected override void Idle()
    {
        //state = State.Idle;
        SetAllAnimationFalse();
        if (!combat.Attacking)
        {
            animator.SetBool("Idle", true);
        }
    }

    private void Escape()
    {
        combat.Attack();
        state = State.Escape;
        //Debug.Log("Escape Mode");
        
        Vector3 p = GetClosestplayer().position;
        EscapeFromPoint(p);
    }

    private void Follow()
    {
        state = State.Follow;

        if (followTarget != null)
        {
            _dest = followTarget.transform.position;
        }
        else
            Patrol();
    }


    //private void GoToPoint(Vector3 point)
    //{
    //    SetAllAnimationFalse();
    //    animator.SetBool("Move", true);

    //    float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
    //    Vector3 direction = new Vector3(0f, angle, 0f);

    //    transform.eulerAngles = direction;
    //    transform.position += speed * Time.deltaTime * transform.forward;
    //    //Debug.Log("mode go to " + point);
    //}

    private void EscapeFromPoint(Vector3 point)
    {
        SetAllAnimationFalse();
        animator.SetBool("Move", true);

        _dest = transform.position - (point - transform.position);
        //float angle = Mathf.Atan2(transform.position.x - point.x, transform.position.z - point.z) * Mathf.Rad2Deg;
        //Vector3 direction = new Vector3(0f, angle, 0f);

        //transform.eulerAngles = direction;
        //transform.position += speed * Time.deltaTime * transform.forward;
        ////Debug.Log("mode escape from " + point);
    }

    private enum State
    {
        Idle,
        Escape,
        Follow,
        RandomPatrol,
        Attack
    }

    private void AddBuffToOthers()
    {
        if (_buffs == null) return;
        if (_buffs.Count == 0) return;
        Debug.Log("Add Buff Check");
        Collider[] hitTargets = Physics.OverlapSphere(gameObject.transform.position, _buffRange, gameObject.layer) ;
        foreach (Collider target in hitTargets)
        {
            CombatUnit targetCombat = target.GetComponent<CombatUnit>();
            if (targetCombat != null)
            {
                // how to add buff
                // random or all
                targetCombat.AddBuff(_buffs[0]);
                Debug.Log("add buff to " + target.name);
            }
        }
    }
    private void SetAllAnimationFalse()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("TurnHead", false);
    }
    protected override void ManageLookAt()
    {
        _lookatDest = _dest;
        var targetDirection = _lookatDest - transform.position;
        if (transform.position != _lookatDest)
        {
            var newDir = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateSpeed, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}