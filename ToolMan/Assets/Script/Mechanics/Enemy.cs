using UnityEngine;
using UnityEngine.AI;
using ToolMan.Combat;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent EnemyAgent;

    [SerializeField] protected Transform[] Players;

    [SerializeField] protected LayerMask GroundMask, PlayerMask;

    [SerializeField] protected string Name;

    // random action
    public int AttackWeight;
    public int PatrolWeight;
    public int IdleWeight;
    protected int[] weight;

    private float _actionLastTime;
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
    private bool isAction;
    private int act;
    public float rotateSpeed;


    public SkillCombat combat;

    private Transform closestPlayer
    {
        get => GetClosestplayer();
    }

    // Patrol
    protected Vector3 walkPoint;
    protected bool walkPointSet;
    [SerializeField] protected float walkPointRange;


    // chase player
    [SerializeField] protected float SightRange;
    protected bool PlayerInSightRange;


    // attack
    [SerializeField] protected float InitAttackInterval;
    private float _attackInterval;
    public float AttackInterval
    {
        get => _attackInterval;
        set
        {
            _attackInterval = Mathf.Max(0, value);
        }
    }

    [SerializeField] protected int InitAttackRange;
    private int _attackRange;
    public int AttackRange
    {
        get => _attackRange;
        set
        {
            _attackRange = value;
        }
    }

    protected bool AlreadyAttacked;
    protected bool PlayerInAttackRange;

    // state & attributes (suppressed by what)
    // health bar https://www.youtube.com/watch?v=37hEX3Lrc0A
    private void Awake()
    {
        weight = new int[] { AttackWeight, PatrolWeight, IdleWeight };
        isAction = false;
    }
    protected virtual void Start()
    {
        EnemyAgent = GetComponent<NavMeshAgent>();

        // get players
        GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int playerNum = PlayerGameObjects.Length;

        Players = new Transform[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            Players[i] = PlayerGameObjects[i].transform;
        }

        AttackInterval = InitAttackInterval;
        AttackRange = InitAttackRange;
    }

    protected virtual void Update()
    {
        GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int playerNum = PlayerGameObjects.Length;

        Players = new Transform[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            Players[i] = PlayerGameObjects[i].transform;
        }
        // check sight and attack range
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerMask);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerMask);
        if (isAction)
        {
            Attack();
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime < 0)
            {
                isAction = false;
                walkPointSet = false;
            }
        }
        else
        {
            if (combat.Attacking) Idle();
            else
            {
                if (!PlayerInSightRange && !PlayerInAttackRange) Patrol();
                if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
                if (PlayerInSightRange && PlayerInAttackRange) Attack();
            }

        }
    }

    protected virtual void Patrol()
    {
        Debug.Log("Patrol Mode");
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            EnemyAgent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    protected virtual void Idle()
    {
        Debug.Log("Idle Mode");
        EnemyAgent.SetDestination(transform.position);
    }

    protected virtual void ChasePlayer()
    {
        
        Debug.Log("Chase Mode");
        // compare two players position and chase the closest
        EnemyAgent.SetDestination(closestPlayer.position);
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack Mode");
        if (!isAction)
        {
            act = GetRandType(weight);
            ActionLastTime = Random.Range(MinActionTime, MaxActionTime);
            isAction = true;
            Debug.Log(act);
        }

        switch (act){
            case 0: // attack
                combat.Attack();
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

        var targetDirection = closestPlayer.position - transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;
        var newDir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0f);

        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected virtual void ResetAttack()
    {
        AlreadyAttacked = false;
    }

    public virtual void BeAttacked()
    {

    }

    protected void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundMask)) walkPointSet = true;
    }

    protected Transform GetClosestplayer()
    {
        Transform target = null;
        float minDist = float.MaxValue;
        foreach(Transform player in Players)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if(dist < minDist)
            {
                target = player;
                minDist = dist;
            }
        }
        return target;
    }

    protected virtual void Die()
    {
        Debug.LogFormat("[{0}] Enemy Dies", Name);
    }

    private int GetRandType(int[] weight)
    {
        int total = 0;
        foreach (int w in weight) total += w;
        int rand = Random.Range(0, total);
        int tmp = 0;
        for(int i = 0; i < weight.Length; i++)
        {
            tmp += weight[i];
            if (rand < tmp) return i;

        }
        return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}
