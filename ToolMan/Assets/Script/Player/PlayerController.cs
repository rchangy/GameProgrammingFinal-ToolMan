using UnityEngine;
using ToolMan.Player;
using ToolMan.Combat;
using ToolMan.Util;

[RequireComponent(typeof(Animator))]
public partial class PlayerController : ToolableMan
{
    /// <summary>
    /// Handle inputs controlling a player.
    /// </summary>
    GameManager gameManager;
    private KeyboardInputController keyboardInputController;

    // ==== Player Status ====
    public bool controlEnable;
    public int playerNum = 1; // player 1 or player 2
    [SerializeField] private bool isDead = false;

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
    [SerializeField] private GameObject Forearm;
    [SerializeField] private ObjectListUI toolListUI;
    [SerializeField] private PlayerController anotherPlayer;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material EmissionMaterial;
    [SerializeField] private Renderer skinRenderer;
    public GameObject grabHint;
    public EffectController effectController;
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
    protected override void Awake()
    {
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        keyboardInputController = new KeyboardInputController();
        GetMaterial().DisableKeyword("_EMISSION");
    }
    override protected void Start()
    {
        grabPoint.setPlayer(this);

        if (playerNum == 1)
        {
            tools.Add(new Pickaxe(this));
            tools.Add(new Boomerang(this));
        }
        else if (playerNum == 2)
        {
            tools.Add(new FlashBomb(this));
            tools.Add(new LightSaber(this));
            tools.Add(new Shield(this));
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
        if (!controlEnable)
        {
            return;
        }
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

        // Select Tool
        CheckToolSelecting();

        // Grab Or Release
        if (!isTool && keyboardInputController.GrabOrRelease(playerNum))
        {
            GrabOrRelease();
        }
    }

    private void FixedUpdate()
    {
        if (!controlEnable)
        {
            return;
        }
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

    private void CheckToolSelecting()
    {
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
    public GameObject GetForearm()
    {
        return Forearm;
    }
    public LayerMask GetLayerMask()
    {
        return playerLayerMask;
    }
    public Material GetMaterial()
    {
        return playerMaterial;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public bool IsGrabbing()
    {
        return grabPoint.IsGrabbing();
    }
    public PlayerController GetAnotherPlayer()
    {
        return anotherPlayer;
    }
    public bool IsAnimationAttacking()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }
    public void setEmission(bool emission)
    {
        if (emission && EmissionMaterial != null)
        {
            skinRenderer.material = EmissionMaterial;
        }
        else if (!emission)
        {
            skinRenderer.material = playerMaterial;
        }
    }
    public void UnlockTool(int toolNum)
    {
        toolListUI.UnLockTool(toolNum);
    }
    // ==== getters ====
}
