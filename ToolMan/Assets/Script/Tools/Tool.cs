using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool
{
    protected GameObject grabbedPoint;
    protected Animator animator;
    protected Rigidbody playerRD;
    public void setAnimator(Animator animator)
    {
        this.animator = animator;
    }
    
    public GameObject getGrabbedPoint()
    {
        return grabbedPoint;
    }

    public void setGrabbedPoint(GameObject grabPoint)
    {
        this.grabbedPoint = grabPoint;
    }

    public void setPlayerRD(Rigidbody playerRD)
    {
        this.playerRD = playerRD;
    }

    public abstract void toTool();

    public void toMan()
    {
        animator.SetBool("isTool", false);
        animator.SetBool("isShield", false);
        animator.SetBool("isFlashBomb", false);
        animator.SetBool("isSword", false);
        animator.SetBool("isBoomerang", false);
        animator.SetBool("isPickaxe", false);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
    }

    public void attack()
    {
        animator.SetBool("Attacking", true);
    }
}
