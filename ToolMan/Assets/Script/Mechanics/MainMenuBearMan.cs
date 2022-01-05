using UnityEngine;
using System.Collections;

public class MainMenuBearMan : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    public Animator animator;
    public LayerMask GroundMask;

    public int GoToAnotherBearManWeight;
    public int GoToCameight;
    public int PatrolWeight;
    public int IdleWeight;
    public Transform Cam;
    public Transform AnotherBearMan;
    private int[] weight;
    private bool isAction;
    public float MaxActionTime;
    public float MinActionTime;

    private float ActionLastTime;
    private int act;

    public float walkPointRange;
    private bool walkPointSet;

    private Vector3 _dest;
    private Vector3 _lookatDest;


    public float Spd;
    public float RotateSpd;

    protected void Awake()
    {
        weight = new int[] { GoToAnotherBearManWeight, GoToCameight, PatrolWeight, IdleWeight };
        isAction = false;
    }
    protected void FixedUpdate()
    {
        ManageBehavior();
        ManageLookAt();
        ManageMovement();
    }

    protected void ManageBehavior()
    {
        if (isAction && act == 3)
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
        RandomBehavior();
    }

    protected virtual void RandomBehavior()
    {
        //Debug.Log("Attack Mode");
        if (!isAction)
        {
            act = GetRandType(weight);
            if(act == 3)
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
            isAction = true;
        }

        switch (act)
        {
            case 0: // attack
                GoToAnotherBearMan();
                break;
            case 1:
                GoToCam();
                break;
            case 2:
                Patrol();
                break;
            default:
                Idle();
                break;
        }
    }

    private void GoToAnotherBearMan()
    {
        if(Vector3.Distance(transform.position, AnotherBearMan.position) < 0.8f)
        {
            isAction = false;
        }
        SetDest(AnotherBearMan.position);
    }

    private void GoToCam()
    {
        if (Vector3.Distance(transform.position, Cam.position) < 0.8f)
        {
            isAction = false;
        }
        SetDest(Cam.position);
    }

    protected virtual void Patrol()
    {
        if (walkPointSet)
        {
            Vector3 distanceToDest = transform.position - _dest;
            if (distanceToDest.magnitude < 0.8f)
            {
                walkPointSet = false;
                isAction = false;
            }
        }
        if (!walkPointSet) SearchWalkPoint();
    }

    protected virtual void Idle()
    {
        SetDest(transform.position);
    }

    protected virtual void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        var tmp = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        SetDest(tmp);
    }

    protected int GetRandType(int[] weight)
    {
        int total = 0;
        foreach (int w in weight) total += w;
        int rand = UnityEngine.Random.Range(0, total);
        int tmp = 0;
        for (int i = 0; i < weight.Length; i++)
        {
            tmp += weight[i];
            if (rand < tmp) return i;

        }
        return 0;
    }

    protected virtual void ManageMovement()
    {
        if (_dest == Vector3.negativeInfinity)
            return;
        if (_dest == transform.position)
        {
            animator.SetFloat("verticalVelocity", 0);
            return;
        }
        Vector3 nextStep = Vector3.MoveTowards(transform.position, _dest, Spd * Time.deltaTime);
        Vector3 nextStepDir = Vector3.Normalize(nextStep - transform.position);
        RaycastHit m_Hit;
        bool m_HitDetect;
        //transform.rotation = Quaternion.LookRotation(newDirection);
        float moveDis = Vector3.Distance(nextStep, transform.position);
        //m_MaxDistance = moveDis;
        m_HitDetect = Physics.BoxCast(_collider.bounds.center + new Vector3(0, 0.5f, 0), transform.localScale / 3, nextStepDir, out m_Hit, Quaternion.identity, moveDis);
        if (!m_HitDetect)
        {
            transform.position = nextStep;
            animator.SetFloat("verticalVelocity", moveDis * 10);
        }
        else
        {
            Debug.Log("enemy hit " + m_Hit.collider.gameObject.name);
            animator.SetFloat("verticalVelocity", 0);
        }


    }
    protected virtual void ManageLookAt()
    {
        _lookatDest = _dest;
        _lookatDest.y = transform.position.y;
        var targetDirection = _lookatDest - transform.position;
        if (transform.position != _lookatDest)
        {
            var newDir = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * RotateSpd, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
    protected virtual void SetDest(Vector3 dest)
    {
        //dest = new Vector3(dest.x, transform.position.y, dest.z);

        if (Physics.Raycast(dest, -transform.up, 10f, GroundMask))
        {
            walkPointSet = true;
            _dest = dest;
        }
        if (!walkPointSet)
        {
            _dest = transform.position;
        }
    }
}
