using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool
{
    protected GameObject grabbedPoint;
    protected Animator animator;
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

    public abstract void toTool();
    public void attack()
    {
        animator.SetBool("Attacking", true);
    }
}
