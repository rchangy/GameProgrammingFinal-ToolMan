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
    [SerializeField] Vector3 followOffset; 
    [SerializeField] private bool targetInSightRange = false;
    [SerializeField] private bool targetInAttackRange = false;
    // ==== Follow other enemies ====

    [SerializeField] private State state = State.Idle;

    [SerializeField] private List<ScriptableBuff> _buffs;

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

        // Get enemy to follow
        SetFollowTarget();
        if (followTarget != null)
            transform.position = followTarget.transform.position + followOffset;
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
        if (combat.Hp <= 0)
        {
            Die();
        }
        GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int playerNum = PlayerGameObjects.Length;

        Players = new Transform[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            Players[i] = PlayerGameObjects[i].transform;
        }

        // If no follow target, set a new one
        if (followTarget == null)
            SetFollowTarget();

        // check sight and attack range
        PlayerInSightRange = Physics.CheckSphere(transform.position, escapeRange, PlayerMask);
        targetInSightRange = EnemyInRange(SightRange);
        targetInAttackRange = EnemyInRange(AttackRange);
        
        if (isAction)
        {
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime < 0)
            {
                isAction = false;
                walkPointSet = false;
            }
            else
            {
                switch (state)
                {
                    case State.Idle:
                        Idle();
                        break;
                    case State.Escape:
                        Escape();
                        break;
                    case State.Follow:
                        Follow();
                        break;
                    case State.RandomPatrol:
                        RandomPatrol();
                        break;
                    case State.Attack:
                        RandomAttackBehavior();
                        break;
                }
            }
        }
        else
        {
            ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
            isAction = true;

            if (combat.Attacking) Idle();
            else
            {                
                if (PlayerInSightRange) Escape();
                else if (targetInAttackRange) RandomAttackBehavior();
                else if (targetInSightRange) Follow();
                else
                {
                    if (followTarget == null)
                        RandomPatrol();
                    else
                        Follow();
                }
            }
        }
        AddBuffToOthers();
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
            case 0: // attack
                RandomAttackBehavior();
                Debug.Log("Attack Mode;)");
                break;
            case 1:
                RandomPatrol();
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
        state = State.Idle;
        animator.SetBool("Move", false);
        animator.SetBool("Death", false);
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
            Vector3 tPos = new Vector3(followTarget.transform.position.x, transform.position.y, followTarget.transform.position.z);
            float d = Vector3.Distance(tPos, transform.position);
            if (d > followOffset.magnitude*1.2f)
                GoToPoint(followTarget.transform.position + followOffset);
            else
            {
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
                Idle();
            }
        }
        else
            RandomPatrol();
    }

    private void RandomPatrol()
    {
        state = State.RandomPatrol;
        //Debug.Log("Random Patrol Mode");
        
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            GoToPoint(walkPoint);
        walkPoint = new Vector3(walkPoint.x, transform.position.y, walkPoint.z);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void GoToPoint(Vector3 point)
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Damage", false);
        animator.SetBool("Death", false);
        animator.SetBool("Move", true);

        float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle, 0f);

        transform.eulerAngles = direction;
        transform.position += speed * Time.deltaTime * transform.forward;
        //Debug.Log("mode go to " + point);
    }

    private void EscapeFromPoint(Vector3 point)
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Damage", false);
        animator.SetBool("Death", false);
        animator.SetBool("Move", true);

        float angle = Mathf.Atan2(transform.position.x - point.x, transform.position.z - point.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle, 0f);

        transform.eulerAngles = direction;
        transform.position += speed * Time.deltaTime * transform.forward;
        //Debug.Log("mode escape from " + point);
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

}