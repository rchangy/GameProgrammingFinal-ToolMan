using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyNormalChicken : Enemy
{

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

    protected override void Hurt()
    {
        animator.SetTrigger("Hurt");
        // animation
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
}