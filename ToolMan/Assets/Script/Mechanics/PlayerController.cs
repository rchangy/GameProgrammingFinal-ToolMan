using UnityEngine;
using ToolMan.Mechanics;
using ToolMan.Combat;
using ToolMan.Util;

[RequireComponent(typeof(Animator))]
public partial class PlayerController : ToolableMan
{
    /// <summary>
    /// Handle inputs controlling a player.
    /// </summary>

    private KeyboardInputController keyboardInputController;

    // ==== Player Status ====
    public int playerNum = 1; // player 1 or player 2
    private State state = new State();
    private bool isDead = false;

    [SerializeField] private bool changeable = false;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    float horizontal, vertical;
    // ==== Player Status ====

    // ==== Components ====
    private Animator animator;
    private Rigidbody rb;
    private ConfigurableJoint confJ = null;

    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private GrabPoint grabPoint;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private ObjectListUI toolListUI;
    [SerializeField] private PlayerController anotherPlayer;
    [SerializeField] private EffectController effectController;
    // ==== Components ====

    // ==== Combat ====
    public PlayerCombat combat;

    private BoolWrapper _attackCharging = new BoolWrapper();
    private FloatWrapper _attackChargingTime = new FloatWrapper();
    [SerializeField]
    private FloatWrapper _comboSkillChargeTime;
    public bool ComboSkillCharged
    {
        get => (_attackChargingTime.Value >= _comboSkillChargeTime.Value && _attackCharging.Value);
    }
    private bool _comboSkillActivateByMan = false;
    public bool ComboSkillActivateByMan
    {
        get => _comboSkillActivateByMan;
    }

    [SerializeField]
    private ProgressBar _comboChargeProgress;
    // ==== Combat ====

    // ==== Camera ====
    [SerializeField] CameraManager cam;
    // ==== Camera ====

    override protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //playerCollider = GetComponent<CapsuleCollider>();
        grabPoint.setPlayer(this);
        grabbedPoint.setPlayer(this);
        state = State.Grounded;
        keyboardInputController = new KeyboardInputController();

        if (playerNum == 1)
        {
            tools.Add(new Pickaxe(this));
            tools.Add(new Boomerang(this));
        }
        else if (playerNum == 2)
        {
            tools.Add(new Shield(this));
            tools.Add(new FlashBomb(this));
            tools.Add(new LightSaber(this));
        }

        distToGround = playerCollider.bounds.extents.y;
        _attackCharging.Value = false;
        _attackChargingTime.Value = 0;
        if(_comboChargeProgress != null)
        {
            _comboChargeProgress.Setup(_attackCharging, _comboSkillChargeTime, _attackChargingTime);
        }
    }

    override protected void Update()
    {
        // ==== for testing (wave tool) ====
        if (beGrabbed && Input.GetKeyDown(KeyCode.Tab)) {
            if (!toolWave)
                SetToolWave(new Vector3(0, 90f, -120f), 1f, true);
            else
                ResetToolWave();
        }
        // ==== for testing (wave tool) ====
        if (!isTool)
        {
            _comboSkillActivateByMan = anotherPlayer.ComboSkillCharged && keyboardInputController.JumpOrAttack(playerNum);
            if (combat.Movable)
            {
                ManageMovement();
                UpdateState();
            }
        }

        else // Tool
        {
            if (IsGrabbed())
            {
                // Combo Skill
                if (ComboSkillCharged && anotherPlayer.ComboSkillActivateByMan)
                {
                    ComboSkillAttack();
                    _attackCharging.Value = false;
                }
                else
                { 
                    // Normal Attack
                    if (keyboardInputController.JumpOrAttack(playerNum))
                    {
                        _attackCharging.Value = combat.ComboSkillAvailable();
                        if(!_attackCharging.Value) Attack();
                        _attackChargingTime.Value = 0;
                    }
                    else
                    {
                        if(_attackCharging.Value && combat.ComboSkillAvailable())
                        {
                            if (keyboardInputController.JumpOrAttackHolding(playerNum))
                            {
                                if (combat.ComboSkillAvailable())
                                { 
                                    _attackChargingTime.Value += Time.deltaTime;
                                }
                                else
                                {
                                    _attackCharging.Value = false;
                                    _attackChargingTime.Value = 0;
                                }
                            }
                            else
                            {
                                if(_attackChargingTime.Value <= 0.5f)
                                {
                                    Attack();
                                }
                                _attackCharging.Value = false;
                                _attackChargingTime.Value = 0;
                            }
                        }
                    }
                }
            }

        }

        // ==== Select Tool && [Man <-> Tool] ====
        if (toolListUI.canChoose() && keyboardInputController.NextTool(playerNum))
        {
            toolListUI.Next();
        }
        if (toolListUI.canChoose() && keyboardInputController.PrevTool(playerNum))
        {
            toolListUI.Previous();
        }
        if (keyboardInputController.Choose(playerNum))
        {
            ToolableManTransform();
        }
        if (!isTool && keyboardInputController.GrabOrRelease(playerNum))
        {
            GrabOrRelease();
        }
        // ==== Select Tool && [Man <-> Tool] ====
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * horizontal * Time.deltaTime);
        transform.position += vertical * transform.forward * speed * Time.deltaTime;
        if (!isTool && confJ!= null)
        {
            confJ.anchor = rightHand.transform.localPosition;
        }
        if (beGrabbed)
        {
            beGrabbedMovement();
        }
    }


    // ==== getters ====
    public Rigidbody getRigidbody()
    {
        return rb;
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public GrabPoint GetGrabPoint()
    {
        return grabPoint;
    }
    public GameObject GetRightHand()
    {
        return rightHand;
    }
    public LayerMask GetLayerMask()
    {
        return playerLayerMask;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public bool IsGrabbing()
    {
        return grabPoint.IsGrabbing();
    }
    // ==== getters ====
}
