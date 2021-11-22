using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Cinemachine;

public class PlayerInputHandler : MonoBehaviour
{
    Vector2 movementInput = Vector2.zero;
    public PlayerController p;
    private PlayerInput playerInput;
    CameraController camController;

    private void Awake()
    {        
        //AssignCamera(index+1);
    }
    private void AssignCamera(int index)
    {
        if (index == 1)
            camController = GameObject.Find("Player1FreeLook").GetComponent<CameraController>();
        else if (index == 2)
            camController = GameObject.Find("Player2FreeLook").GetComponent<CameraController>();
    }

    // ==== Movement ====
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        p.SetMovementInput(movementInput);
        Debug.Log("OnMove: " + movementInput);

    }
    public void OnAttackOrJump(InputAction.CallbackContext context)
    {
        Debug.Log("aaaaaaa " + context.action.triggered);
        if (context.action.triggered)
            p.AttackOrJump();
    }
    // ==== Movement ====


    // ==== Look ====
    public void OnLook(InputAction.CallbackContext context) {
        Vector2 lookInput = context.ReadValue<Vector2>();
        Debug.Log("look = " + lookInput);
        if (lookInput.magnitude == 0)
            camController.DisableCam();
        else
            camController.EnableCam();
    }
    // ==== Look ====


    // ==== UI ====
    public void OnUI_Prev(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
            p.UI_Prev();
    }
    public void OnUI_Next(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
            p.UI_Next();
    }
    public void OnUI_Select(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
            p.UI_Choose();
    }
    // ==== UI ====
}
