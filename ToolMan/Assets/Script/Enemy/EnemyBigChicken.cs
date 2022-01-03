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

    protected override void SetAllAnimationFalse() {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("TurnHead", false);
    }

    protected override void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

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