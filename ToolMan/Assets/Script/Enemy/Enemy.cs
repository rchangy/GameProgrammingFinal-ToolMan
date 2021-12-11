using UnityEngine;
using UnityEngine.AI;
using ToolMan.Combat;
using System.Collections.Generic;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [SerializeField] protected Transform[] Players;

    [SerializeField] protected LayerMask GroundMask, PlayerMask;

    [SerializeField] protected string Name;

    // random action
    public int AttackWeight;
    public int PatrolWeight;
    public int IdleWeight;
    protected int[] weight;

    [SerializeField]
    protected int[] skillWeight;

    protected float _actionLastTime;
    public float ActionLastTime
    {
        get => _actionLastTime;
        set
        {
            _actionLastTime = value;
        }
    }

    public float MaxActionTime;
    public float MinActionTime;
    protected bool isAction;
    protected int act;
    public float rotateSpeed;


    public SkillCombat combat;

    protected Vector3 closestPlayer
    {
        get
        {
            Transform tmp;
            if((tmp = GetClosestplayer()) == null)
            {
                return _dest;
            }
            else
            {
                return tmp.position;
            }
        }
    }

    protected Vector3 _dest;
    protected Vector3 _lookatDest;

    // Patrol
    protected bool walkPointSet;
    [SerializeField] protected float walkPointRange;


    // chase player
    [SerializeField] protected float SightRange;
    protected bool PlayerInSightRange;
    [SerializeField] protected float speed;
    protected float _currentSpeed;

    // attack
    protected List<string> _skillSet = new List<string>();


    [SerializeField] protected int InitAttackRange;
    protected int _attackRange;
    public int AttackRange
    {
        get => _attackRange;
        set
        {
            _attackRange = value;
        }
    }

    protected bool PlayerInAttackRange;

    // state & attributes (suppressed by what)
    // health bar https://www.youtube.com/watch?v=37hEX3Lrc0A
    protected virtual void Awake()
    {
        weight = new int[] { AttackWeight, PatrolWeight, IdleWeight };
        isAction = false;
        _currentSpeed = speed;
    }
    protected virtual void Start()
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
    }

    protected virtual void Update()
    {
        if (combat.Hp <= 0)
        {
            Die();
        }
        // check sight and attack range
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerMask);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerMask);
    }
    protected virtual void FixedUpdate()
    {
        ManageBehavior();
        ManageLookAt();
        ManageMovement();
        _currentSpeed = speed;
    }

    protected virtual void ManageBehavior()
    {
        if (combat.Attacking) return;
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
        if (!PlayerInSightRange && !PlayerInAttackRange) Patrol();
        if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInSightRange && PlayerInAttackRange) RandomBehavior();
        
    }

    protected virtual void Patrol()
    {
        Debug.Log("Patrol Mode");
        Vector3 distanceToDest = transform.position - _dest;
        if (distanceToDest.magnitude < 1f) walkPointSet = false;
        if (!walkPointSet) SearchWalkPoint();
    }

    protected virtual void Idle()
    {
        Debug.Log("Idle Mode");
        SetDest(transform.position);
    }

    protected virtual void ChasePlayer()
    {
        Debug.Log("Chase Mode");
        SetDest(closestPlayer);
        _currentSpeed *= 2;
    }

    protected virtual void RandomBehavior()
    {
        Debug.Log("Attack Mode");
        if (!isAction)
        {
            act = GetRandType(weight);
            if(act > 0)
            {
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
                isAction = true;
            }
        }

        switch (act){
            case 0: // attack
                Idle();
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

    protected virtual void RandomAttackBehavior()
    {
        if (combat.Attacking) return;
        if (skillWeight == null) return;
        int attackAct = GetRandType(skillWeight);
        //Debug.Log("mode " + _skillSet[attackAct]);
        combat.SetCurrentUsingSkill(_skillSet[attackAct]);
        if (!combat.Attack())
        {
            attackAct = 0;
            combat.SetCurrentUsingSkill(_skillSet[attackAct]);
            combat.Attack();
        }
    }

    protected virtual void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        var tmp = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(tmp, -transform.up, 10f, GroundMask))
        {
            SetDest(tmp);
            walkPointSet = true;
        }
    }

    protected Transform GetClosestplayer()
    {
        Transform target = null;
        float minDist = float.MaxValue;
        foreach(Transform player in Players)
        {
            if (player == null) continue;
            float dist = Vector3.Distance(transform.position, player.position);
            if(dist < minDist)
            {
                target = player;
                minDist = dist;
            }
        }
        return target;
    }


    protected int GetRandType(int[] weight)
    {
        int total = 0;
        foreach (int w in weight) total += w;
        int rand = UnityEngine.Random.Range(0, total);
        int tmp = 0;
        for(int i = 0; i < weight.Length; i++)
        {
            tmp += weight[i];
            if (rand < tmp) return i;

        }
        return 0;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    //protected void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, AttackRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, SightRange);
    //}

    protected void ManageMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _dest, Time.deltaTime * _currentSpeed);
    }

    protected virtual void ManageLookAt()
    {
        if (combat.Attacking) return;
        if (PlayerInSightRange) _lookatDest = new Vector3(closestPlayer.x, transform.position.y, closestPlayer.z);
        else _lookatDest = _dest;
        var targetDirection = _lookatDest - transform.position;
        if(transform.position != _lookatDest)
        {
            var newDir = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateSpeed, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    private void SetDest(Vector3 dest)
    {
        _dest = new Vector3(dest.x, transform.position.y, dest.z);
    }
}
