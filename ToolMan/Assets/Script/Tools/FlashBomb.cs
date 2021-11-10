using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBomb : Tool
{

    public FlashBomb(GameObject player)
    {
        setUp(player);
        name = "FlashBomb";
    }


    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isFlashBomb", true);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, 0.25f, 0.7f);
        point = new Vector3(0.0f, 0.25f, 0.7f);
    }
}
