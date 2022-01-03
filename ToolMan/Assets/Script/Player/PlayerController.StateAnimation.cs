using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void UpdateState()
    {
        if (!isGrounded)
        {
            animator.SetBool("inFlight", true);
            animator.SetBool("isGrounded", false);
            animator.SetFloat("jumpVelocity", rb.velocity.y);
        }
        else
        {
            animator.SetFloat("verticalVelocity", Mathf.Abs(vertical));
            animator.SetFloat("jumpVelocity", 0);
            animator.SetBool("inFlight", false);
            animator.SetBool("isGrounded", true);
        }
        //if (!isTool && (Mathf.Abs(vertical) >= 0.01f || !isGrounded) && !isMoving)
        //{
        //    animator.SetTrigger("Move");
        //    isMoving = true;
        //}
        //else
        //{
        //    isMoving = false;
        //}
    }

    public void AnimationAttack()
    {
        animator.SetTrigger("Attack");
        anotherPlayer.animator.SetTrigger("Attack");
    }

    public void AnimationHurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void AnimationDie()
    {
        animator.SetBool("isAlive", false);
        animator.SetTrigger("isDead");
    }

    public void AnimationGrab(string toolName)
    {
        animator.SetBool("isGrabbing", true);
        animator.SetBool("isGrabbing" + toolName, true);
        anotherPlayer.animator.SetBool("Grabbed", true);
    }
    public void AnimationRelease(string toolName)
    {
        animator.SetBool("isGrabbing", false);
        animator.SetBool("isGrabbing" + toolName, false);
        anotherPlayer.animator.SetBool("Grabbed", false);
    }
    public void AnimationUnlock(int level)
    {
        animator.SetTrigger("unlock");
        animator.SetInteger("unlockIdx", level);
    }
}
