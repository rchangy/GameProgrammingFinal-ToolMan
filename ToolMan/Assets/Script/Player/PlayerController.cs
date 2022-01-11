using UnityEngine;
using ToolMan.Player;
using ToolMan.Combat;
using ToolMan.Util;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;

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
    public float distToGround;
    private bool isGrounded;

    [SerializeField] private bool changeable = false;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    private float horizontal, vertical;
    private int unlockedToolNum;
    // ==== Player Status ====

    // ==== Components ====
    private Animator animator;
    private Rigidbody rb;
    private ConfigurableJoint confJ = null;

    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private GrabPoint grabPoint;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject Forearm;
    [SerializeField] private PlayerController anotherPlayer;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material EmissionMaterial;
    [SerializeField] private Renderer skinRenderer;
    public GameObject grabHint;
    public EffectController effectController;
    // ==== Components ====

    // ==== Audio ====
    public PlayerAudioStat playerAudioStat;
    public AudioSource audioSource;
    public AudioClip jumpAudio;
    public AudioClip walkAudio;
    public AudioClip toToolAudio;
    public AudioClip hurtAudio;
    public AudioClip addHPAudio;
    public AudioClip pickaxeHitAudio;
    public AudioClip lightSaberHitAudio;
    public AudioClip boomerangWhirlAudio;
    public AudioClip flashBombAttackAudio;
    public AudioClip shieldDefendAudio;
    public AudioClip unlockToolAudio;
    // ==== Audio ====

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

    /// === WaterMat === 
    [SerializeField] private BearManWaterMatSwitcher _waterMatSwitcher;
    /// === WaterMat === 


    // ==== Camera ====
    [SerializeField] CameraManager cam;
    // ==== Camera ====

    // ==== Debug ====
    RaycastHit m_Hit;
    float m_MaxDistance;
    bool m_HitDetect;
    // ==== Debug ====
    protected override void Awake()
    {
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        keyboardInputController = new KeyboardInputController();
        GetMaterial().DisableKeyword("_EMISSION");
        playerAudioStat = new PlayerAudioStat();
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
        if (isDead)
            return;
        if (!controlEnable)
        {
            ManageMovement();
            UpdateState();
            return;
        }
        playerAudioStat.update();
        if (!isTool.Value)
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
        if (!isTool.Value && keyboardInputController.GrabOrRelease(playerNum))
        {
            GrabOrRelease();
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        if (beGrabbed)
        {
            beGrabbedMovement();
            return;
        }

        // Movement
        transform.Rotate(Vector3.up * horizontal * Time.deltaTime);
        float moveDis = vertical * speed * Time.deltaTime * combat.Spd;
        if (Mathf.Abs(moveDis) > 0)
        {
            m_MaxDistance = moveDis;
            m_HitDetect = Physics.BoxCast(playerCollider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, moveDis);
            if (!m_HitDetect || m_Hit.collider.gameObject.CompareTag("Player") || m_Hit.collider.isTrigger)
            {
                transform.position += moveDis * transform.forward;
                if (playerAudioStat.lastWalkTime > 0.4f)
                    Schedule<PlayerWalking>().player = this;
            }
            //else
            //{
            //    Debug.Log("Hit : " + m_Hit.collider.name);
            //}
        }
        if (!isTool.Value && confJ != null)
        {
            confJ.anchor = rightHand.transform.localPosition;
        }
    }

    private void CheckToolSelecting()
    {
        if (keyboardInputController.NextTool(playerNum))
        {
            toolIdx.Value++;
            if (toolIdx.Value >= unlockedToolNum) toolIdx.Value = 0;
            if (isTool.Value)
            {
                ToAnotherTool();
            }
        }
        if (keyboardInputController.PrevTool(playerNum))
        {
            toolIdx.Value--;
            if (toolIdx.Value < 0) toolIdx.Value = unlockedToolNum - 1;
            if (isTool.Value)
            {
                ToAnotherTool();
            }
            //toolListUI.Previous();
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

    public Collider GetCollider()
    {
        return playerCollider;
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
            if (_waterMatSwitcher != null && _waterMatSwitcher.InWater)
                _waterMatSwitcher.SetWaterMat();
            else
                skinRenderer.material = playerMaterial;
        }
    }
    public void LoadTool(int toolNum)
    {
        unlockedToolNum = toolNum;
        toolListUI.LoadTool(toolNum);
        SetUpToolList();
    }
    public void UnlockTool(int level, int toolNum)
    {
        if (toolNum > unlockedToolNum)
        {
            unlockedToolNum = toolNum;
            AnimationUnlock(level);
            if (audioSource && unlockToolAudio)
                audioSource.PlayOneShot(unlockToolAudio);
        }
    }
    public void ResetToIdle()
    {
        controlEnable = false;
        if (isTool.Value && IsGrabbed())
        {
            anotherPlayer.Release();
        }
        if (isTool.Value)
        {
            ToolableManTransform();
        }
    }
    // ==== getters ====

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        }
    }
}
