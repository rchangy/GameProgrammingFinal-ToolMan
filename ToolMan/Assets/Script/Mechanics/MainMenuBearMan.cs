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
    public int SpinWeight;
    public Transform Cam;
    public Transform AnotherBearMan;
    private int[] weight;
    private bool isAction;
    public float MaxActionTime;
    public float MinActionTime;
    public float SocialDistance;
    public float LookCamTime;

    private float ActionLastTime;
    private int act;
    public string Action;

    public float walkPointRange;
    private bool walkPointSet;

    private Vector3 _dest;
    private Vector3 _lookatDest;
    private Vector3 _camPosition;
    private float _currentLookCamTime;
    private bool isSpinning = false;


    public float Spd;
    public float RotateSpd;

    protected void Awake()
    {
        weight = new int[] { GoToAnotherBearManWeight, GoToCameight, PatrolWeight, SpinWeight, IdleWeight };
        isAction = false;
        _camPosition = transform.position;
    }
    protected void FixedUpdate()
    {
        ManageBehavior();
        ManageLookAt();
        ManageMovement();
    }

    protected void ManageBehavior()
    {
        if (isAction && (act == 3 || act == 4))
        {
            ActionLastTime -= Time.deltaTime;
            if (ActionLastTime <= 0)
            {
                isAction = false;
                walkPointSet = false;
                isSpinning = false;
                animator.SetBool("isSpinning", false);
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
            _currentLookCamTime = 0f;
            if(act == 3 || act == 4)
                ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
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
            case 3:
                Spin();
                break;
            default:
                Idle();
                break;
        }
    }

    private void GoToAnotherBearMan()
    {
        if (!isAction)
        {
            SetDest(AnotherBearMan.position);
            isAction = true;
            Action = "GoToAnotherBearMan";
        }
        

        if (Vector3.Distance(transform.position, _dest) < SocialDistance)
        {
            isAction = false;
        }
        else
        {
            SetDest(AnotherBearMan.position);
        }
    }

    private void GoToCam()
    {
        if (!isAction)
        {
            SetDest(_camPosition);
            isAction = true;
            Action = "GoToCam";
        }

        if (Vector3.Distance(transform.position, _dest) < 0.8f)
        {
            SetDest(transform.position);
            if (_currentLookCamTime >= LookCamTime)
                isAction = false;
            else
            {
                _currentLookCamTime += Time.deltaTime;
            }
        }
        else
            SetDest(_camPosition);
    }

    protected virtual void Patrol()
    {
        if (!isAction)
        {
            isAction = true;
            Action = "Patrol";
        }

        if (walkPointSet)
        {
            Vector3 distanceToDest = transform.position - _dest;
            if (Vector3.Distance(transform.position, _dest) < 0.8f)
            {
                walkPointSet = false;
                isAction = false;
            }
        }
        if (!walkPointSet) SearchWalkPoint();
    }

    protected virtual void Idle()
    {
        isAction = true;
        Action = "Idle";
        SetDest(transform.position);
    }

    protected virtual void Spin()
    {
        isAction = true;
        Action = "Spin";
        SetDest(transform.position);
        if (isSpinning)
            return;
        isSpinning = true;
        animator.SetTrigger("startSpinning");
        animator.SetBool("isSpinning", true);
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
            //Debug.Log("hit " + m_Hit.collider.gameObject.name);
            animator.SetFloat("verticalVelocity", 0);
            SetDest(transform.position);
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
