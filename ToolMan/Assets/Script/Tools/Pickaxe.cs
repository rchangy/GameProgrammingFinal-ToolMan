using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Tool
{
    public Pickaxe(PlayerController player)
    {
        setUp(player);
        name = "Pickaxe";
        //theToolEulerAngle = new Vector3(0f, 90f, -26f);
        //theToolEulerAngle = new Vector3(-74.36f, -69.445f, -35.713f);
        //theToolEulerAngle = new Vector3(-48.5f, -58.5f, -32.5f);
        theToolEulerAngle = new Vector3(0f, 0f, 0f);
    }

    public override void toTool()
    {
        base.toTool();
        animator.SetBool("isPickaxe", true);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.48f, 0.0f);
        point = new Vector3(0.0f, -1.48f, 0.0f);
    }
}
