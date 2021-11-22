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
        //Debug.Log("no:(( player = " + p.name);
        
        //AssignCamera(index+1);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        p.SetMovementInput(movementInput);
        Debug.Log("OnMove: " + movementInput);

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        //jumped = context.action.triggered;
        //Debug.Log(gameObject.name + " jump input = " + jumped);
    }
    public void OnLook(InputAction.CallbackContext context) {
        Vector2 lookInput = context.ReadValue<Vector2>();
        Debug.Log("look = " + lookInput);
        if (lookInput.magnitude == 0)
            camController.DisableCam();
        else
            camController.EnableCam();
    }

    private void AssignCamera(int index) {
        if (index == 1)
            camController = GameObject.Find("Player1FreeLook").GetComponent<CameraController>();
        else if (index == 2)
            camController = GameObject.Find("Player2FreeLook").GetComponent<CameraController>();
    }
}
