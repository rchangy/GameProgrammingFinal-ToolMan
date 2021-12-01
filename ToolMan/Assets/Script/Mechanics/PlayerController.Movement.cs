using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    /// <summary>
    /// Handling a player's movement and Animation State
    /// </summary>

    private void ManageMovement()
    {
        horizontal = 0;
        vertical = 0;
        horizontal = keyboardInputController.MoveHorizontal(playerNum) * moveAngleSensitivity;
        vertical = keyboardInputController.MoveVertical(playerNum);

        

        // Jump
        if (keyboardInputController.JumpOrAttack(playerNum))
            Jump();
        isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f);
        if (isGrounded)
        {
            currentJumpCount = 0;
        }
    }

    private void Jump()
    {
        if (currentJumpCount < maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpForce);
            currentJumpCount++;
        }

    }
}
