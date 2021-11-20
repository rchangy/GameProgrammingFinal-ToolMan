using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Mechanics;
using UnityEngine.InputSystem;

public class PlayerControllerTest : MonoBehaviour
{
    // ==== Components ====
    private Rigidbody rb;
    private State state = new State();
    private BoxCollider playerCollider;
    // ==== Components ====

    // ==== Player Movement ====
    [SerializeField] private float speed = 5;
    public float jumpForce = 300;
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;

    private float distToGround;
    private bool isGrounded;
    // ==== Player Movement ====

    // ==== New input system
    Vector2 movementInput = Vector2.zero;
    bool jumped = false;


    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(gameObject.name + " movement input = " + movementInput);

    }

    public void OnJump(InputAction.CallbackContext context) {
        jumped = context.action.triggered;
        Debug.Log(gameObject.name + " jump input = " + jumped);
    }
    // ==== New input system

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        state = State.Grounded;

        distToGround = playerCollider.bounds.extents.y;
    }

    protected void Update()
    {
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        movement *= speed * Time.deltaTime;
        transform.position += movement;

        if (jumped)
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

    public enum State
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed,
    }
    // ==== State ====
}
