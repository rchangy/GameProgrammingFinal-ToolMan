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
            Debug.Log("isGrounded = " + isGrounded);
            Debug.Log("distToGround = " + distToGround);
            animator.SetFloat("verticalVelocity", Mathf.Abs(vertical));
            animator.SetFloat("jumpVelocity", 0);
            animator.SetBool("inFlight", false);
            animator.SetBool("isGrounded", true);
        }
    }

    public enum State
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed,
        Tool
    }
}
