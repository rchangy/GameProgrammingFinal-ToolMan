using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyWhale : Enemy
{
    // ==== Rush ====
    [SerializeField] private float stopRushDistance = 3f;
    [SerializeField] private float rushSpeed = 10f;
    private float tmpSpeed;
    // ==== Rush ====

    // ==== state ====
    [SerializeField] Height height;
    [SerializeField] float highY, middleY, lowY;
    [SerializeField] State state = State.Idle;
    [SerializeField] int hpIntervals = 3;
    [SerializeField] float hpBase;
    [SerializeField] float hpDicreaseThres;
    [SerializeField] float lowTimeSpan;
    private float lowTimeLeft;
    private int nowSardines = 0;
    [SerializeField] int maxSardines;
    // ==== state ====

    // ==== effect ====
    public EffectController effectController;
    // ==== effect ====

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

        SetHeight(Height.High);
        hpBase = combat.HpMaxValue;
        lowTimeLeft = lowTimeSpan;
    }

    protected override void Update()
    {
        GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int playerNum = PlayerGameObjects.Length;

        Players = new Transform[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            Players[i] = PlayerGameObjects[i].transform;
        }

        if (!combat.Attacking)
        {
            if (isAction)
            {
                ActionLastTime -= Time.deltaTime;
                if (ActionLastTime < 0)
                {
                    isAction = false;
                    walkPointSet = false;
                }
                switch (state)
                {
                    case State.Idle:
                        Idle();
                        break;
                    case State.BigSkill:
                        BigSkill();
                        break;
                    case State.Patrol:
                        Patrol();
                        break;
                    case State.Sardine:
                        Sardine();
                        break;
                    case State.Chase:
                        ChasePlayer();
                        break;
                }
            }
            else
            {
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
                isAction = true;
                RandomBehavior();
            }
        }

        // Hp
        if (combat.Hp <= hpBase - combat.HpMaxValue / hpIntervals)
        {
            hpBase -= combat.HpMaxValue / hpIntervals;
            SetHeight(Height.High);
        }

        HeightTransition();
    }

    private void HeightTransition() {
        switch (height)
        {
            case Height.High:
                if (combat.Hp <= hpBase - hpDicreaseThres)
                {
                    SetHeight(Height.Middle);
                    isAction = false;
                }
                break;

            case Height.Middle:
                if (nowSardines >= maxSardines)
                {
                    SetHeight(Height.Low);
                    nowSardines = 0;
                    isAction = false;
                }
                break;

            case Height.Low:
                lowTimeLeft -= Time.deltaTime;
                if (lowTimeLeft <= 0)
                {
                    SetHeight(Height.Middle);
                    lowTimeLeft = lowTimeSpan;
                    isAction = false;
                }
                break;
        }
    }
    public void TakeSardine() { nowSardines++; }

    protected override void RandomBehavior()
    {
        // High states: Idle, BigSkill, Patrol
        // Middle states: Idle, Sardine, Chase
        // Low states: Idle
        switch (height)
        {
            case Height.High:
                act = GetRandType(weight);
                switch (act)
                {
                    case 0: // attack
                        BigSkill();
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
                break;

            case Height.Middle:
                PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerMask);
                if (PlayerInSightRange && AttackWeight > 0)
                    Sardine();
                else
                {
                    int[] w = { PatrolWeight, IdleWeight };
                    act = GetRandType(w);
                    switch (act)
                    {
                        case 0: // attack
                            ChasePlayer();
                            break;
                        case 1:
                            Idle();
                            break;
                        default:
                            break;
                    }
                }
                break;

            case Height.Low:
                Idle();
                break;
        }
    }

    protected override void Idle()
    {
        state = State.Idle;
        animator.SetTrigger("Swim1");
    }
    
    protected override void ChasePlayer()
    {
        //Debug.Log("Chase Mode");
        state = State.Chase;

        Vector3 p = GetClosestplayer().transform.position;
        p = new Vector3(p.x, 0, p.z); // No need to consider y
        Vector3 w = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(p, w);
        if (distance > stopRushDistance)
        {
            animator.SetTrigger("Swim2");
            tmpSpeed = speed;
            speed = rushSpeed;
            GoToPoint(p);
        }
        else
        {
            Idle();
            speed = tmpSpeed;
        }
    }

    protected override void Patrol()
    {
        Debug.Log("Patrol Mode");
        state = State.Patrol;
        animator.SetTrigger("Swim1");

        // Random patrol
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            GoToPoint(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }
    protected override void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
    }

    public void BigSkill() {
        state = State.BigSkill;
        combat.SetCurrentUsingSkill("WhaleBigSkill");
        combat.Attack();
    }

    public void Sardine()
    {
        state = State.Sardine;
        combat.SetCurrentUsingSkill("SardineMissle");
        combat.Attack();
    }

    public Animator GetAnimator() { return animator; }

    private void GoToPoint(Vector3 point)
    {
        float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle + 180, 0f);

        transform.eulerAngles = direction;
        transform.position -= speed * Time.deltaTime * transform.forward;
        //Debug.Log("go to " + point);
    }

    private enum Height
    {
        High,
        Middle,
        Low
    }

    private void SetHeight(Height h)
    {
        switch (h)
        {
            case Height.High:
                height = Height.High;
                transform.position = new Vector3(transform.position.x, highY, transform.position.z);
                break;

            case Height.Middle:
                height = Height.Middle;
                transform.position = new Vector3(transform.position.x, middleY, transform.position.z);
                break;

            case Height.Low:
                height = Height.Low;
                transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
                break;
        }
    }

    private enum State {
        Idle,
        BigSkill,
        Patrol,
        Sardine,
        Chase
    }
}
