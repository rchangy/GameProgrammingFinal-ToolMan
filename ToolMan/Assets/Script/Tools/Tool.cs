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
    protected Vector3 point;

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
        point = new Vector3(0.0f, -1.2f, 0.0f);
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
        Debug.Log(name + " call to Man");
        animator.SetBool("isTool", false);
        animator.SetBool("isShield", false);
        animator.SetBool("isFlashBomb", false);
        animator.SetBool("isSword", false);
        animator.SetBool("isBoomerang", false);
        animator.SetBool("isPickaxe", false);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
        point = new Vector3(0.0f, -1.2f, 0.0f);
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        player.transform.rotation = Quaternion.identity;
        player.transform.position = new Vector3(player.transform.position.x, 1.9f, player.transform.position.z);
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

    public void beGrabbed()
    {
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        player.transform.rotation = Quaternion.Euler(0f, 0f, 26f);
        resetRigidBody();
    }

    public void beReleased()
    {
        playerRB.constraints = RigidbodyConstraints.None;
    }
    public Vector3 getPoint()
    {
        return point;
    }
}
