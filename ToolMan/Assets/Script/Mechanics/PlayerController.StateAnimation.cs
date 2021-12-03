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
        }
        else
        {
            Debug.Log("isGrounded = " + isGrounded);
            Debug.Log("distToGround = " + distToGround);
            animator.SetFloat("velocity_x", Mathf.Abs(vertical));
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
