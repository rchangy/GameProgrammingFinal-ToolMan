using UnityEngine;
using System;
using ToolMan.Combat.Equip;
public class EnemyWhale : Enemy
{
    // ==== Rush ====
    [SerializeField] private float stopRushDistance = 3f;
    [SerializeField] private float rushSpeed = 10f;
    private float tmpSpeed;
    // ==== Rush ====

    // === CirclePath ===
    [SerializeField] private Transform[] pathNodes;
    private int _currentNodeIdx;
    // === CirclePath ===


    // ==== state ====
    private float _initY;

    [SerializeField] Height height;
    [SerializeField] float highY, middleY, lowY;
    [SerializeField] State state = State.Idle;
    [SerializeField] int hpIntervals = 3;
    [SerializeField] float hpBase;
    [SerializeField] float hpDicreaseThres;
    [SerializeField] float lowTimeSpan;
    private float lowTimeLeft;
    [SerializeField] private int nowSardines = 0;
    [SerializeField] int maxSardines;

    [SerializeField] Vector3 LowestPoint = Vector3.zero;

    // ==== state ====

    // ==== effect ====
    public EffectController effectController;
    // ==== effect ====

    // ==== skill ====
    public GameObject bigSkillPrefab;
    public Flood flood;

    public Transform[] SharkPos;
    [SerializeField] private int[] SharkNumEachWave;
    private int _currentSharkWave;

    private float _timerForSkills;
    [SerializeField] private float _timeToStartSharkWave;
    private bool _sharkReleased;

    // ==== skill ====

    protected override void Awake()
    {
        base.Awake();
        tmpSpeed = speed;
    }

    protected override void Start()
    {
        base.Start();
        _initY = transform.position.y;
        SetHeight(Height.Middle);

        hpBase = combat.HpMaxValue;
        lowTimeLeft = lowTimeSpan;
        SetDest(pathNodes[_currentNodeIdx].position);
        GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int playerNum = PlayerGameObjects.Length;

        Players = new Transform[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            Players[i] = PlayerGameObjects[i].transform;
        }
    }


    protected override void Update()
    {
        if (_timerForSkills > 0)
            _timerForSkills -= Time.deltaTime;
        if (isAction)
        {
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime < 0)
            {
                isAction = false;
            }
            switch (state)
            {
                case State.Idle:
                    Idle();
                    break;
                case State.Patrol:
                    Patrol();
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
        HeightTransition();

    }

    protected override void FixedUpdate()
    {
        ManageBehavior();
        ManageLookAt();
        ManageMovement();
    }

    private void HeightTransition() {
        switch (height)
        {
            case Height.High:
                if (_sharkReleased && !combat.Attacking)
                {
                    SetHeight(Height.Middle);
                    isAction = false;
                }
                break;

            case Height.Middle:
                if (nowSardines >= maxSardines)
                {
                    SetHeight(Height.Low);
                    isAction = false;
                }
                break;

            case Height.Low:
                lowTimeLeft -= Time.deltaTime;
                if (lowTimeLeft <= 0)
                {
                    SetHeight(Height.High);
                    lowTimeLeft = lowTimeSpan;
                    isAction = false;
                }
                break;
        }
    }

    private float getHeightValue()
    {
        switch (height)
        {
            case Height.High:
                return highY + _initY;
            case Height.Middle:
                return middleY + _initY;
            case Height.Low:
                return lowY + _initY;
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
                _timerForSkills = _timeToStartSharkWave;
                _sharkReleased = false;
                break;

            case Height.Middle:
                nowSardines = 0;
                height = Height.Middle;
                //flood.StopFlooding();
                break;

            case Height.Low:
                height = Height.Low;
                break;
        }
    }

    protected override void RandomBehavior()
    {
        switch (height)
        {
            case Height.High:
                act = GetRandType(weight);
                if(_timerForSkills <= 0 && !_sharkReleased)
                {
                    ReleaseSharks();
                }
                switch (act)
                {
                    case 0: // attack
                        BigSkill();
                        break;
                    default:
                        Patrol();
                        break;
                }
                break;

            case Height.Middle:
                Sardine();
                Patrol();
                //else
                //{
                //    int[] w = { PatrolWeight, IdleWeight };
                //    act = GetRandType(w);
                //    switch (act)
                //    {
                //        case 0: // attack
                //            ChasePlayer();
                //            break;
                //        case 1:
                //            Idle();
                //            break;
                //        default:
                //            break;
                //    }
                //}
                break;

            case Height.Low:
                SetDest(LowestPoint);
                break;
        }
    }

    protected override void Idle()
    {
        state = State.Idle;
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
            animator.SetBool("Swim2", true);
            tmpSpeed = speed;
            speed = rushSpeed;
            SetDest(p);
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
        SetDest(pathNodes[_currentNodeIdx].position);
        Vector3 distanceToWalkPoint = transform.position - _dest;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            _currentNodeIdx++;
            if (_currentNodeIdx == pathNodes.Length) _currentNodeIdx = 0;
        }
    }


