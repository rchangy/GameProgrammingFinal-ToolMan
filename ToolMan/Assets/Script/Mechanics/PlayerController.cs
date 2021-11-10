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
    public GameObject toolListUI;
    public GameObject grabbedPoint;
    public GrabbedPoint grabbedPointClass;
    public GrabPoint grabPoint;
    public int playerNum = 1; // player 1 or player 2
    // ==== Components ====

    // ==== to Tool ====
    public bool isTool = false;
    List<Tool> tools = new List<Tool>();
    private int toolIdx;
    // ==== to Tool ====

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
        grabbedPointClass = grabbedPoint.GetComponent<GrabbedPoint>();
        grabPoint.setPlayer(this);
        grabbedPointClass.setPlayer(this);

        if (playerNum == 1)
        {
            tools.Add(new Pickaxe(gameObject));
            tools.Add(new Boomerang(gameObject));
        }
        else if (playerNum == 2)
        {
            tools.Add(new Shield(gameObject));
            tools.Add(new FlashBomb(gameObject));
            tools.Add(new LightSaber(gameObject));
        }

        distToGround = playerCollider.bounds.extents.y;
    }

    private void Update()
    {
        if (!isTool)
        {
            // ==== Movement ====
            float horizontal = 0, vertical = 0;
            if (playerNum == 1)
            {
                horizontal = Input.GetAxisRaw("Horizontal1");
                vertical = Input.GetAxisRaw("Vertical1");
            }
            if (playerNum == 2)
            {
                horizontal = Input.GetAxisRaw("Horizontal2");
                vertical = Input.GetAxisRaw("Vertical2");
            }
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
            // ==== Movement ====
        }

        else // Tool
        {
            // Attack
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

        // ==== Man <-> Tool ====
        if ((Input.GetButtonDown("Choose1") && playerNum == 1) || (Input.GetButtonDown("Choose2") && playerNum == 2))
            SelectTool();
        // ==== Man <-> Tool ====
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

    public void SelectTool() {
        isTool = !isTool;
        if (isTool)
        {
            toolIdx = toolListUI.GetComponent<ObjectListUI>().currentIdx;
            tools[toolIdx].toTool();
            combat.SetCurrentUsingSkill(tools[toolIdx].getName());
        }
        else
        {
            tools[toolIdx].toMan();
            toolListUI.GetComponent<ObjectListUI>().unchoose = toolIdx;
        }
    }

    public Rigidbody getRigidbody()
    {
        return rb;
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public GameObject getGrabbedPoint()
    {
        return grabbedPoint;
    }

    public Tool getTool()
    {
        return tools[toolIdx];
    }

    public void beGrabbed()
    {
        tools[toolIdx].beGrabbed();
        grabbedPoint.GetComponent<Collider>().isTrigger = true;
        //gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        gameObject.GetComponent<Collider>().isTrigger = true;
    }

    public void beReleased()
    {
        tools[toolIdx].beReleased();
        grabbedPoint.GetComponent<Collider>().isTrigger = false;
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<Collider>().isTrigger = false;
    }
}
