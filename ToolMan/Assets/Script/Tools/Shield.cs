using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Tool
{

    public Shield(GameObject player)
    {
        setUp(player);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isShield", true);
        grabbedPoint.transform.localPosition = new Vector3(0.3f, 0.6f, -0.9f);
    }
}

