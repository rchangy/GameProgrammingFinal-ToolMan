using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    /// <summary>
    /// Handling a player's actions, e.g., Attack, Grab, ...
    /// </summary>
    /// 
    private void Attack()
    {
        //Debug.Log("attack pressed");
        combat.Attack();
    }

    private void ComboSkillAttack()
    {
        combat.ComboSkillAttack();
    }

    public void GrabOrRelease()
    {
        if (!grabPoint.IsGrabbing())
        {
            Grab();
            AnimationGrab(anotherPlayer.getTool().getName());
        }
        else
        {
            AnimationRelease(anotherPlayer.getTool().getName());
            Release();
        }
    }

    public void Grab()
    {
        this.confJ = grabPoint.Grab();
    }

    public void Release()
    {
        grabPoint.Release();
        this.confJ = null;
    }

    public void Die()
    {
        isDead = true;
        if (isTool)
        {
            ToolableManTransform();
        }
        AnimationDie();
    }

    public void Hurt()
    {
        AnimationHurt();
    }

    public void Win()
    {
        if (isTool)
            ToolableManTransform();
        animator.SetTrigger("startSpinning");
        animator.SetBool("isSpinning", true);
        winOrLose = true;
    }
    public void Lose()
    {
        winOrLose = true;
    }
}
