﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ToolMan.Combat.Skills;
using System.Linq;
using ToolMan.Util;

namespace ToolMan.Combat
{
    public class SkillCombat : CombatUnit
    {
        // skills
        [SerializeField]
        private LayerMask _targetLayers;
        public LayerMask TargetLayers
        {
            get => _targetLayers;
        }
        public SkillSet availableSkillSet;
        private List<string> CurrentUsingSkillSet;
        public List<string> InitUsingSkillSet;
        protected Skill currentUsingSkill;
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

        protected Coroutine _skillCoroutine = null;

        public bool Attacking
        {
            get =>  _skillCoroutine != null;
        }

        protected Dictionary<string, float> _skillCd = new Dictionary<string, float>();

        protected BoolWrapper _collisionEnable = new BoolWrapper();

        public bool CollisionEnable
        {
            get => _collisionEnable.Value;
        }
        protected float _hitRefractoryPeriod;

        protected Dictionary<CombatUnit, float> _refractoryPeriod = new Dictionary<CombatUnit, float>();

        protected override void Awake()
        {
            base.Awake();
            if (availableSkillSet != null)
            {
                availableSkillSet.Load();
                SetSkills(InitUsingSkillSet);
                if (CurrentUsingSkillSet.Count > 0) SetCurrentUsingSkill(CurrentUsingSkillSet[0]);
                else SetCurrentUsingSkill(null);
            }
            else
            {
                currentUsingSkill = null;
                CurrentUsingSkillSet = null;
            }
        }
        protected override void Update()
        {
            base.Update();

            if (Attacking)
            {
                // during attack
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
            if (!_hasSkillToUse) return false;
            if (Attacking) return false;
            if(_skillCd[currentUsingSkillName] <= 0)
            {
                _skillCoroutine = StartCoroutine(PerformSkill());
                return true;
            }
            return false;

        }

        private IEnumerator PerformSkill()
        {
            _hitRefractoryPeriod = currentUsingSkill.RefractoryPeriod;
            yield return StartCoroutine(currentUsingSkill.Attack(this, _collisionEnable));
            SkillFinish();
        }

        // set skills that can be selected during battle
        public void SetSkills(List<string> skillNames)
        {
            if(Attacking)
                InterruptAttack();
            if (skillNames == null || skillNames.Count == 0)
            {
                skillNames = availableSkillSet.GetSkills();
            }
            CurrentUsingSkillSet = new List<string>();
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
            StopCoroutine(_skillCoroutine);
            SkillFinish();
        }

        private void SkillFinish()
        {
            _collisionEnable.Value = false;
            _refractoryPeriod.Clear();
            _skillCd[currentUsingSkill.getName()] = currentUsingSkill.Cd;
            _skillCoroutine = null;
        }

        protected override void Interrupted(CombatUnit damager)
        {
            base.Interrupted(damager);
            if (Attacking) InterruptAttack();
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != Converter.LayerBitMaskToLayerNumber(TargetLayers)) return;
            if (!CollisionEnable) return;
            CombatUnit target = other.gameObject.GetComponent<CombatUnit>();
            if (target == null) return;
            if (_refractoryPeriod.ContainsKey(target)) return;
            _refractoryPeriod.Add(target, _hitRefractoryPeriod);
            //_hitDir = transform.position - target.transform.position;
            Hit(target);
        }

        protected virtual void Hit(CombatUnit target)
        {
            Debug.Log(name + " hit " + target.name);
            StartCoroutine(currentUsingSkill.Hit(this, target));
            target.TakeDamage(Atk, this);
        }

        public IReadOnlyCollection<String> GetCurrentUsingSkillSet()
        {
            return CurrentUsingSkillSet;
        }

        protected override void Die()
        {
            
        }

    }
}