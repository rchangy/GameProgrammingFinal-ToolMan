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

    // ==== Patrol ====
    //[SerializeField] private bool onPatrolOrbit = false;
    //[SerializeField] private bool patrolStarted = false;
    //[SerializeField] private Transform patrolCenter;
    //[SerializeField] private float patrolRadius;
    //[SerializeField] private Vector3 patrolAxis;
    //private Vector3 patrolStartPoint;
    //private float patrolAngle = 0;
    //[SerializeField] private float patrolAngularVelocity = 30;
    // ==== Patrol ====

    // ==== state ====
    [SerializeField] Height height = Height.High;
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
        //patrolStartPoint = patrolCenter.position + Vector3.forward * patrolRadius;
        //transform.position = new Vector3(transform.position.x, patrolCenter.position.y, transform.position.z);
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

        transform.position = new Vector3(transform.position.x, highY, transform.position.z);
        hpBase = combat.HpMaxValue;
        lowTimeLeft = lowTimeSpan;

        // effect
        //Effect bigSkillWarning = effectController.effectList.Find(e => e.name == "WhaleBigSkillWarning");
        //Effect bigSkillEffect = effectController.effectList.Find(e => e.name == "WhaleBigSkillEffect");
        //bigSkillWarning.transform.position = transform.position + Vector3.down * 10;
        //bigSkillEffect.transform.position = transform.position + Vector3.down * (highY + 2);

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

        //// check sight and attack range
        //PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerMask);
        //PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerMask);

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
            height = Height.High;
            transform.position = new Vector3(transform.position.x, highY, transform.position.z);
        }

        HeightTransition();
    }

    private void HeightTransition() {
        switch (height)
        {
            case Height.High:
                if (combat.Hp <= hpBase - hpDicreaseThres)
                {
                    height = Height.Middle;
                    isAction = false;
                    transform.position = new Vector3(transform.position.x, middleY, transform.position.z);
                }
                break;

            case Height.Middle:
                if (nowSardines >= maxSardines)
                {
                    height = Height.Low;
                    nowSardines = 0;
                    isAction = false;
                    transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
                }
                break;

            case Height.Low:
                lowTimeLeft -= Time.deltaTime;
                if (lowTimeLeft <= 0)
                {
                    height = Height.Middle;
                    lowTimeLeft = lowTimeSpan;
                    isAction = false;
                    transform.position = new Vector3(transform.position.x, middleY, transform.position.z);
                }
                break;
        }
    }

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
                act = GetRandType(weight);
                switch (act)
                {
                    case 0: // attack
                        Sardine();
                        break;
                    case 1:
                        ChasePlayer();
                        break;
                    case 2:
                        Idle();
                        break;
                    default:
                        break;
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
        animator.SetTrigger("Swim1");

        // Random patrol
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            GoToPoint(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;

        //onPatrolOrbit = Vector3.Distance(transform.position, patrolCenter.position) <= 0.5f + patrolRadius;
        //if (!onPatrolOrbit)
        //    patrolStarted = false;


        //if (!patrolStarted)
        //{
        //    patrolAngle = 0;
        //    GoToPoint(patrolStartPoint);
        //    patrolStarted = Vector3.Distance(transform.position, patrolStartPoint) <= 0.5f;
        //    float dir = Mathf.Sign(-transform.forward.x * patrolCenter.position.y - transform.right.z * patrolCenter.position.x);
        //    patrolAxis = Vector3.up;
        //    if (dir > 0)
        //        patrolAxis = Vector3.down;
        //}
        //else
        //{
        //    transform.RotateAround(patrolCenter.position, patrolAxis, patrolAngularVelocity * Time.deltaTime);

        //}
    }

    public void BigSkill() {
        state = State.BigSkill;
        combat.SetCurrentUsingSkill("WhaleBigSkill");
        combat.Attack();
        Debug.Log("combat aaaaa " + combat.currentUsingSkillName);
    }

    public void Sardine()
    {
        state = State.Sardine;
    }

    public Animator GetAnimator() { return animator; }

    private void GoToPoint(Vector3 point)
    {
        float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(0f, angle + 180, 0f);

        transform.eulerAngles = direction;
        transform.position -= speed * Time.deltaTime * transform.forward;
    }

    private enum Height
    {
        High,
        Middle,
        Low
    }

    private enum State {
        Idle,
        BigSkill,
        Patrol,
        Sardine,
        Chase
    }
}
