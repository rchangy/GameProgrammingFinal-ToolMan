using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Tool
{

    public Boomerang(PlayerController player)
    {
        setUp(player);
        name = "Boomerang";
        //theToolEulerAngle = new Vector3(0f, 90f, -26f);
        //theToolEulerAngle = new Vector3(0f, 0f, 0f);
        theToolEulerAngle = new Vector3(-50f, -58f, -32.5f);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isBoomerang", true);
        grabbedPoint.transform.localPosition = new Vector3(-0.46f, -1f, 0f);
        point = new Vector3(-0.46f, -1f, 0f);
    }
}
