using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // ==== Components ====
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider playerCollider;

    int playerNum = 1; // player 1 or player 2
    public bool isTool = false;
    // ==== Components ====

    // ==== Movement ====
    public Transform mainCameraTrans;
    [SerializeField] private float speed = 5;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public float jumpForce = 300;
   
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;

    private float distToGround;
    private bool isGrounded;
    // ==== Movement ====

    // ==== Combat ====
    public CombatUnit combat;
    // ==== Combat ====

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        distToGround = playerCollider.bounds.extents.y;
    }

    private void Update()
    {
        if (!isTool)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

            if (movement.sqrMagnitude > 0.01f)
            {
                // Facing angle (smoothed)
                float movementAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCameraTrans.eulerAngles.y;
                float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);

                // Move
                Vector3 adjustedMovement = Quaternion.Euler(0, movementAngle, 0) * Vector3.forward; // Relative to mainCameraTrans
                adjustedMovement *= speed * Time.deltaTime;
                transform.position += adjustedMovement;
            }

            // Jump
            if (playerNum == 1)
            {
                if (Input.GetButtonDown("JumpOrAttack1"))
                    Jump();
            }
            else if (playerNum == 2)
            {
                if (Input.GetButtonDown("JumpOrAttack2"))
                    Jump();
            }
            isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f);
            if (isGrounded)
                currentJumpCount = 0;
        }

        else
        {
            if (playerNum == 1)
            {
                if (Input.GetButtonDown("JumpOrAttack1"))
                    Attack();
            }
            else if (playerNum == 2) { 
                if (Input.GetButtonDown("JumpOrAttack2"))
                    Attack();
             }
        }
    }

    private void Attack() {
        //Debug.Log("attack pressed");
        combat.Attack();
    }

    private void Jump() {
        if (currentJumpCount < maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpForce);
            currentJumpCount++;
        }

    }
}
