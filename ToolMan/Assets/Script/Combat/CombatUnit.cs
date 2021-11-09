using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;



public class CombatUnit : MonoBehaviour
{
    public bool CanAttack;
    public bool CanBeHurt;
    public bool CanMove;

    public int MaxHealth;
    public HealthBar healthBar;
    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Max(0, value);
            _health = Mathf.Min(MaxHealth, _health);
            // change health bar
            if(healthBar != null) healthBar.SetHealth(_health);
            if (_health <= 0) Die();
        }

    }

    public int Atk;

    // for skill cooldown
    protected float _timeToNextAttack;
    protected bool _hasAttacked;
    public bool HasAttacked
    {
        get => _hasAttacked;
        set
        {
            _hasAttacked = value;
        }
    }

    // skills
    public Animator Anim;
    public LayerMask TargetLayers;

    public AvailableSkillSet availableSkillSet;
    public List<string> CurrentUsingSkillSet;
    [SerializeField] private string currentUsingSkillName;
    private Skill currentUsingSkill;
    private bool isUsingSkill;

    private Coroutine skillPerforming = null;

    private void Awake()
    {
        if (CurrentUsingSkillSet == null) CurrentUsingSkillSet = new List<string>();   
    }

    protected virtual void Start()
    {
        if(healthBar != null) healthBar.SetMaxHealth(MaxHealth);
        Health = MaxHealth;

        _hasAttacked = false;
        isUsingSkill = true;
        CanAttack = true;
        CanBeHurt = true;
        CanMove = true;

        if (currentUsingSkillName != null && currentUsingSkillName.Length > 0) SetCurrentUsingSkill(currentUsingSkillName);
        else if (CurrentUsingSkillSet.Count > 0) SetCurrentUsingSkill(CurrentUsingSkillSet[0]);
        else SetCurrentUsingSkill(null);

    }

    public virtual void Attack()
    {
        if (!isUsingSkill) return;
        if (HasAttacked) return;
        HasAttacked = true;

        skillPerforming = StartCoroutine(PerformSkill());

    }

    private IEnumerator PerformSkill()
    {
        yield return StartCoroutine(currentUsingSkill.Attack(Anim, TargetLayers, this, Atk));
        yield return new WaitForSeconds(currentUsingSkill.attackInterval);
        HasAttacked = false;
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
            isUsingSkill = false;
        }
        else
        {
            if (CurrentUsingSkillSet.Contains(skillName))
            {
                currentUsingSkill = availableSkillSet.GetSkillbyName(currentUsingSkillName);
                isUsingSkill = true;
            }
            else
            {
                Debug.Log("Unable use skill " + skillName + ", set current skill to null");
                isUsingSkill = false;
            }
        }
    }

    private void InterruptAttack()
    {
        Debug.Log("coroutine stopped");
        StopCoroutine(skillPerforming);
        skillPerforming = null;
        HasAttacked = false;
        // animation
    }

    public virtual void TakeDamage(int dmg)
    {
        if (!CanBeHurt) return;
        Debug.LogFormat("[{0}] Took {1} Damage", name, dmg);

        if (skillPerforming != null) InterruptAttack();
        Health -= dmg;
    }

    public virtual void Healed(int heal)
    {
        Debug.LogFormat("[{0}] Healed {1}", name, heal);
        Health += heal;
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
