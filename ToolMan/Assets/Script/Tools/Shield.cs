using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Tool
{

    public Shield(Animator animator, GameObject grabPoint, Rigidbody playerRD)
    {
        setAnimator(animator);
        setGrabbedPoint(grabPoint);
        setPlayerRD(playerRD);
    }

    public override void toTool()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isShield", true);
        grabbedPoint.transform.localPosition = new Vector3(0.3f, 0.6f, -0.9f);
    }
}

