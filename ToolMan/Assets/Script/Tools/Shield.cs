using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Tool
{

    public Shield(PlayerController player)
    {
        setUp(player);
        name = "Shield";
        theToolEulerAngle = new Vector3(0f, 0f, 0f);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetTrigger("toShield");
        grabbedPoint.transform.localPosition = new Vector3(0.3f, 0.6f, -0.9f);
        point = new Vector3(0.3f, 0.6f, -0.9f);
    }
}

