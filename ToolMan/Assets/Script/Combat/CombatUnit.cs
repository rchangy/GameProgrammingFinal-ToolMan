using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class CombatUnit : MonoBehaviour
{
    public bool isPlayer;

    private CharacterStats stats;
    private Resource _hp;
    private Stat _attackSpeed;
    private Stat _atk;

    private Ability _canAttack;
    private Ability _canBeHurt;
    private Ability _canMove;

    public bool CanAttack
    {
        get => _canAttack.Value;
    }
    public bool CanBeHurt
    {
        get => _canBeHurt.Value;
    }
    public bool CanMove
    {
        get => _canMove.Value;
    }
    

    private int _lastHpValue;

    public HealthBar healthBar;

    public PlayerController playerController;


    // skills
    public Animator Anim;
    public LayerMask TargetLayers;

    public AvailableSkillSet availableSkillSet;
    public List<string> CurrentUsingSkillSet;
    [SerializeField] private string currentUsingSkillName;
    private Skill currentUsingSkill;
    private bool _hasSkillToUse;

    private Coroutine skillPerforming = null;

    // for skill cooldown
    protected float _timeToNextAttack;
    private bool _hasAttacked;

    private void Awake()
    {
        if (CurrentUsingSkillSet == null) CurrentUsingSkillSet = new List<string>();

    }

    private void Update()
    {
        if(_lastHpValue != _hp.Value)
        {
            _lastHpValue = _hp.Value;
            healthBar.SetHealth(_hp.Value);
        }
    }

    protected virtual void Start()
    {
        _hp = stats.GetResourceByName("hp");
        if (_hp == null) Debug.Log(gameObject.name + " has no resource hp");
        _attackSpeed = stats.GetStatByName("attack speed");
        if (_attackSpeed == null) Debug.Log(gameObject.name + " has no stat attack speed");
        _atk = stats.GetStatByName("atk");
        if (_atk == null) Debug.Log(gameObject.name + " has no stat atk");


        if (healthBar != null)
        {
            healthBar.SetMaxHealth(_hp.MaxValue);
            healthBar.SetHealth(_hp.Value);
            _lastHpValue = _hp.Value;
        }

        _hasAttacked = false;
        _hasSkillToUse = true;
        if ((playerController = gameObject.GetComponent<PlayerController>()) != null) isPlayer = true;
        else isPlayer = false;

        if (currentUsingSkillName != null && currentUsingSkillName.Length > 0) SetCurrentUsingSkill(currentUsingSkillName);
        else if (CurrentUsingSkillSet.Count > 0) SetCurrentUsingSkill(CurrentUsingSkillSet[0]);
        else SetCurrentUsingSkill(null);
    }

    public virtual void Attack()
    {
        if (!_hasSkillToUse) return;
        if (_hasAttacked) return;
        _hasAttacked = true;

        skillPerforming = StartCoroutine(PerformSkill());

    }

    private IEnumerator PerformSkill()
    {
        yield return StartCoroutine(currentUsingSkill.Attack(Anim, TargetLayers, _atk, _attackSpeed, stats));
        yield return new WaitForSeconds(currentUsingSkill.attackInterval / _attackSpeed.Value);
        _hasAttacked = false;
    }

    // set skills that can be selected during battle
    public void SetSkills(string[] skillNames)
    {
        CurrentUsingSkillSet = new List<string>();
        foreach (string skillName in skillNames)
        {
            if (availableSkillSet.HasSkill(skillName)) CurrentUsingSkillSet.Add(skillName);
        }
    }

    // select a skill to use now
#nullable enable
    public void SetCurrentUsingSkill(string? skillName)
    {
        currentUsingSkillName = skillName;
        if (skillName == null)
        {
            Debug.Log("No skill is set to " + gameObject.name);
            _hasSkillToUse = false;
        }
        else
        {
            if (CurrentUsingSkillSet.Contains(skillName))
            {
                currentUsingSkill = availableSkillSet.GetSkillbyName(currentUsingSkillName);
                _hasSkillToUse = true;
            }
            else
            {
                Debug.Log("Unable use skill " + skillName + ", set current skill to null");
                _hasSkillToUse = false;
            }
        }
    }

    private void InterruptAttack()
    {
        Debug.Log("coroutine stopped");
        StopCoroutine(skillPerforming);
        skillPerforming = null;
        _hasAttacked = false;
        // animation
    }


    /**
     * everything that happens after hit by someone
     */
    public virtual void TakeDamage(int rawDmg, CharacterStats attackerStats)
    {
        // check if this unit can be hurt now
        if (!CanBeHurt) return;


        // animation
        Anim.SetTrigger("Hurt");

        // skill interrupt
        if (skillPerforming != null) InterruptAttack();
        // set all skill time variables to there init value

        // check strength

        if (isPlayer)
        {
            playerController.grabPoint.Release();
            if (playerController.isTool) playerController.ToolableManTransform();
        }

        // compute complete damage and take damage
        // compute TODO
        _hp.ChangeValueBy(-rawDmg);
        Debug.LogFormat("[{0}] Took {1} Damage", name, rawDmg);
    }

    public virtual void Die()
    {
        Debug.Log(name + " dies");
        Destroy(gameObject);
    }

    // for attack delay
    protected IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}