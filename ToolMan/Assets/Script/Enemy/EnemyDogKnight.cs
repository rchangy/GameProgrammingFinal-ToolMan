using UnityEngine;
using System.Collections;
using System;

public class EnemyDogKnight : Enemy
{
    // skills
    public Transform IaiAttackPoint;
    public Explosion ChargingVfx;
    public Explosion SwordTrailVfx;
    public Explosion IaiSparkVfx;
    public Explosion NotifyVfx;

    public int LongRangeAttackWeight;
    public int LongRangePatrolWeight;
    public int LongRangeChaseWeight;
    public int LongRangeIdleWeight;
    private int[] longRangeWeight;

    private bool isLongRangeAction = false;

    [SerializeField] private int _longRangeSkillStartIdx;

    private int[] _meleeSkillWeight;
    private int[] _longRangeSKillWeight;

    protected override void Awake()
    {
        base.Awake();
        longRangeWeight = new int[] { LongRangeAttackWeight, LongRangePatrolWeight, LongRangeIdleWeight, LongRangeChaseWeight };
    }

    protected override void Start()
    {
        base.Start();
        _meleeSkillWeight = new int[_longRangeSkillStartIdx];
        Array.Copy(skillWeight, 0, _meleeSkillWeight, 0, Mathf.Min(_longRangeSkillStartIdx, weight.Length));


        if (weight.Length > _longRangeSkillStartIdx)
        {
            _longRangeSKillWeight = new int[skillWeight.Length - _longRangeSkillStartIdx];
            Array.Copy(skillWeight, _longRangeSkillStartIdx, _longRangeSKillWeight, 0, skillWeight.Length - _longRangeSkillStartIdx);
        }
        else
        {
            _longRangeSKillWeight = new int[]{0};
        }

    }

    protected override void ManageBehavior()
    {
        if (combat.Attacking) return;
        if (isAction || isLongRangeAction)
        {
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime <= 0)
            {
                isAction = false;
                isLongRangeAction = false;
                walkPointSet = false;
            }
            else
            {
                if (isAction)
                    RandomBehavior();
                else if (isLongRangeAction)
                    LongRangeRandomBehavior();
                return;
            }
        }

        if (!PlayerInSightRange && !PlayerInAttackRange) Patrol();
        if (PlayerInSightRange && !PlayerInAttackRange) LongRangeRandomBehavior();
        if (PlayerInSightRange && PlayerInAttackRange) RandomBehavior();

    }

    protected override void ChasePlayer()
    {
        Debug.Log("chase");
        base.ChasePlayer();
        SetAllAnimationFalse();
        animator.SetBool("Run", true);
    }

    protected override void Idle()
    {
        Debug.Log("idle");
        base.Idle();
        SetAllAnimationFalse();
    }

    protected override void SetAllAnimationFalse()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Defend", false);
    }

    protected override void Patrol()
    {
        Debug.Log("patrol");
        base.Patrol();
        SetAllAnimationFalse();
        animator.SetBool("Walk", true);
    }

    private void LongRangeRandomBehavior()
    {
        Debug.Log("long range");
        if (!isAction && !isLongRangeAction)
        {
            act = GetRandType(longRangeWeight);
            if (act > 0)
            {
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
            }
            isLongRangeAction = true;
        }
        if (isLongRangeAction)
        {
            switch (act)
            {
                case 0: // long range attack
                    Idle();
                    SetAllAnimationFalse();
                    RandomAttackBehavior();
                    break;
                case 1:
                    Patrol();
                    break;
                case 2:
                    Idle();
                    break;
                default:
                    ChasePlayer();
                    break;
            }
        }
    }


    protected override void RandomBehavior()
    {
        Debug.Log("close range");
        if (!isAction && !isLongRangeAction)
        {
            act = GetRandType(weight);
            if (act > 0)
            {
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
            }
            isAction = true;
        }
        if (isAction)
        {
            switch (act)
            {
                case 0: // attack
                    Idle();
                    SetAllAnimationFalse();
                    RandomAttackBehavior();
                    break;
                case 1:
                    Patrol();
                    break;
                case 2:
                    Idle();
                    break;
                default:
                    ChasePlayer();
                    break;
            }
        }
    }

    protected override void RandomAttackBehavior()
    {
        if (combat.Attacking) return;
        if (skillWeight == null || skillWeight.Length == 0) return;
        int attackAct = -1;
        if (isAction)
        {
            attackAct = GetRandType(_meleeSkillWeight);
        }
        else if (isLongRangeAction)
        {
            attackAct = GetRandType(_longRangeSKillWeight) + _longRangeSkillStartIdx;
        }
        if(attackAct != -1)
        {
            combat.SetCurrentUsingSkill(_skillSet[attackAct]);
            combat.Attack();
            if (!combat.Attack())
            {
                if (isAction)
                {
                    attackAct = 0;
                    combat.SetCurrentUsingSkill(_skillSet[attackAct]);
                    combat.Attack();
                }
            }
        }
    }
    protected override void Die()
    {
        animator.SetTrigger("Die");
        base.Die();
    }
}
