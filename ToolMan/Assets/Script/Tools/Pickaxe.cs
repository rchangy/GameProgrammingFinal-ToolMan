using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Tool
{
    public Pickaxe(Animator animator, GameObject grabPoint, Rigidbody playerRD)
    {
        setAnimator(animator);
        setGrabbedPoint(grabPoint);
        setPlayerRD(playerRD);
    }

    public override void toTool()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isPickaxe", true);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.4f, 0.0f);
    }
}
