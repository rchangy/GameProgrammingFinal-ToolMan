using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EnemyBigChicken : Enemy
{
    [SerializeField] private float _crazyTime;
    //private float _lastingCrazyTime;
    //private bool _inCrazyMode = false;

    [SerializeField] private float _speedUp;

    // ==== Rush ====
    //[SerializeField] private bool rushing = false;
    //[SerializeField] private float rushSpped = 10f;
    //private float tmpSpeed;
    // ==== Rush ====

    //protected override void Awake()
    //{
    //    base.Awake();
    //    tmpSpeed = speed;
    //}

    //protected override void Start()
    //{
    //    // get players
    //    GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
    //    int playerNum = PlayerGameObjects.Length;

    //    Players = new Transform[playerNum];
    //    for (int i = 0; i < playerNum; i++)
    //    {
    //        Players[i] = PlayerGameObjects[i].transform;
    //    }
    //    AttackRange = InitAttackRange;

    //    IReadOnlyCollection<string> skillSet = combat.GetCurrentUsingSkillSet();
    //    _skillSet = (List<string>)skillSet;
    //    if (skillSet != null && skillSet.Count > 0)
    //    {
    //        if (skillWeight.Length > skillSet.Count)
    //        {
    //            var tmp = skillWeight;
    //            skillWeight = new int[skillSet.Count];
    //            Array.Copy(tmp, skillWeight, skillSet.Count);
    //        }
    //    }
    //    else
    //    {
    //        skillWeight = null;
    //    }
    //}

    //protected override void Update()
    //{
    //    //Debug.Log("in Update");
    //    //Debug.Log("PlayerInSightRange: " + PlayerInSightRange);
    //    //Debug.Log("PlayerInAttackRange: " + PlayerInAttackRange);
    //    //Debug.Log("mode: isAction = " + isAction);
    //    ////Debug.Log("mode: Action Last time = " + ActionLastTime);
    //    base.Update();
    //}

    //protected override void RandomBehavior()
    //{
    //    // When player in attack range
    //    Debug.Log("Random Mode");
    //    if (!isAction)
    //    {
    //        act = GetRandType(weight);
    //        //Debug.Log("mode act = " + act + ", rushing = " + rushing);
    //        //Debug.Log("skillWeight = " + skillWeight[0] + ", " + skillWeight[1]);
    //        ActionLastTime = UnityEngine.Random.Range(MinActionTime, MaxActionTime);
    //        isAction = true;
    //        rushing = false;
    //    }

    //    if (rushing)
    //        Rush();

    //    else
    //    {
    //        Debug.Log("not rushing");
    //        switch (act)
    //        {
    //            case 0: // attack
    //                RandomAttackBehavior();
    //                Debug.Log("Attack Mode;)");
    //                break;
    //            case 1:
    //                Patrol();
    //                break;
    //            case 2:
    //                Idle();
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //}

    //protected override void Update()
    //{
    //    base.Update();
    //    //if (_inCrazyMode)
    //    //{
    //    //    _lastingCrazyTime -= Time.deltaTime;
    //    //    if(_lastingCrazyTime <= 0)
    //    //    {
    //    //        // stop crazy mode
    //    //    }
    //    //}
    //}

    protected override void Idle()
    {
        base.Idle();
        SetAllAnimationFalse(); // Idle animation
        if (UnityEngine.Random.Range(0, 1) > 0.7f) // TurnHead animation
            animator.SetBool("TurnHead", true);
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        SetAllAnimationFalse();
        animator.SetBool("Run", true);
    }

    protected override void Patrol()
    {
        base.Patrol();
        SetAllAnimationFalse();
        animator.SetBool("Walk", true);
    }

    //private void Rush() {
    //    Debug.Log("Rush Mode");
    //    rushing = true;

    //    Vector3 p = GetClosestplayer().transform.position;
    //    p = new Vector3(p.x, 0, p.z); // No need to consider y
    //    Vector3 w = new Vector3(transform.position.x, 0, transform.position.z);

    //    float distance = Vector3.Distance(p, w);

    //    SetAllAnimationFalse();
    //    if (distance > AttackRange)
    //    {
    //        animator.SetBool("Run", true);
    //        tmpSpeed = speed;
    //        speed = rushSpped;
    //        GoToPoint(p);
    //    }
    //    else
    //    {
    //        rushing = false;
    //        speed = tmpSpeed;
    //        RandomAttackBehavior();
    //    }
    //}

    private void SetAllAnimationFalse() {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("TurnHead", false);
    }

    //private void GoToPoint(Vector3 point)
    //{
    //    //Debug.Log("mode point: " + point);
    //    float angle = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
    //    Vector3 direction = new Vector3(0f, angle, 0f);

    //    transform.eulerAngles = direction;
    //    transform.position += speed * Time.deltaTime * transform.forward;
    //    //if ((speed * Time.deltaTime * transform.forward).magnitude > 0.1f)
    //    //{
    //    //    Debug.Log("magnitude = " + (speed * Time.deltaTime * transform.forward).magnitude + ", += " + speed * Time.deltaTime * transform.forward);
    //    //    Debug.Log("+= " + speed * Time.deltaTime * transform.forward);
    //    //    transform.position += speed * Time.deltaTime * transform.forward;
    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("+= " + transform.forward / transform.forward.magnitude * 0.1f);
    //    //    transform.position += transform.forward / transform.forward.magnitude * 0.1f;
    //    //}
    //}

    private IEnumerator CrazyMode()
    {
        Debug.Log("Crazy Mode");
        //{ AttackWeight, PatrolWeight, IdleWeight }
        transform.Find("Toon Chicken").GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color32(255, 88, 66, 216));
        speed *= _speedUp;
        int[] cacheWeight = new int[weight.Length];
        Array.Copy(weight, cacheWeight, weight.Length);
        skillWeight[0] = 0;
        skillWeight[1] = 1;
        weight = new int[] { 1, 0, 0 };
        //Debug.Log("skillWeight = " + skillWeight[0] + ", " + skillWeight[1]);
        //Debug.Log("weight = " + weight[0] + ", " + weight[1] + ", " + weight[2]);
        yield return new WaitForSeconds(_crazyTime);
        skillWeight[0] = 1;
        skillWeight[1] = 0;
        speed /= _speedUp;
        Array.Copy(cacheWeight, weight, weight.Length);
        //Debug.Log("skillWeight = " + skillWeight[0] + ", " + skillWeight[1]);
        //Debug.Log("weight = " + weight[0] + ", " + weight[1] + ", " + weight[2]);
        transform.Find("Toon Chicken").GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
        Debug.Log("End Crazy mode");
    }

    public void ChickKilled()
    {
        StartCoroutine(CrazyMode());
    }
}