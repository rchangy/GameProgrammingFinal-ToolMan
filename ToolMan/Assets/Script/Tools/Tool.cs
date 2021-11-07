using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool
{
    protected GameObject grabPoint;
    protected Animator animator;
    public void setAnimator(Animator animator)
    {
        this.animator = animator;
    }
    
    public GameObject getGrabPoint()
    {
        return grabPoint;
    }

    public void setGrabPoint(GameObject grabPoint)
    {
        this.grabPoint = grabPoint;
    }

    public abstract void toTool();
    public void attack()
    {
        animator.SetBool("Attacking", true);
    }
}
