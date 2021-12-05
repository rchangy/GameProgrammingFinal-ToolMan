using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public void ToolManChange() // Man Player
    {
        // set anotherplayer
        grabPoint.setAnotherPlayerAndTarget(anotherPlayer);
        anotherPlayer.grabPoint.setAnotherPlayerAndTarget(this);
        anotherPlayer.changeable = false;

        // cache position
        Vector3 manPosition = this.transform.position;
        Vector3 toolPosition = anotherPlayer.transform.position;

        // Release & Transform
        Release();
        transform.position = toolPosition;
        anotherPlayer.transform.position = manPosition;
        anotherPlayer.ToolableManTransform(); // Tool to Man
        toolIdx = 0;
        ToolableManTransform(); // Man to Tool

        if (!anotherPlayer.IsGrabbing())
        {
            anotherPlayer.Grab();
        }
    }

    public void setChangeable(bool changeable)
    {
        this.changeable = changeable;
    }

    override public void ToolableManTransform()
    {
        if (!isTool)
        {
            // To Tool
            toolListUI.Choose();
            toolIdx = toolListUI.GetComponent<ObjectListUI>().currentIdx;
            tools[toolIdx].toTool();
            combat.SetCurrentUsingSkill(tools[toolIdx].getName());
            cam.EnableFreeLook();

            //effect
            Effect toToolEffect = effectController.effectList.Find(e => e.name == "ToToolEffect");
            toToolEffect.PlayEffect();
            isTool = !isTool;
            animator.SetTrigger("changeToTool");
        }
        else if (!beGrabbed)
        {
            // To Man
            tools[toolIdx].toMan();
            toolListUI.GetComponent<ObjectListUI>().unchoose = toolIdx;
            toolListUI.Unchoose();
            cam.EnableMain();

            //effect
            Effect toManEffect = effectController.effectList.Find(e => e.name == "ToManEffect");
            toManEffect.PlayEffect();
            isTool = !isTool;
        }
    }

    // ==== grab/grabbed ====

    override public void BeGrabbed(PlayerController anotherPlayer)
    {
        // reset
        transform.rotation = Quaternion.Euler(0f, 0f, 26f);
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        resetRigidBody();

        grabbedPoint.gameObject.GetComponent<Collider>().isTrigger = true;
        //gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        playerCollider.isTrigger = true;
        grabbedPoint.setAnotherPlayer(anotherPlayer);

        rb.mass = 0.000001f;
        beGrabbed = true;
    }

    override public void BeReleased()
    {
        rb.mass = 1;
        rb.constraints = RigidbodyConstraints.None;
        
        grabbedPoint.gameObject.GetComponent<Collider>().isTrigger = false;
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        playerCollider.isTrigger = false;
        grabbedPoint.setAnotherPlayer(null);
        beGrabbed = false;
        ResetToolWave();
    }
    public void resetRigidBody()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
