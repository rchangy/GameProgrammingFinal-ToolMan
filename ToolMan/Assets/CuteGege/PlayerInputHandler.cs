using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerInputHandler : MonoBehaviour
{
    Vector2 movementInput = Vector2.zero;
    NewPlayerController p;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<NewPlayerController>();
        var index = playerInput.playerIndex;
        Debug.Log("input index = " + index);
        p = players.FirstOrDefault(p => p.playerNum == index+1);
        Debug.Log("player input : p = " + p.name);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        p.Move(movementInput);
        //Debug.Log(gameObject.name + " movement input = " + movementInput);

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        //jumped = context.action.triggered;
        //Debug.Log(gameObject.name + " jump input = " + jumped);
    }
}
