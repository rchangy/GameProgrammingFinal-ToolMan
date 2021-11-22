using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ToolMan.Combat.Skills;
using ToolMan.Combat.Stats;
using ToolMan.Combat.Stats.Buff;

namespace ToolMan.Combat
{
    public class CombatUnit : MonoBehaviour
    {
        protected CharacterStats stats;
        private Resource _hp;
        public int Hp
        {
            get => _hp.Value;
        }

        private Stat _aspd;
        public float Aspd
        {
            get => _aspd.Value;
        }
        private Stat _atk;
        public float Atk
        {
            get => _atk.Value;
        }
        private Stat _def;
        public float Def
        {
            get => _def.Value;
        }
        private Stat _str;
        public float Str
        {
            get => _str.Value;
        }

        protected Ability _attackEnabled;
        public bool AttackEnabled
        {
            get => _attackEnabled.Value;
        }
        protected Ability _vulnerable;
        public bool Vulnerable
        {
            get => _vulnerable.Value;
        }
        protected Ability _movable;
        public bool Movable
        {
            get => _movable.Value;
        }

        [SerializeField]
        private DamageCalculator damageCalculator;

        // healthBar
        private int _lastHpValue;
        public HealthBar healthBar;

        // skills
        public Animator Anim;
        public LayerMask TargetLayers;

        public SkillSet availableSkillSet;
        private List<string> CurrentUsingSkillSet;
        public List<string> InitUsingSkillSet;
        private Skill currentUsingSkill;
        public string currentUsingSkillName
        {
            get
            {
                if (currentUsingSkill == null) return "";
                return currentUsingSkill.getName();
            }

        }
        protected bool _hasSkillToUse
        {
            get => currentUsingSkill != null;
        }

        protected Coroutine skillPerforming = null;

        public bool Attacking
        {
            get => skillPerforming != null;
        }

         protected virtual void Start()
        {
            stats = gameObject.GetComponent<CharacterStats>();
            _hp = stats.GetResourceByName("HP");

            _aspd = stats.GetStatByName("ASPD");
            _atk = stats.GetStatByName("ATK");
            _def = stats.GetStatByName("DEF");
            _str = stats.GetStatByName("STR");

            _attackEnabled = stats.GetAbilityByName("AttackEnabled");
            _movable = stats.GetAbilityByName("Movable");
            _vulnerable = stats.GetAbilityByName("Vulnerable");

            if (healthBar != null)
            {
                healthBar.SetMaxHealth(_hp.MaxValue);
                healthBar.SetHealth(_hp.Value);
            }
            _lastHpValue = _hp.Value;

            if (stats == null)
            {
                Debug.Log(gameObject.name + " has no stats component");
            }

            availableSkillSet.CheckStatsExsistence(this);


            SetSkills(InitUsingSkillSet);
            if (CurrentUsingSkillSet.Count > 0) SetCurrentUsingSkill(CurrentUsingSkillSet[0]);
            else SetCurrentUsingSkill(null);

            damageCalculator.checkLoad();
        }

        private void Update()
        {
            if (_lastHpValue != Hp && healthBar != null)
            {
                _lastHpValue = Hp;
                healthBar.SetHealth(Hp);
            }
        }


        public virtual void Attack()
        {
            if (!AttackEnabled) return;
            if (!_hasSkillToUse) return;
            if (Attacking) return;

            skillPerforming = StartCoroutine(PerformSkill());

        }

        private IEnumerator PerformSkill()
        {
            yield return StartCoroutine(currentUsingSkill.Attack(Anim, TargetLayers, this));
            yield return new WaitForSeconds(currentUsingSkill.attackInterval / Aspd);
            skillPerforming = null;

        }

        // set skills that can be selected during battle
        public void SetSkills(List<string> skillNames)
        {
            CurrentUsingSkillSet = new List<string>();
            if (skillNames == null || skillNames.Count == 0) return;
            foreach (string skillName in skillNames)
            {
                if (availableSkillSet.HasSkill(skillName)) CurrentUsingSkillSet.Add(skillName);
            }
        }

        // select a skill to use now
#nullable enable
        public void SetCurrentUsingSkill(string? skillName)
        {
            if (skillName == null)
            {
                Debug.Log("No skill is set to " + gameObject.name);
                currentUsingSkill = null;
            }
            else
            {
                if (CurrentUsingSkillSet.Contains(skillName))
                {
                    currentUsingSkill = availableSkillSet.GetSkillbyName(skillName);
                    currentUsingSkill.SetAttackPoint(transform);
                }
                else
                {
                    Debug.Log("Unable use skill " + skillName + ", set current skill to null");
                    currentUsingSkill = null;
                }
            }
        }
#nullable disable

        private void InterruptAttack()
        {
            Debug.Log(name + " attack interrupted, coroutine stopped");
            StopCoroutine(skillPerforming);
            skillPerforming = null;
        }

        public virtual int TakeDamage(int rawDmg, CombatUnit damager)
        {
            // check if this unit can be hurt now
            if (!Vulnerable) return 0;

            // compute complete damage and take damage
            float typeEffectedDmg = damageCalculator.CalculateDmg(rawDmg, damager.GetCurrentTypes(), this.GetCurrentTypes());
            float dmg = typeEffectedDmg - Def;
            // check strength
            if (dmg > Str)
            {
                // skill interrupt
                if (Attacking) InterruptAttack();
                Anim.SetTrigger("Hurt");
            }
            // set all skill time variables to there init value
            _hp.ChangeValueBy(-(int)dmg);
            Debug.LogFormat("[{0}] Took {1} Damage", name, dmg);
            return (int)dmg;
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

        public void AddBuff(ScriptableBuff buff)
        {
            buff.AddBuff(stats);
        }

#nullable enable
        public float? GetStatValue(String name)
        {
            if (stats.HasStat(name))
            {
                return stats.GetStatByName(name).Value;
            }
            return null;
        }
        public float? GetStatBaseValue(String name)
        {
            if (stats.HasStat(name))
            {
                return stats.GetStatByName(name).BaseValue;
            }
            return null;
        }
        public float? GetResourceValue(String name)
        {
            if (stats.HasResource(name))
            {
                return stats.GetResourceByName(name).Value;
            }
            return null;
        }
        public float? GetResourceMaxValue(String name)
        {
            if (stats.HasResource(name))
            {
                return stats.GetResourceByName(name).MaxValue;
            }
            return null;
        }
        public bool? GetAbilityState(String name)
        {
            if (stats.HasAbility(name))
            {
                return stats.GetAbilityByName(name).Value;
            }
            return null;
        }
        public bool? GetAbilityInitState(String name)
        {
            if (stats.HasAbility(name))
            {
                return stats.GetAbilityByName(name).BaseValue;
            }
            return null;
        }
#nullable disable
        public IReadOnlyCollection<string> GetCurrentTypes()
        {
            return stats.GetCurrentTypes();
        }
    }
}