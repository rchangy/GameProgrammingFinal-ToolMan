using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void UpdateState()
    {
        if (!isGrounded)
        {
            animator.SetBool("inFlight", true);
            animator.SetBool("isGrounded", false);
            animator.SetFloat("jumpVelocity", rb.velocity.y);
        }
        else
        {
            animator.SetFloat("verticalVelocity", Mathf.Abs(vertical));
            animator.SetFloat("jumpVelocity", 0);
            animator.SetBool("inFlight", false);
            animator.SetBool("isGrounded", true);
        }
        //if (!isTool && (Mathf.Abs(vertical) >= 0.01f || !isGrounded) && !isMoving)
        //{
        //    animator.SetTrigger("Move");
        //    isMoving = true;
        //}
        //else
        //{
        //    isMoving = false;
        //}
    }

    public void AnimationAttack()
    {
        animator.SetTrigger("Attack");
        anotherPlayer.animator.SetTrigger("Attack");
    }

    public void AnimationHurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void AnimationDie()
    {
        animator.SetBool("isAlive", false);
        animator.SetTrigger("isDead");
    }

    public void AnimationGrab(string toolName)
    {
        ResetAnimationGrab();
        animator.SetTrigger("Grab");
        animator.ResetTrigger("Release");
        animator.SetBool("isGrabbing", true);
        animator.SetBool("isGrabbing" + toolName, true);
        anotherPlayer.animator.SetBool("Grabbed", true);
    }
    public void AnimationRelease(string toolName)
    {
        animator.ResetTrigger("Grab");
        animator.SetTrigger("Release");
        animator.SetBool("isGrabbing", false);
        ResetAnimationGrab();
        anotherPlayer.animator.SetBool("Grabbed", false);
    }
    public void ResetAnimationGrab()
    {
        animator.SetBool("isGrabbingPickaxe", false);
        animator.SetBool("isGrabbingBoomerang", false);
        animator.SetBool("isGrabbingShield", false);
        animator.SetBool("isGrabbingLightSaber", false);
        animator.SetBool("isGrabbingFlashBomb", false);
    }
    public void AnimationUnlock(int level)
    {
        animator.SetTrigger("unlock");
        animator.SetInteger("unlockIdx", level);
    }
    public void AnimtationForceToMan()
    {
        Effect toManEffect = effectController.effectList.Find(e => e.name == "ToManEffect");
        toManEffect.PlayEffect();
        animator.SetBool("isTool", false);
        animator.SetTrigger("toMan");
    }
    public void cutSceneToMan()
    {
        // reset parameters of animator
        animator.SetBool("isTool", false);
        //animator.SetTrigger("toMan");
        animator.ResetTrigger("toTool");
        animator.ResetTrigger("toPickaxe");
        animator.ResetTrigger("toBoomerang");
        animator.ResetTrigger("toShield");
        animator.ResetTrigger("toFlashBomb");
        animator.ResetTrigger("toLightSaber");
        animator.ResetTrigger("Attack");

        // reset grabbed point (need to fix this part if changing animation)
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
        //point = new Vector3(0.0f, -1.2f, 0.0f);

        // ==== reset player ==== //
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        transform.rotation = Quaternion.Euler(0, 154, 0);
        // when a tool transform to a man, it may stuck into ground and fall down, this may need to be adjusted (position.y)
        float newY;
        newY = transform.position.y - GetCollider().bounds.extents.y - 1;
        transform.position = new Vector3(gameObject.transform.position.x-1, newY, gameObject.transform.position.z + 0.5f);
        //resetRigidBody();
    }
}
