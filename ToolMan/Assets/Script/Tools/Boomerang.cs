using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Tool
{

    public Boomerang(Animator animator, GameObject grabPoint)
    {
        setAnimator(animator);
        setGrabbedPoint(grabPoint);
    }

    public override void toTool()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isBoomerang", true);
        grabbedPoint.transform.localPosition = new Vector3(-0.4f, -1.1f, 0.0f);
    }
}
