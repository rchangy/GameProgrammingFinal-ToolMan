using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Tool
{
    public Pickaxe(PlayerController player)
    {
        setUp(player);
        name = "Pickaxe";
        theToolEulerAngle = new Vector3(0f, 90f, -26f);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isPickaxe", true);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.4f, 0.0f);
        point = new Vector3(0.0f, -1.4f, 0.0f);
    }
}
