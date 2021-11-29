using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ToolMan.Combat.Skills;
using ToolMan.Combat.Stats;
using ToolMan.Combat.Stats.Buff;
using System.Linq;

namespace ToolMan.Combat
{
    public class SkillCombat : CombatUnit
    {
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

        public bool ColliderEnable = false;
        protected float _hitRefractoryPeriod;

        protected Dictionary<CombatUnit, float> _refractoryPeriod = new Dictionary<CombatUnit, float>();

         protected override void Start()
        {
            base.Start();
            if(availableSkillSet != null)
            {
                availableSkillSet.CheckStatsExsistence(this);
                SetSkills(InitUsingSkillSet);
                if (CurrentUsingSkillSet.Count > 0) SetCurrentUsingSkill(CurrentUsingSkillSet[0]);
                else SetCurrentUsingSkill(null);
            }
            else
            {
                currentUsingSkill = null;
            }
        }

        protected override void Update()
        {
            base.Update();
            lock (_refractoryPeriod)
            {
                if (_refractoryPeriod.Count > 0)
                {
                    foreach (CombatUnit c in _refractoryPeriod.Keys.ToList())
                    {
                        _refractoryPeriod[c] -= Time.deltaTime;
                        if (_refractoryPeriod[c] <= 0)
                        {
                            _refractoryPeriod.Remove(c);
                        }
                    }
                }
            }
        }

        public override void Attack()
        {
            if (!AttackEnabled) return;
            if (!_hasSkillToUse) return;
            if (Attacking) return;

            skillPerforming = StartCoroutine(PerformSkill());

        }

        private IEnumerator PerformSkill()
        {
            _hitRefractoryPeriod = currentUsingSkill.RefractoryPeriod;
            yield return StartCoroutine(currentUsingSkill.Attack(Anim, TargetLayers, this));
            yield return new WaitForSeconds(currentUsingSkill.attackInterval / Aspd);
            _refractoryPeriod.Clear();
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

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            if (!Vulnerable) return 0;

            base.TakeDamage(baseDmg, damager);
            // compute complete damage and take damage
            float typeEffectedDmg = damageCalculator.CalculateDmg(baseDmg, damager.GetCurrentTypes(), this.GetCurrentTypes());
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

        protected override void Die()
        {
            Debug.Log(name + " dies");
            Destroy(gameObject);
        }

        // for attack delay
        protected IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!ColliderEnable) return;
            CombatUnit target = other.gameObject.GetComponent<CombatUnit>();
            if (target == null) return;
            if (_refractoryPeriod.ContainsKey(target)) return;
            Hit(target);
        }

        protected virtual void Hit(CombatUnit target)
        {
            target.TakeDamage(Atk, this);

            _refractoryPeriod.Add(target, _hitRefractoryPeriod);
        }
    }
}