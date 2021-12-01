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
        grabPoint.Release();
        transform.position = toolPosition;
        anotherPlayer.transform.position = manPosition;
        anotherPlayer.ToolableManTransform(); // Tool to Man
        toolIdx = 0;
        ToolableManTransform(); // Man to Tool

        if (!anotherPlayer.grabPoint.grabbing)
        {
            anotherPlayer.grabPoint.Grab();
        }
    }

    public void setChangeable(bool changeable)
    {
        this.changeable = changeable;
    }

    override public void ToolableManTransform()
    {
        isTool = !isTool;
        if (isTool)
        {
            toolListUI.Choose();
            toolIdx = toolListUI.GetComponent<ObjectListUI>().currentIdx;
            tools[toolIdx].toTool();
            //combat.SetCurrentUsingSkill(tools[toolIdx].getName());
            cam.EnableFreeLook();

            //effect
            Effect toToolEffect = effectController.effectList.Find(e => e.name == "ToToolEffect");
            toToolEffect.PlayEffect();
        }
        else
        {
            tools[toolIdx].toMan();
            toolListUI.GetComponent<ObjectListUI>().unchoose = toolIdx;
            toolListUI.Unchoose();
            cam.EnableMain();

            //effect
            Effect toManEffect = effectController.effectList.Find(e => e.name == "ToManEffect");
            toManEffect.PlayEffect();

        }
    }

    // ==== grab/grabbed ====

    public void Release()
    {
        grabbedPoint.resetRigidBody();
    }

    override public void BeGrabbed(PlayerController anotherPlayer)
    {
        tools[toolIdx].beGrabbed();
        grabbedPoint.gameObject.GetComponent<Collider>().isTrigger = true;
        //gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        playerCollider.isTrigger = true;
        grabbedPoint.setAnotherPlayer(anotherPlayer);
    }

    override public void BeReleased()
    {
        tools[toolIdx].beReleased();
        grabbedPoint.gameObject.GetComponent<Collider>().isTrigger = false;
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        playerCollider.isTrigger = false;
        grabbedPoint.setAnotherPlayer(null);
    }
}
