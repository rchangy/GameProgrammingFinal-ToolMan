using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool
{
    protected string name;
    protected GameObject player;
    protected GameObject grabbedPoint;
    protected Animator animator;
    protected Rigidbody playerRB;

    public string getName()
    {
        return name;
    }
    public void setUp(GameObject player)
    {
        setPlayer(player);
        setAnimator(player.GetComponent<PlayerController>().getAnimator());
        setGrabbedPoint(player.GetComponent<PlayerController>().getGrabbedPoint());
        setPlayerRB(player.GetComponent<PlayerController>().getRigidbody());
    }
    public void setAnimator(Animator animator)
    {
        this.animator = animator;
    }
    
    public GameObject getGrabbedPoint()
    {
        return grabbedPoint;
    }

    public void setGrabbedPoint(GameObject grabPoint)
    {
        this.grabbedPoint = grabPoint;
    }

    public void setPlayerRB(Rigidbody playerRB)
    {
        this.playerRB = playerRB;
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    public virtual void toTool()
    {
        animator.SetBool("isTool", true);
        playerRB.constraints = RigidbodyConstraints.None;
    }

    public void toMan()
    {
        animator.SetBool("isTool", false);
        animator.SetBool("isShield", false);
        animator.SetBool("isFlashBomb", false);
        animator.SetBool("isSword", false);
        animator.SetBool("isBoomerang", false);
        animator.SetBool("isPickaxe", false);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        player.transform.rotation = Quaternion.identity;
        resetRigidBody();
    }

    public void attack()
    {
        animator.SetBool("Attacking", true);
    }

    public void resetRigidBody()
    {
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;
    }
}
