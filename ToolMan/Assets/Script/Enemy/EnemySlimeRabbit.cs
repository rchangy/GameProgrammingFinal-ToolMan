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

    //[SerializeField] private float _escapeRange;
    [SerializeField] private float _buffRange;

    // ==== Follow other enemies ====
    [SerializeField] private Enemy followTarget = null;
    //[SerializeField] Vector3 followOffset; 
    //[SerializeField] private bool targetInSightRange = false;
    [SerializeField] private bool targetInAttackRange = false;
    // ==== Follow other enemies ====

    //[SerializeField] private State state = State.Idle;

    [SerializeField] private List<ScriptableBuff> _buffs;


    private bool _escaping = false;

    protected override void Start()
    {
        base.Start();

        // Get enemy to follow
        SetFollowTarget();
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

            float d = Vector3.Distance(e.transform.position, transform.position);
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
        // If no follow target, set a new one
        if (followTarget == null)
            SetFollowTarget();

        base.Update();


        // check sight and attack range
        //targetInSightRange = EnemyInRange(SightRange);
        targetInAttackRange = EnemyInRange(AttackRange);
        
        AddBuffToOthers();
    }

    protected override void ManageBehavior()
    {
        if (combat.Attacking) return;
        if (PlayerInAttackRange && !_escaping)
        {
            isAction = false;
            Escape();
            return;
        }
        if (_escaping)
        {
            if (!PlayerInSightRange) _escaping = false;
            else
            {
                Escape();
                return;
            }
        }
        if (isAction)
        {
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime <= 0)
            {
                isAction = false;
                walkPointSet = false;
            }
            else
            {
                RandomBehavior();
                return;
            }
        }

        if (targetInAttackRange) RandomBehavior();
        else Follow();
        
    }

    private bool EnemyInRange(float range) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies)
        {
            if (e.GetComponent<EnemySlimeRabbit>() != null)
                continue;

            //Vector3 ePos = new Vector3(e.transform.position.x, transform.position.y, e.transform.position.z);
            if (Vector3.Distance(e.transform.position, transform.position) <= range)
                return true;
        }
        return false;
    }

    protected override void RandomBehavior()
    {
        // When player in attack range
        if (!isAction)
        {
            if (_escaping) act = 3;
            else
            {
                act = GetRandType(weight);
            }
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
            case 3:
                Escape();
                break;
            default:
                Follow();
                break;
        }
    }

    protected override void Idle()
    {
        base.Idle();
        SetAllAnimationFalse();
        if (!combat.Attacking)
        {
            animator.SetBool("Idle", true);
        }
    }

    private void Escape()
    {
        combat.Attack();
        //state = State.Escape;
        Vector3 p = GetClosestplayer().position;
        SetAllAnimationFalse();
        animator.SetBool("Move", true);
        SetDest(transform.position - (p - transform.position));
        //EscapeFromPoint(p);
        _escaping = true;
    }

    private void Follow()
    {
        //state = State.Follow;
        SetAllAnimationFalse();
        animator.SetBool("Move", true);
        if (followTarget != null)
        {
            SetDest(followTarget.transform.position);
            //_dest = new Vector3(followTarget.transform.position.x, transform.position.y, followTarget.transform.position.z);
        }
        else
            Patrol();
    }
    protected override void Patrol()
    {
        base.Patrol();
        SetAllAnimationFalse();
        animator.SetBool("Move", true);
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
        Collider[] hitTargets = Physics.OverlapSphere(gameObject.transform.position, _buffRange, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider target in hitTargets)
        {
            
            CombatUnit targetCombat = target.gameObject.GetComponent<CombatUnit>();
                
            if (targetCombat != null)
            {
                // how to add buff
                // random or all
                targetCombat.AddBuff(_buffs[0]);
            }
        }
    }
    protected override void SetAllAnimationFalse()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Move", false);

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