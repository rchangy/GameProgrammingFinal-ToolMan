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
        float horizontal = 0, vertical = 0;
        horizontal = keyboardInputController.MoveHorizontal(playerNum) * moveAngleSensitivity * Time.deltaTime;
        vertical = keyboardInputController.MoveVertical(playerNum);

        transform.Rotate(Vector3.up * horizontal);
        transform.position += vertical * transform.forward * speed * Time.deltaTime;

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
