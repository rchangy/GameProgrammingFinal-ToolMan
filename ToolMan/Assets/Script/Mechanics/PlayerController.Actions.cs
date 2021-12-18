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
            if (this.confJ != null)
                AnimationGrab(anotherPlayer.getTool().getName());
        }
        else
        {
            if (this.confJ != null)
                AnimationRelease(anotherPlayer.getTool().getName());
            Release();
        }
    }

    public void Grab()
    {
        this.confJ = grabPoint.Grab();
    }

    public void forceGrabbing()
    {
        grabPoint.setAnotherPlayerAndTarget(this);
        Grab();
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
            if (anotherPlayer.IsGrabbing())
            {
                anotherPlayer.Release();
            }
            ToolableManTransform();
        }
        else
        {
            if (IsGrabbing())
                Release();
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
