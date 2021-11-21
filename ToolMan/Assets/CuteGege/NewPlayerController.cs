using UnityEngine;
using ToolMan.Mechanics;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class NewPlayerController : ToolableMan
{
    // ==== Components ====
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    [SerializeField] private GameObject toolListUI;
    public GrabPoint grabPoint;

    private State state = new State();
    
    private KeyboardInputController keyboardInputController;

    public int playerNum = 1; // player 1 or player 2
    [SerializeField] private PlayerController anotherPlayer;
    [SerializeField] private bool changeable = false;

    // ==== Components ====


    // ==== Player Movement ====
    Vector2 movementInput = Vector2.zero;
    bool jumped = false;
    [SerializeField] private float speed = 5;
    public float jumpForce = 300;
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;

    private float distToGround;
    private bool isGrounded;
    // ==== Player Movement ====

    // ==== Camera Movement ====
    public Transform mainCameraTrans;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    // ==== Camera Movement ====

    // ==== Combat ====
    public CombatUnit combat;
    // ==== Combat ====

    override protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        grabbedPointController = grabbedPoint.GetComponent<GrabbedPoint>();
        //grabPoint.setPlayer(this);
        //grabbedPointController.setPlayer(this);
        //keyboardInputController = new KeyboardInputController();
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
    }

    override protected void Update()
    {
        //if (!isTool)
        //{
        //    //Move();
        //}

        //else // Tool
        //{
        //    // Attack
        //    if (keyboardInputController.JumpOrAttack(playerNum))
        //        Attack();
        //}

        // ==== Man <-> Tool ====
        //if (keyboardInputController.Choose(playerNum))
        //    ToolableManTransform();
        // ==== Man <-> Tool ====
    }

    // ==== Movement ====
    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    movementInput = context.ReadValue<Vector2>();
    //    Debug.Log(gameObject.name + " movement input = " + movementInput);

    //}
    //public void OnJump(InputAction.CallbackContext context)
    //{
    //    jumped = context.action.triggered;
    //    Debug.Log(gameObject.name + " jump input = " + jumped);
    //}

    public void Move(Vector2 movementInput)
    {
        if (isTool)
            return;

        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        Debug.Log(gameObject.name + " movement = " + movement);
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

        //if (jumped)
        //    Jump();
        //isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f);
        //if (isGrounded)
        //{
        //    currentJumpCount = 0;
        //}
    }
    private void Jump()
    {
        if (currentJumpCount < maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpForce);
            currentJumpCount++;
        }

    }
    // ==== Movement ====

    //// ==== Actions ====
    //private void Attack()
    //{
    //    //Debug.Log("attack pressed");
    //    combat.Attack();
    //}

    override public void ToolableManTransform() {
        //    isTool = !isTool;
        //    if (isTool)
        //    {
        //        toolIdx = toolListUI.GetComponent<ObjectListUI>().currentIdx;
        //        tools[toolIdx].toTool();
        //        combat.SetCurrentUsingSkill(tools[toolIdx].getName());
        //        changeable = false;
        //    }
        //    else
        //    {
        //        tools[toolIdx].toMan();
        //        toolListUI.GetComponent<ObjectListUI>().unchoose = toolIdx;
        //    }
    }

    //public void ToolManChange() // Man Player
    //{
    //    // set anotherplayer
    //    grabPoint.setAnotherPlayerAndTarget(anotherPlayer);
    //    //anotherPlayer.grabPoint.setAnotherPlayerAndTarget(this);
    //    //anotherPlayer.changeable = false;

    //    // cache position
    //    Vector3 manPosition = this.transform.position;
    //    Vector3 toolPosition = anotherPlayer.transform.position;

    //    // Release & Transform
    //    grabPoint.Release();
    //    transform.position = toolPosition;
    //    anotherPlayer.transform.position = manPosition;
    //    anotherPlayer.ToolableManTransform(); // Tool to Man
    //    toolIdx = 0;
    //    ToolableManTransform(); // Man to Tool

    //    if (!anotherPlayer.grabPoint.grabbing)
    //    {
    //        anotherPlayer.grabPoint.Grab();
    //    }
    //}
    //// ==== Actions ====



    // ==== getters & setters ====
    public Rigidbody getRigidbody()
    {
        return rb;
    }

    public Animator getAnimator()
    {
        return animator;
    }
    public void setChangeable(bool changeable)
    {
        this.changeable = changeable;
    }
    // ==== getters

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
