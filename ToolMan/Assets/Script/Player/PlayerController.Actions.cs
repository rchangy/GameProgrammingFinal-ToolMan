using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;

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
        }
        else
        {
            Release();
        }
    }

    public void Grab()
    {
        this.confJ = grabPoint.Grab();
        if (this.confJ != null)
            AnimationGrab(anotherPlayer.getTool().getName());
    }

    public void forceGrabbing()
    {
        grabPoint.setAnotherPlayerAndTarget(anotherPlayer);
        Grab();
    }

    public void Release()
    {
        if (this.confJ != null)
            AnimationRelease(anotherPlayer.getTool().getName());
        grabPoint.Release();
        this.confJ = null;
        Debug.Log("release!!!!!!!!!!");
    }

    public void AddHP(int hpAdd)
    {
        Schedule<PlayerAddHP>().player = this;
        Effect addHPEffect = effectController.effectList.Find(e => e.name == "AddHPEffect");
        if (addHPEffect != null)
            addHPEffect.PlayEffect();
        combat.AddHP(hpAdd);
    }

    public void Die()
    {
        isDead = true;
        controlEnable = false;
        if (isTool.Value)
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
        if (isTool.Value)
            ToolableManTransform();
        animator.SetTrigger("startSpinning");
        animator.SetBool("isSpinning", true);
        controlEnable = false;
    }
    public void Lose()
    {
        controlEnable = false;
    }
}
