using UnityEngine;
using System;
using System.Collections.Generic;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Combat;

public class EnemySlimeRabbit1 : Enemy
{
    // ==== Actions ====
    // Escapes when player approaches
    // ==== Actions ====

    [SerializeField] private bool _escaping = false;
    [SerializeField] private State state; // For debugging
    [SerializeField] protected bool PlayerInEscapeRange;
    [SerializeField] protected float EscapeRange;
    private bool _destAvailable = false;
    [SerializeReference] private EffectController effectController;

    private GameObject _target;
    Vector3 nextStep;

    RaycastHit m_Hit;
    float m_MaxDistance;
    bool m_HitDetect;
    float moveDis;

    protected override void Start()
    {
        base.Start();
        state = State.Idle;
    }

    protected override void Update()
    {
        _destAvailable = false;
        base.Update();
        PlayerInEscapeRange = Physics.CheckSphere(transform.position, EscapeRange, PlayerMask);
    }

    protected override void ManageBehavior()
    {
        if (!PlayerInAttackRange && PlayerInEscapeRange && !_escaping)
        {
            isAction = false;
            Escape();
            return;
        }
        if (_escaping)
        {
            if (!PlayerInEscapeRange || PlayerInAttackRange) _escaping = false;
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
        else
        {
            if (!PlayerInSightRange) ChasePlayer();
            else if (PlayerInAttackRange) RandomBehavior();
            else if (!PlayerInEscapeRange && PlayerInSightRange) Attack();
        }
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
        }
    }

    private void Attack()
    {
        state = State.Attack;
        SetAllAnimationFalse();
        _destAvailable = false;
        if (PlayerInSightRange && !PlayerInAttackRange)
        {
            _target = GetRealClosestplayer().gameObject;
            combat.SetCurrentUsingSkill("SlimeRabbitShoot");
            combat.Attack();
        }
        else if (PlayerInAttackRange)
        {
            Debug.Log("normal attack");
            // normal attack
            combat.SetCurrentUsingSkill("SlimeRabbitNormal");
            combat.Attack();
        }
    }

    protected override void Idle()
    {
        state = State.Idle;
        base.Idle();
        SetAllAnimationFalse();
        _destAvailable = false;
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
    protected override void SetAllAnimationFalse()
    {
        animator.SetBool("Walk", false);
    }

    protected override void ManageLookAt()
    {
        if (!_destAvailable)
            return;

        _lookatDest = _dest;
        var targetDirection = _lookatDest - transform.position;

        var newDir = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateSpeed, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected override void SetDest(Vector3 dest)
    {
        //Debug.Log("set dest = " + (dest-transform.position));
        //_dest = new Vector3(dest.x, transform.position.y, dest.z);
        _destAvailable = true;
        if (true)
        {
            //Debug.Log("set dest success = " + (dest - transform.position));

            walkPointSet = true;
            _dest = dest;
        }
        if (!walkPointSet)
        {
            _dest = transform.position;
        }
    }

    public EffectController GetEffectController() { return effectController; }
    public GameObject GetTarget()
    {
        return _target;
    }
    protected override void ManageMovement()
    {
        if (_dest == Vector3.negativeInfinity)
        {
            return;
        }
        //_dest.y = transform.position.y;
        //transform.position = Vector3.MoveTowards(transform.position, _dest, Time.deltaTime * _currentSpeed);
        //return;

        // Determine which direction to rotate towards
        //Vector3 targetDirection = _dest - transform.position;
        // The step size is equal to speed times frame time.
        //float singleStep = speed * Time.deltaTime;
        // Rotate the forward vector towards the target direction by one step
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        nextStep = Vector3.MoveTowards(transform.position, _dest, speed * combat.Spd * Time.deltaTime);
        Vector3 nextStepDir = Vector3.Normalize(nextStep - transform.position);

        //transform.rotation = Quaternion.LookRotation(newDirection);
        moveDis = Vector3.Distance(nextStep, transform.position);
        m_MaxDistance = moveDis;
        m_HitDetect = Physics.BoxCast(enemyCollider.bounds.center, transform.localScale/3, nextStepDir, out m_Hit, Quaternion.identity, moveDis);
        if (!m_HitDetect || m_Hit.collider.gameObject == this || m_Hit.collider.gameObject.tag == "Player" || m_Hit.collider.isTrigger)
        {
            transform.position = nextStep;
        }
        //else
        //    Debug.Log("will hit: " + m_Hit.collider.gameObject.name);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(enemyCollider.bounds.center, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(enemyCollider.bounds.center + transform.forward * m_Hit.distance, transform.localScale/3);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(enemyCollider.bounds.center, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(enemyCollider.bounds.center + transform.forward * m_MaxDistance, transform.localScale/3);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_dest, _dest + Vector3.up * 10f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(nextStep, nextStep + Vector3.up * 10f);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EscapeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}