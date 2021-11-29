using UnityEngine;
using ToolMan.Mechanics;
using ToolMan.Combat;
[RequireComponent(typeof(Animator))]
public class PlayerController : ToolableMan
{
    // ==== Components ====
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    [SerializeField] private ObjectListUI toolListUI;
    public GrabPoint grabPoint;

    private State state = new State();
    
    private KeyboardInputController keyboardInputController;

    public int playerNum = 1; // player 1 or player 2
    [SerializeField] private PlayerController anotherPlayer;
    [SerializeField] private bool changeable = false;
    // ==== Components ====

    // ==== Player Movement ====
    [SerializeField] private float speed = 20;
    public float moveAngleSensitivity = 750f;
    public float jumpForce = 300;
    public int maxJumpCount = 1; // It can actaully jump once more 
    public int currentJumpCount = 0;

    private float distToGround;
    private bool isGrounded;
    // ==== Player Movement ====

    // ==== Combat ====
    public PlayerCombat combat;
    // ==== Combat ====

    // ==== Camera ====
    [SerializeField] CameraManager cam;
    // ==== Camera ====

    override protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        grabbedPointController = grabbedPoint.GetComponent<GrabbedPoint>();
        grabPoint.setPlayer(this);
        grabbedPointController.setPlayer(this);
        state = State.Grounded;
        keyboardInputController = new KeyboardInputController();

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
        if (!isTool)
        {
            ManageMovement();
            UpdateState();
        }

        else // Tool
        {
            // Attack
            if (keyboardInputController.JumpOrAttack(playerNum))
                Attack();
        }

        // ==== Man <-> Tool ====
        if (keyboardInputController.Choose(playerNum))
            ToolableManTransform();
        // ==== Man <-> Tool ====
    }


    public void ToolManChange() // Man Player
    {
        // set anotherplayer
        grabPoint.setAnotherPlayerAndTarget(anotherPlayer);
        anotherPlayer.grabPoint.setAnotherPlayerAndTarget(this);
        anotherPlayer.changeable = false;

        // cache position
        Vector3 manPosition = this.transform.position;
        Vector3 toolPosition = anotherPlayer.transform.position;

        // Release & Transform
        grabPoint.Release();
        transform.position = toolPosition;
        anotherPlayer.transform.position = manPosition;
        anotherPlayer.ToolableManTransform(); // Tool to Man
        toolIdx = 0;
        ToolableManTransform(); // Man to Tool

        if (!anotherPlayer.grabPoint.grabbing)
        {
            anotherPlayer.grabPoint.Grab();
        }
    }
  
    public void setChangeable(bool changeable)
    {
        this.changeable = changeable;
    }

    // ==== Movement ====
    private void ManageMovement()
    {
        float horizontal = 0, vertical = 0;
        horizontal = keyboardInputController.MoveHorizontal(playerNum) * moveAngleSensitivity * Time.deltaTime;
        vertical = keyboardInputController.MoveVertical(playerNum);

        transform.Rotate(Vector3.up * horizontal);
        transform.position += vertical * transform.forward * speed * Time.deltaTime;

        // Jump
        if (keyboardInputController.JumpOrAttack(playerNum))
            Jump();
        isGrounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, distToGround + 0.1f);
        if (isGrounded)
        {
            currentJumpCount = 0;
        }
    }
    // ==== Movement ====

    // ==== Actions ====
    private void Attack()
    {
        //Debug.Log("attack pressed");
        combat.Attack();
    }

    private void Jump()
    {
        if (currentJumpCount < maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpForce);
            currentJumpCount++;
        }

    }

    override public void ToolableManTransform()
    {
        isTool = !isTool;
        Debug.Log("Noooo");
        if (isTool)
        {
            toolIdx = toolListUI.GetComponent<ObjectListUI>().currentIdx;
            tools[toolIdx].toTool();
            //combat.SetCurrentUsingSkill(tools[toolIdx].getName());
            cam.EnableFreeLook();
        }
        else
        {
            tools[toolIdx].toMan();
            toolListUI.GetComponent<ObjectListUI>().unchoose = toolIdx;
            cam.EnableMain();
        }
    }
    // ==== Actions ====



    // ==== getters ====
    public Rigidbody getRigidbody()
    {
        return rb;
    }

    public Animator getAnimator()
    {
        return animator;
    }
    // ==== getters

    // ==== State ====
    private void UpdateState()
    {

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
