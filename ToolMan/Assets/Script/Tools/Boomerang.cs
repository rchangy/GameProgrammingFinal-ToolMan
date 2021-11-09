using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Tool
{

    public Boomerang(GameObject player)
    {
        setUp(player);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isBoomerang", true);
        grabbedPoint.transform.localPosition = new Vector3(-0.4f, -1.1f, 0.0f);
    }
}
