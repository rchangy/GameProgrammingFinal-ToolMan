using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBomb : Tool
{

    public FlashBomb(Animator animator, GameObject grabPoint)
    {
        setAnimator(animator);
        setGrabPoint(grabPoint);
    }

    public override void toTool()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isFlashBomb", true);
        grabPoint.transform.localPosition = new Vector3(0.0f, 0.2f, 0.7f);
    }
}
