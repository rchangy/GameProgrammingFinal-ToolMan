using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Tool
{
    public Pickaxe(Animator animator, GameObject grabPoint)
    {
        setAnimator(animator);
        setGrabPoint(grabPoint);
    }

    public override void toTool()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isPickaxe", true);
        grabPoint.transform.localPosition = new Vector3(0.0f, -1.4f, 0.0f);
    }
}
