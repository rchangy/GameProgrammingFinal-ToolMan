using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Tool
{

    public Shield(Animator animator, GameObject grabPoint)
    {
        setAnimator(animator);
        setGrabPoint(grabPoint);
    }

    public override void toTool()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isShield", true);
        grabPoint.transform.localPosition = new Vector3(0.3f, 0.6f, -0.9f);
    }
}

