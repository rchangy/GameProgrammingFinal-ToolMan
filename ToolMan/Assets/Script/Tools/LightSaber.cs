using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaber : Tool
{

    public LightSaber(PlayerController player)
    {
        setUp(player);
        name = "LightSaber";
        theToolEulerAngle = new Vector3(0f, 90f, -26f);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isSword", true);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.3f, 0f);
        point = new Vector3(0.0f, -1.3f, 0.0f);
    }
}