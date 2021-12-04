using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBomb : Tool
{

    public FlashBomb(PlayerController player)
    {
        setUp(player);
        name = "FlashBomb";
        theToolEulerAngle = new Vector3(0f, 0f, 0f);
    }


    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isFlashBomb", true);
        //grabbedPoint.transform.localPosition = new Vector3(0.0f, 0.2f, 0.6f);
        point = new Vector3(0.0f, 0.2f, 0.6f);
    }
}
