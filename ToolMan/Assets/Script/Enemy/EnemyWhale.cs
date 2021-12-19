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
    [SerializeField] private float lowTimeLeft;
    [SerializeField] private int nowSardines = 0;
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
        //// get players
        //GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        //int playerNum = PlayerGameObjects.Length;

        //Players = new Transform[playerNum];
        //for (int i = 0; i < playerNum; i++)
        //{
        //    Players[i] = PlayerGameObjects[i].transform;
        //}
        //AttackRange = InitAttackRange;

        //IReadOnlyCollection<string> skillSet = combat.GetCurrentUsingSkillSet();
        //_skillSet = (List<string>)skillSet;
        //if (skillSet != null && skillSet.Count > 0)
        //{
        //    if (skillWeight.Length > skillSet.Count)
        //    {
        //        var tmp = skillWeight;
        //        skillWeight = new int[skillSet.Count];
        //        Array.Copy(tmp, skillWeight, skillSet.Count);
        //    }
        //}
        //else
        //{
        //    skillWeight = null;
        //}
        base.Start();

        SetHeight(Height.Middle);
        hpBase = combat.HpMaxValue;
        lowTimeLeft = lowTimeSpan;
    }

    protected override void FixedUpdate()
    {
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
            //Debug.Log("action time = " + ActionLastTime + " state = " + state + " isAction = " + isAction);
            if (isAction)
            {
                //Debug.Log("isAction = true");
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
                //Debug.Log("isAction = false");
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

        // Move to correct height
        if (Math.Abs(transform.position.y - getHeightValue()) > 0.3f)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, getHeightValue(), transform.position.z), speed * Time.deltaTime);
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
        state = State.Patrol;
        animator.SetTrigger("Swim1");

        // Random patrol
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            GoToPoint(_dest);
        Vector3 distanceToWalkPoint = transform.position - _dest;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }
    protected override void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        _dest = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
    }

    public void BigSkill() {
        state = State.BigSkill;
        combat.SetCurrentUsingSkill("WhaleBigSkill");
        if (!combat.Attacking)
            combat.Attack();
    }

    public void Sardine()
    {
        state = State.Sardine;
        combat.SetCurrentUsingSkill("SardineMissle");
        if (!combat.Attacking)
            combat.Attack();
    }
    public void TakeSardine() { nowSardines++; }

    public Animator GetAnimator() { return animator; }

    private void GoToPoint(Vector3 point)
    {
        if (transform.position != point)
        {
            Vector3 towardDir = transform.position - point;
            towardDir = new Vector3(towardDir.x, 0, towardDir.z); // Ignore y axis
            Vector3 newDir = Vector3.RotateTowards(transform.forward, towardDir, Time.deltaTime * rotateSpeed, 0f);
            
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        transform.position -= speed * Time.deltaTime * transform.forward;
    }

    private enum Height
    {
        High,
        Middle,
        Low
    }
    private float getHeightValue()
    {
        switch (height)
        {
            case Height.High:
                return highY;
            case Height.Middle:
                return middleY;
            case Height.Low:
                return lowY;
            default:
                return -1;
        }
    }

    private void SetHeight(Height h)
    {
        Debug.Log("set height " + h);
        switch (h)
        {
            case Height.High:
                height = Height.High;
                //transform.position = new Vector3(transform.position.x, highY, transform.position.z);
                break;

            case Height.Middle:
                height = Height.Middle;
                nowSardines = 0;
                //transform.position = new Vector3(transform.position.x, middleY, transform.position.z);
                break;

            case Height.Low:
                height = Height.Low;
                //transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
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
