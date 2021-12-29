using UnityEngine;

public class EnemyTurtle : Enemy
{
    // ==== Actions ====
    // Follows other enemies
    // Escapes when player approaches
    // ==== Actions ====

    // ==== Follow other enemies ====
    [SerializeField] private Enemy followTarget = null;
    //[SerializeField] Vector3 followOffset; 
    [SerializeField] private bool targetInSightRange = false;
    //[SerializeField] private bool targetInAttackRange = false;
    // ==== Follow other enemies ====

    [SerializeField] private bool _escaping = false;

    [SerializeField] private State state; // For debugging

    protected override void Start()
    {
        base.Start();

        // Get enemy to follow
        SetFollowTarget();
        state = State.Idle;
    }

    private void SetFollowTarget() {
        if (followTarget != null && followTarget.gameObject.activeSelf)
            return;

        // set closest one
        float distance = float.MaxValue;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies)
        {
            if (e.GetComponent<EnemyTurtle>() != null || !e.activeSelf)
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
        if (followTarget == null || !followTarget.gameObject.activeSelf)
        {
            SetFollowTarget();
            //Debug.Log("chicken died");
        }

        base.Update();


        // check sight and attack range
        //targetInSightRange = EnemyInRange(SightRange);
        targetInSightRange = EnemyInRange(SightRange);
        
        //AddBuffToOthers();
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
            if (!PlayerInAttackRange) _escaping = false;
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

        if (!targetInSightRange && followTarget != null) {
            isAction = true;
            ActionLastTime = Random.Range(MinActionTime, MaxActionTime);
            Follow();
        }
        else RandomBehavior();     
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
            ActionLastTime = Random.Range(MinActionTime, MaxActionTime);
            isAction = true;
        }

        switch (act)
        {
            case 0:
                Attack();
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

    private void Attack() {
        state = State.Attack;
        SetAllAnimationFalse();
        SetDest(transform.position);
        combat.Attack();
    }

    protected override void Idle()
    {
        state = State.Idle;
        base.Idle();
        SetAllAnimationFalse();
        if (!combat.Attacking)
        {
            //animator.SetBool("Idle", true);
        }
        SetDest(transform.position);
    }

    private void Escape()
    {
        state = State.Escape;
        Vector3 p = GetClosestplayer().position;
        SetAllAnimationFalse();
        animator.SetBool("Walk", true);
        //Debug.Log("desired dest = " + (-p + transform.position));
        SetDest(transform.position - (p - transform.position));
        //Debug.Log("now dest = " + (_dest-transform.position));
        //EscapeFromPoint(p);
        _escaping = true;
    }

    private void Follow()
    {
        state = State.Follow;
        SetAllAnimationFalse();
        animator.SetBool("Walk", true);
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
        state = State.RandomPatrol;
        base.Patrol();
        SetAllAnimationFalse();
        animator.SetBool("Walk", true);
    }

    private enum State
    {
        Idle,
        Escape,
        Follow,
        RandomPatrol,
        Attack
    }

    //private void AddBuffToOthers()
    //{
    //    if (_buffs == null) return;
    //    if (_buffs.Count == 0) return;
    //    Collider[] hitTargets = Physics.OverlapSphere(gameObject.transform.position, _buffRange, 1 << LayerMask.NameToLayer("Enemy"));
    //    foreach (Collider target in hitTargets)
    //    {
            
    //        CombatUnit targetCombat = target.gameObject.GetComponent<CombatUnit>();
                
    //        if (targetCombat != null)
    //        {
    //            // how to add buff
    //            // random or all
    //            targetCombat.AddBuff(_buffs[0]);
    //        }
    //    }
    //}
    protected override void SetAllAnimationFalse()
    {
        animator.SetBool("Walk", false);
        //animator.SetBool("Move", false);

    }

    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    Debug.lo
    //}

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

    public Animator GetAnimator() { return animator; }

    protected override void SetDest(Vector3 dest)
    {
        //Debug.Log("set dest = " + (dest-transform.position));
        //_dest = new Vector3(dest.x, transform.position.y, dest.z);
        if (true)
        {
            //Debug.Log("set dest success = " + (dest-transform.position));

            walkPointSet = true;
            _dest = dest;
        }
        if (!walkPointSet)
        {
            _dest = transform.position;
        }
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    //protected void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(_dest, _dest-10*Vector3.up);
    //}
}