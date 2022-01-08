using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;

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
        toolIdx.Value = 0;
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

    private void ToAnotherTool()
    {
        bool toForceGrab = false;
        if (IsGrabbed())
        {
            toForceGrab = true;
            GetAnotherPlayer().Release();
        }
        tools[toolIdx.Value].toTool();
        combat.SetCurrentUsingSkill(tools[toolIdx.Value].getName());
        combat.AddType(tools[toolIdx.Value].getName());
        Effect toToolEffect = effectController.effectList.Find(e => e.name == "ToToolEffect");
        toToolEffect.PlayEffect();
        Schedule<PlayerToTool>().player = this;
        if (toForceGrab)
            GetAnotherPlayer().forceGrabbing();
    }

    override public void ToolableManTransform()
    {
        if (!isTool.Value && !IsGrabbing())
        {
            // To Tool
            //toolListUI.Choose();
            //toolIdx.Value = toolListUI.GetComponent<ObjectListUI>().currentIdx;
            tools[toolIdx.Value].toTool();
            combat.SetCurrentUsingSkill(tools[toolIdx.Value].getName());
            combat.AddType(tools[toolIdx.Value].getName());
            cam.EnableFreeLook();
            vertical = 0;
            horizontal = 0;
            // audio
            Schedule<PlayerToTool>().player = this;
            //effect
            Effect toToolEffect = effectController.effectList.Find(e => e.name == "ToToolEffect");
            toToolEffect.PlayEffect();
            // grab hint
            grabPoint.TeammateGrabHint(false);
        }
        else if (isTool.Value && !beGrabbed)
        {
            // To Man
            tools[toolIdx.Value].toMan();
            //toolListUI.GetComponent<ObjectListUI>().unchoose = toolIdx.Value;
            //toolListUI.Unchoose();
            combat.RemoveType(tools[toolIdx.Value].getName());
            cam.EnableMain();

            //effect
            Effect toManEffect = effectController.effectList.Find(e => e.name == "ToManEffect");
            toManEffect.PlayEffect();
        }
        isTool.Value = !isTool.Value;
    }

    // ==== grab/grabbed ====

    override public void BeGrabbed(PlayerController anotherPlayer)
    {
        animator.ResetTrigger("Attack");
        // reset
        transform.rotation = Quaternion.Euler(0f, 0f, 26f);
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        resetRigidBody();

        grabbedPoint.gameObject.GetComponent<Collider>().isTrigger = true;
        playerCollider.isTrigger = true;

        rb.mass = 0.000001f;
        beGrabbed = true;
    }

    override public void BeReleased()
    {
        rb.mass = 1;
        rb.constraints = RigidbodyConstraints.None;
        
        grabbedPoint.gameObject.GetComponent<Collider>().isTrigger = false;
        playerCollider.isTrigger = false;
        beGrabbed = false;
        ResetToolWave();
    }
    public void resetRigidBody()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
