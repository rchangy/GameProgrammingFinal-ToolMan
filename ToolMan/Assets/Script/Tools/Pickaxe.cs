using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Tool
{
    public Pickaxe(GameObject player)
    {
        setUp(player);
        name = "Pickaxe";
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isPickaxe", true);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.4f, 0.0f);
        point = new Vector3(0.0f, -1.4f, 0.0f);
    }
}