    public void BigSkill() {
        combat.SetCurrentUsingSkill("WhaleBigSkill");
        if (!combat.Attacking)
            combat.Attack();
    }

    private void ReleaseSharks()
    {
        flood.StartFlooding();
        combat.SetCurrentUsingSkill("Shark Missle");
        _sharkReleased = combat.Attack();
    }

    public int GetCurrentSharkWave()
    {
        int ret = SharkNumEachWave[_currentSharkWave];
        _currentSharkWave++;
        if (_currentSharkWave == SharkNumEachWave.Length) _currentSharkWave = 0;
        return ret;
    }

    public void Sardine()
    {
        combat.SetCurrentUsingSkill("SardineMissle");
        if (!combat.Attacking)
            combat.Attack();
    }

    public void TakeSardine() { nowSardines++; }
    public Animator GetAnimator() { return animator; }
    
    protected override void SetDest(Vector3 dest)
    {
        _dest = new Vector3(dest.x, transform.position.y, dest.z);
    }

    protected override void ManageLookAt()
    {
        _lookatDest = _dest;
        if (Vector3.Distance(transform.position, _lookatDest) >= 0.3f)
        {
            var targetDirection = _lookatDest - transform.position;
            targetDirection.x = -targetDirection.x;
            targetDirection.z = -targetDirection.z;

            var newDir = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateSpeed, 0f);

            transform.rotation = Quaternion.LookRotation(newDir);

        }
    }

    protected override void ManageMovement()
    {
        if (_dest == Vector3.negativeInfinity)
            return;

        Vector3 nextStep = Vector3.MoveTowards(transform.position, _dest, speed * combat.Spd * Time.deltaTime);
        // Move to correct height
        if (Math.Abs(transform.position.y - getHeightValue()) > 0.3f)
            nextStep = Vector3.MoveTowards(nextStep, new Vector3(nextStep.x, getHeightValue(), nextStep.z), speed * combat.Spd * Time.deltaTime);
        Vector3 nextStepDir = Vector3.Normalize(nextStep - transform.position);
        RaycastHit m_Hit;
        bool m_HitDetect;
        float moveDis = Vector3.Distance(nextStep, transform.position);
        m_HitDetect = Physics.BoxCast(GetComponent<Collider>().bounds.center, transform.localScale, nextStepDir, out m_Hit, Quaternion.identity, moveDis);
        if (!m_HitDetect || m_Hit.collider.gameObject == this || m_Hit.collider.isTrigger)
            transform.position = nextStep;
    }

    protected override void SetAllAnimationFalse()
    {
        Anim.SetBool("Swim2", false);
    }

    private enum Height
    {
        High,
        Middle,
        Low
    }
    private enum State
    {
        Idle,
        BigSkill,
        Patrol,
        Sardine,
        Chase
    }
}