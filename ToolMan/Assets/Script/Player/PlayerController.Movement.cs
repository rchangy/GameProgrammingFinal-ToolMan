using System.Collections;
using System.Collections.Generic;
using static ToolMan.Core.Simulation;
using ToolMan.Gameplay;
using UnityEngine;

public partial class PlayerController
{
    /// <summary>
    /// Handling a player's movement and Animation State
    /// </summary>
    /// 

    // ==== status ====
    [SerializeField] private float speed = 20;
    public float moveAngleSensitivity = 750f;
    public float jumpForce = 300;
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;
    

    // ==== wave ====
    private bool toolWave = false;
    public bool ToolWave
    {
        get => toolWave;
    }
    private Vector3 specialToolEulerAngle; // the euler angle at the ending of waving
    private Vector3 toolEulerAngle; // current euler angle of the player - tool
    private Vector3 originalToolEulerAngle; // the euler angle before starting waving
    private float waveSpeed;
    private bool shouldWaveBack;
    private bool inWaveBack;
    private bool waveDirChange;
    private bool waveEnd;
    public bool WaveEnd
    {
        get => waveEnd;
    }

    private void ManageMovement()
    {
        horizontal = 0;
        vertical = 0;
        if (controlEnable)
        {
            horizontal = keyboardInputController.MoveHorizontal(playerNum) * moveAngleSensitivity;
            vertical = keyboardInputController.MoveVertical(playerNum);
            // Jump
            if (keyboardInputController.JumpOrAttack(playerNum))
                Jump();
        }
        isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f, groundLayerMask.value);
        if (isGrounded)
        {
            currentJumpCount = 0;
        }
    }

    private void Jump()
    {
        currentJumpCount++;
        if (currentJumpCount < maxJumpCount || isGrounded)
        {
            Schedule<PlayerJumped>().player = this;
            rb.AddForce(Vector3.up * jumpForce);
        }

    }

    private void beGrabbedMovement()
    {
        //if (!toolWave)
        //{
        //    toolEulerAngle = tools[toolIdx].getTheToolEulerAngle();
        //}
        //else
        //{
        //    ManageWaving();
        //}
        //transform.eulerAngles = anotherPlayer.transform.eulerAngles + toolEulerAngle;
        toolEulerAngle = tools[toolIdx.Value].getTheToolEulerAngle();
        if (tools[toolIdx.Value].getName().Equals("Shield") || tools[toolIdx.Value].getName().Equals("FlashBomb"))
        {
            transform.eulerAngles = anotherPlayer.transform.eulerAngles + toolEulerAngle;
        }
        else if (tools[toolIdx.Value].getName().Equals("Pickaxe"))
        {
            transform.eulerAngles = anotherPlayer.GetForearm().transform.eulerAngles;
            transform.Rotate(new Vector3(0f, -90f, 0f), Space.Self);
        }
        else
        {
            transform.eulerAngles = anotherPlayer.GetForearm().transform.eulerAngles + toolEulerAngle;
        }
        waveEnd = !anotherPlayer.IsAnimationAttacking();
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
        originalToolEulerAngle = tools[toolIdx.Value].getTheToolEulerAngle();
    }

    public void ResetToolWave()
    {
        this.toolWave = false;
    }

    //private void ManageWaving()
    //{
    //    if (waveEnd)
    //        return;
    //    if (shouldWaveBack && waveDirChange)
    //    {
    //        Vector3 tmp = specialToolEulerAngle;
    //        specialToolEulerAngle = originalToolEulerAngle;
    //        originalToolEulerAngle = tmp;
    //    }
    //    ComputeToolEulerAngle();
    //}

    private void ComputeToolEulerAngle()
    {
        Vector3 newAngle = toolEulerAngle + (specialToolEulerAngle - originalToolEulerAngle) * Time.deltaTime * waveSpeed;
        if (!IsCBetweenAB(originalToolEulerAngle, specialToolEulerAngle, newAngle))
        {
            toolEulerAngle = specialToolEulerAngle;
        }
        else
        {
            toolEulerAngle = newAngle;
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
}
