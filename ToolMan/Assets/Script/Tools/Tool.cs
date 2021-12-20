using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool
{
    protected string name;
    protected PlayerController player;
    protected GameObject grabbedPoint;
    protected Animator animator;
    protected Rigidbody playerRB;
    protected Vector3 point;
    protected Vector3 theToolEulerAngle; // the specific enuler angle when the player-tool is being grabbed

    public string getName()
    {
        return name;
    }
    public void setUp(PlayerController player)
    {
        this.player = player;
        this.animator = player.getAnimator();
        this.grabbedPoint = player.GetGrabbedPoint().gameObject;
        this.playerRB = player.getRigidbody();
        //point = new Vector3(0.0f, -1.2f, 0.0f);
    }

    public virtual void toTool()
    {
        animator.SetBool("isTool", true);
        playerRB.constraints = RigidbodyConstraints.None;
        player.resetRigidBody();
    }

    public void toMan()
    {
        // reset parameters of animator
        animator.SetBool("isTool", false);
        animator.SetBool("isShield", false);
        animator.SetBool("isFlashBomb", false);
        animator.SetBool("isLightSaber", false);
        animator.SetBool("isBoomerang", false);
        animator.SetBool("isPickaxe", false);

        // reset grabbed point (need to fix this part if changing animation)
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
        //point = new Vector3(0.0f, -1.2f, 0.0f);

        // ==== reset player ==== //
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        player.gameObject.transform.rotation = Quaternion.identity;
        // when a tool transform to a man, it may stuck into ground and fall down, this may need to be adjusted (position.y)
        player.gameObject.transform.position = new Vector3(player.gameObject.transform.position.x, player.GetAnotherPlayer().transform.position.y, player.gameObject.transform.position.z);
        player.resetRigidBody();
        // ==== reset player ====
        player.GetMaterial().DisableKeyword("_EMISSION");
    }

    public Vector3 getPoint()
    {
        return point;
    }
    public Vector3 getTheToolEulerAngle()
    {
        return theToolEulerAngle;
    }
}
