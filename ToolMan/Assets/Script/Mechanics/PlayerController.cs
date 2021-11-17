using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

[RequireComponent(typeof(Animator))]
public class PlayerController : ToolableMan
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

    override protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        grabbedPointController = grabbedPoint.GetComponent<GrabbedPoint>();
        grabPoint.setPlayer(this);
        grabbedPointController.setPlayer(this);
        keyboardInputController = new KeyboardInputController();
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
        if (!isTool)
        {
            ManageMovement();
            UpdateState();
            if (Input.GetKeyDown(KeyCode.Space) && !isTool && anotherPlayer.isTool && changeable)
            {
                ToolManChange();
            }
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

    // ==== Movement ====
    private void ManageMovement()
    {
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

    override public void ToolableManTransform() {
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

    public void ToolManChange() // Man Player
    {
        Debug.Log("ToolManChange!" + ", I am player - " + playerNum);
        Debug.Log("Player - " + playerNum + " is Tool = " + isTool);
        Debug.Log("Player - " + anotherPlayer.playerNum + " is Tool = " + anotherPlayer.isTool);
        // set anotherplayer
        grabPoint.setAnotherPlayerAndTarget(anotherPlayer);
        anotherPlayer.grabPoint.setAnotherPlayerAndTarget(this);
        anotherPlayer.changeable = false;

        // cache position
        Vector3 manPosition = this.transform.position;
        Vector3 toolPosition = anotherPlayer.transform.position;

        // Release & Transform
        grabPoint.Release();
        //rb.useGravity = false;
        //anotherPlayer.getRigidbody().useGravity = false;
        transform.position = toolPosition;
        anotherPlayer.transform.position = manPosition;
        anotherPlayer.ToolableManTransform(); // Tool to Man
        toolIdx = 0;
        ToolableManTransform(); // Man to Tool

        //// set position & Grab
        //if (!grabPoint.grabbing)
        //    grabPoint.Grab();

        if (!anotherPlayer.grabPoint.grabbing)
        {
            Debug.Log("in playerController: player " + anotherPlayer.playerNum + " grabs player " + playerNum);
            anotherPlayer.grabPoint.Grab();
        }
    }

    // ==== Actions ====



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
