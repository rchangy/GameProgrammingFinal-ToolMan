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

        protected Dictionary<string, float> _skillCd = new Dictionary<string, float>();

        public bool ColliderEnable = false;
        protected float _hitRefractoryPeriod;

        protected Dictionary<CombatUnit, float> _refractoryPeriod = new Dictionary<CombatUnit, float>();

        protected override void Start()
        {
            base.Start();
            if (availableSkillSet != null)
            {
                availableSkillSet.CheckStatsExsistence(this);
                SetSkills(InitUsingSkillSet);
                if (CurrentUsingSkillSet.Count > 0) SetCurrentUsingSkill(CurrentUsingSkillSet[0]);
                else SetCurrentUsingSkill(null);
                Debug.Log(GetCurrentUsingSkillSet());
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
            lock (_skillCd)
            {
                if(_skillCd.Count > 0)
                {
                    foreach(string skillName in _skillCd.Keys.ToList())
                    {
                        if(_skillCd[skillName] > 0)
                            _skillCd[skillName] -= Time.deltaTime;
                    }
                }
            }
        }

        public override bool Attack()
        {

            if (!AttackEnabled) return false;
            Debug.Log("Attack");
            if (!_hasSkillToUse) return false;
            Debug.Log("Attack");
            if (Attacking) return false;
            Debug.Log("Attack");
            if(_skillCd[currentUsingSkillName] <= 0)
            { 
                skillPerforming = StartCoroutine(PerformSkill());
                return true;
            }
            return false;

        }

        private IEnumerator PerformSkill()
        {
            _hitRefractoryPeriod = currentUsingSkill.RefractoryPeriod;
            Debug.Log("using skill: " + currentUsingSkill.getName());
            yield return StartCoroutine(currentUsingSkill.Attack(Anim, TargetLayers, this));
            yield return new WaitForSeconds(currentUsingSkill.attackInterval / Aspd);
            _refractoryPeriod.Clear();
            _skillCd[currentUsingSkill.getName()] = currentUsingSkill.Cd;
            skillPerforming = null;
        }

        // set skills that can be selected during battle
        public void SetSkills(List<string> skillNames)
        {
            if(Attacking)
                InterruptAttack();
            CurrentUsingSkillSet = new List<string>();
            if (skillNames == null || skillNames.Count == 0) return;
            foreach (string skillName in skillNames)
            {
                if (availableSkillSet.HasSkill(skillName))
                {
                    _skillCd.Add(skillName, 0f);
                    CurrentUsingSkillSet.Add(skillName);
                }
            }
        }

        // select a skill to use now
#nullable enable
        public virtual bool SetCurrentUsingSkill(string? skillName)
        {
            if (Attacking) return false;
            if (skillName == null)
            {
                Debug.Log("No skill is set to " + gameObject.name);
                currentUsingSkill = null;
                return false;
            }
            else
            {
                if (CurrentUsingSkillSet.Contains(skillName))
                {
                    
                    currentUsingSkill = availableSkillSet.GetSkillbyName(skillName);
                    currentUsingSkill.SetAttackPoint(transform);
                    return true;
                }
                else
                {
                    Debug.Log("Unable use skill " + skillName + ", set current skill to null");
                    currentUsingSkill = null;
                    return false;
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
            var dmg = base.TakeDamage(baseDmg, damager);
            // check strength
            if (dmg > Str)
            {
                // skill interrupt
                if (Attacking) InterruptAttack();
                Anim.SetTrigger("Hurt");
            }
            return (int)dmg;
        }

        protected override void Die()
        {
            Debug.Log(name + " dies");
            Destroy(gameObject);
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

        public IReadOnlyCollection<String> GetCurrentUsingSkillSet()
        {
            return CurrentUsingSkillSet;
        }
    }
}