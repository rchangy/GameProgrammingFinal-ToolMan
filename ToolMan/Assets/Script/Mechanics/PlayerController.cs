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
    [SerializeField] private GameObject toolListUI;
    [SerializeField] private GameObject grabbedPoint;
    private GrabbedPoint grabbedPointController;
    public GrabPoint grabPoint;

    private State state = new State();
    public int playerNum = 1; // player 1 or player 2

    private KeyboardInputController keyboardInputController;
    // ==== Components ====

    // ==== to Tool ====
    public bool isTool = false;
    private List<Tool> tools = new List<Tool>();
    private int toolIdx;
    // ==== to Tool ====

    // ==== Camera Movement ====
    public Transform mainCameraTrans;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    // ==== Camera Movement ====

    // ==== Player Movement ====
    [SerializeField] private float speed = 5;
    public float jumpForce = 300;
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;

    private float distToGround;
    private bool isGrounded;
    // ==== Player Movement ====

    // ==== Combat ====
    public CombatUnit combat;
    // ==== Combat ====

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        grabbedPointController = grabbedPoint.GetComponent<GrabbedPoint>();
        grabPoint.setPlayer(this);
        grabbedPointController.setPlayer(this);
        state = State.Grounded;

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
        keyboardInputController = new KeyboardInputController();
    }

    private void Update()
    {
        if (!isTool)
        {   
            // ==== Movement ====
            float horizontal = 0, vertical = 0;
            horizontal = keyboardInputController.MoveHorizontal(playerNum);
            vertical = keyboardInputController.MoveVertical(playerNum);
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
            if (keyboardInputController.JumpOrAttack(playerNum))
                Jump();
            isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f);
            if (isGrounded)
                currentJumpCount = 0;
            // ==== Movement ====
        }

        else // Tool
        {
            // Attack
            if (keyboardInputController.JumpOrAttack(playerNum))
                Attack();
        }

        // ==== Man <-> Tool ====
        if (keyboardInputController.Choose(playerNum))
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

    // ==== grab/grabbed ====

    public void Release()
    {
        grabbedPointController.resetRigidBody();
    }

    public void beGrabbed(PlayerController anotherPlayer)
    {
        tools[toolIdx].beGrabbed();
        grabbedPoint.GetComponent<Collider>().isTrigger = true;
        //gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        gameObject.GetComponent<Collider>().isTrigger = true;
        grabbedPointController.setAnotherPlayer(anotherPlayer);
    }

    public void beReleased()
    {
        tools[toolIdx].beReleased();
        grabbedPoint.GetComponent<Collider>().isTrigger = false;
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<Collider>().isTrigger = false;
        grabbedPointController.setAnotherPlayer(null);
    }

    // ==== getters/status ====
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

    public bool inToolState()
    {
        return isTool;
    }

    // ==== State ====
    public enum State
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed,
    }
}
