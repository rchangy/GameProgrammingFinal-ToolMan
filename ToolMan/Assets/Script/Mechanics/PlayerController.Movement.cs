using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    /// <summary>
    /// Handling a player's movement and Animation State
    /// </summary>
    /// 

    [SerializeField] private float speed = 20;
    public float moveAngleSensitivity = 750f;
    public float jumpForce = 300;
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;

    private bool toolWave = false;
    public bool ToolWave
    {
        get => toolWave;
    }
    private Vector3 specialToolEulerAngle;
    private Vector3 toolEulerAngle;
    private Vector3 originalToolEulerAngle;
    private float waveSpeed;
    private bool shouldWaveBack;
    private bool inWaveBack;
    private bool waveDirChange;
    private bool waveEnd;
    public bool WaveEnd
    {
        get => waveEnd;
    }

    private float distToGround;
    private bool isGrounded;

    private void ManageMovement()
    {
        horizontal = 0;
        vertical = 0;
        horizontal = keyboardInputController.MoveHorizontal(playerNum) * moveAngleSensitivity;
        vertical = keyboardInputController.MoveVertical(playerNum);

        

        // Jump
        if (keyboardInputController.JumpOrAttack(playerNum))
            Jump();
        Debug.Log("++= " + transform.position + playerCollider.center);
        isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f, groundLayerMask.value);
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

    private void beGrabbedMovement()
    {
        if (!toolWave)
        {
            toolEulerAngle = new Vector3(0, 90f, -26f);
        }
        else
        {
            ManageWaving();
        }
        transform.eulerAngles = anotherPlayer.transform.eulerAngles + toolEulerAngle;
    }

    // ==== Wave Tool ====
    // When Attacking, you can call "SetToolWave(...)" to specify the euler angle of the player-tool
    // After finishing attacking, please call "ResetSpecialRotate()" to make the player-tool to follow the player-man
    // There is an exmaple in PlayerController.Update()
    public void SetToolWave(Vector3 specialToolEulerAngle, float waveSpeed, bool shouldWaveBack)
    {
        this.waveSpeed = waveSpeed;
        this.shouldWaveBack = shouldWaveBack;
        this.inWaveBack = false;
        this.waveDirChange = false;
        this.waveEnd = false;
        this.toolWave = true;
        this.specialToolEulerAngle = specialToolEulerAngle;
        originalToolEulerAngle = new Vector3(0, 90f, -26f);
    }

    public void ResetToolWave()
    {
        this.toolWave = false;
    }

    private void ManageWaving()
    {
        if (waveEnd)
            return;
        if (shouldWaveBack && waveDirChange)
        {
            Vector3 tmp = specialToolEulerAngle;
            specialToolEulerAngle = originalToolEulerAngle;
            originalToolEulerAngle = tmp;
        }
        ComputeToolEulerAngle();
    }

    private void ComputeToolEulerAngle()
    {
        Vector3 newAngle = toolEulerAngle + (specialToolEulerAngle - originalToolEulerAngle) * Time.deltaTime * waveSpeed;
        if (!IsCBetweenAB(originalToolEulerAngle, specialToolEulerAngle, newAngle))
        {
            toolEulerAngle = specialToolEulerAngle;
            //if (waveDirChange == true)
            //{
            //    Debug.Log("out");
            //    Debug.Log("new: " + newAngle);
            //    Debug.Log("origin: " + originalToolEulerAngle);
            //    Debug.Log("special: " + specialToolEulerAngle);
            //}
        }
        else
        {
            toolEulerAngle = newAngle;
            //if (waveDirChange == true)
            //{
            //    Debug.Log("in");
            //    Debug.Log("new: " + newAngle);
            //    Debug.Log("origin: " + originalToolEulerAngle);
            //    Debug.Log("special: " + specialToolEulerAngle);
            //}
        }
        if (toolEulerAngle == specialToolEulerAngle)
        {
            if (shouldWaveBack && !inWaveBack)
            {
                inWaveBack = true;
                waveDirChange = true;
            }
            else
                waveEnd = true;
        }
        else
            waveDirChange = false;
    }
    bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
    {
        return Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f;
    }

    // ==== Wave Tool ====
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + playerCollider.center, transform.position + playerCollider.center - new Vector3(0, distToGround + 0.1f, 0));
    }
}
