using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent EnemyAgent;

    [SerializeField] protected Transform[] Players;

    [SerializeField] protected LayerMask GroundMask, PlayerMask;

    [SerializeField] protected string Name;

    //[SerializeField] protected HealthBar healthBar;

    //[SerializeField] protected int TotalHP;
    //private int _hp;
    //public int HP
    //{
    //    get => _hp;
    //    set
    //    {
    //        _hp = Mathf.Max(0, value);
    //        healthBar.SetHealth(_hp);
    //        if (_hp == 0) Die();
    //    }
    //}

    public CombatUnit combat;

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

        // initialize attributes
        //healthBar.SetMaxHealth(TotalHP);
        //HP = TotalHP;
        AttackInterval = InitAttackInterval;
        AttackRange = InitAttackRange;
    }

    protected virtual void Update()
    {
        // check sight and attack range
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerMask);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerMask);

        if (!PlayerInSightRange && !PlayerInAttackRange) Patrol();
        if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInSightRange && PlayerInAttackRange) Attack();
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
        EnemyAgent.SetDestination(transform.position);
        transform.LookAt(closestPlayer);
        combat.Attack();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}
