using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Tool
{

    public Boomerang(PlayerController player)
    {
        setUp(player);
        name = "Boomerang";
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isBoomerang", true);
        grabbedPoint.transform.localPosition = new Vector3(-0.4f, -1.1f, 0.0f);
        point = new Vector3(-0.4f, -1.1f, 0.0f);
    }
}
