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
        this.grabbedPoint = player.GetGrabbedPoint();
        this.playerRB = player.getRigidbody();
        //point = new Vector3(0.0f, -1.2f, 0.0f);
    }

    public virtual void toTool()
    {
        animator.ResetTrigger("toMan");
        animator.SetTrigger("toTool");
        animator.SetBool("isTool", true);
        playerRB.constraints = RigidbodyConstraints.None;
        player.resetRigidBody();
    }

    public void toMan()
    {
        // release
        if (player.IsGrabbed())
            player.GetAnotherPlayer().Release();
        // reset parameters of animator
        animator.SetBool("isTool", false);
        animator.SetTrigger("toMan");
        animator.ResetTrigger("toTool");
        animator.ResetTrigger("toPickaxe");
        animator.ResetTrigger("toBoomerang");
        animator.ResetTrigger("toShield");
        animator.ResetTrigger("toFlashBomb");
        animator.ResetTrigger("toLightSaber");
        animator.ResetTrigger("Attack");

        // reset grabbed point (need to fix this part if changing animation)
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
        point = new Vector3(0.0f, -1.2f, 0.0f);

        // ==== reset player ==== //
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        player.gameObject.transform.rotation = Quaternion.identity;
        // when a tool transform to a man, it may stuck into ground and fall down, this may need to be adjusted (position.y)
        float newY;
        if (!player.IsGrabbed())
            newY = player.transform.position.y - player.GetCollider().bounds.extents.y + player.distToGround + 0.2f;
        else
            newY = player.GetAnotherPlayer().transform.position.y;
        player.gameObject.transform.position = new Vector3(player.gameObject.transform.position.x, newY, player.gameObject.transform.position.z);
        player.resetRigidBody();
        // ==== reset player ====
        player.setEmission(false);
    }

    public Vector3 getPoint()
    {
        //return point;
        return grabbedPoint.transform.localPosition;
    }
    public Vector3 getTheToolEulerAngle()
    {
        return theToolEulerAngle;
    }
}
