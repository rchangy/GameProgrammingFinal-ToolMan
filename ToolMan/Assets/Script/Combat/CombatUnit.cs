using UnityEngine;
using ToolMan.Combat.Stats;
using ToolMan.Combat.Stats.Buff;
using System;
using System.Collections.Generic;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat
{
    [RequireComponent(typeof(CharacterStats))]
    public abstract class CombatUnit : MonoBehaviour
    {
        public int HpMaxValue = 1;
        public int HpInitValue = 1;
        public float AtkBaseValue = 1f;
        public float DefBaseValue = 1f;
        public float StrBaseValue = 1f;
        public bool AttackEnableBaseValue = true;
        public bool MovableBaseValue = true;
        public bool VulnerableBaseValue = true;

        protected Resource _hp;
        public int Hp
        {
            get => _hp.Value;
        }
        

        protected Stat _atk;
        public float Atk
        {
            get => _atk.Value;
        }
        protected Stat _aspd;
        public float Aspd
        {
            get => _aspd.Value;
        }
        protected Stat _def;
        public float Def
        {
            get => _def.Value;
        }
        protected Stat _str;
        public float Str
        {
            get => _str.Value;
        }

        protected Ability _attackEnabled;
        public bool AttackEnabled{
            get => _attackEnabled.Value;
        }
        protected Ability _movable;
        public bool Movable
        {
            get => _movable.Value;
        }
        protected Ability _vulnerable;
        public bool Vulnerable
        {
            get => _vulnerable.Value;
        }

        protected CharacterStats _stats;

        protected DamageCalculator damageCalculator;
        protected CombatManager manager;

        // healthBar
        protected int _lastHpValue;
        public HealthBar healthBar;

        protected virtual void Awake()
        {
            manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
            damageCalculator = manager.Model.DmgCalculator;
            _stats = GetComponent<CharacterStats>();
            _atk = _stats.AddStat(new Stat("ATK", AtkBaseValue));
            _aspd = _stats.AddStat(new Stat("ASPD", 1));
            _def = _stats.AddStat(new Stat("DEF", DefBaseValue));
            _str = _stats.AddStat(new Stat("STR", StrBaseValue));

            _hp = _stats.AddResource(new Resource("HP", HpMaxValue, HpInitValue));

            _attackEnabled = _stats.AddAbility(new Ability("AttackEnabled", AttackEnableBaseValue));
            _movable = _stats.AddAbility(new Ability("Movable", MovableBaseValue));
            _vulnerable = _stats.AddAbility(new Ability("Vulnerable", VulnerableBaseValue));
        }

        protected virtual void Start()
        {
            if (healthBar != null)
            {
                healthBar.Setup(_hp);
            }
            _lastHpValue = _hp.Value;
        }


        protected virtual void Update()
        {
            Debug.Log(name + " Hp: " + Hp);
            if(Hp <= 0)
            {
                Die();
            }
        }


        public abstract bool Attack();

        public virtual int TakeDamage(float baseDmg, CombatUnit damager)
        {
            if (!Vulnerable) return 0;
            float typeEffectedDmg = damageCalculator.CalculateDmg(baseDmg, damager.GetCurrentTypes(), this.GetCurrentTypes());
            float dmg = typeEffectedDmg - Def;
            dmg = Mathf.Max(dmg, 0);
            _hp.ChangeValueBy(-(int)dmg);
            Debug.Log(name + " takes " + dmg + " damage, Hp: " + Hp);

            return (int)dmg;
        }

        protected abstract void Die();

        public void AddBuff(ScriptableBuff buff)
        {
            buff.AddBuff(_stats);
        }

        public void AddType(string type)
        {
            _stats.AddType(type);
        }
        public void RemoveType(string type)
        {
            _stats.RemoveType(type);
        }
#nullable enable
        public float? GetStatValue(String name)
        {
            if (_stats.HasStat(name))
            {
                return _stats.GetStatByName(name).Value;
            }
            return null;
        }
        public float? GetStatBaseValue(String name)
        {
            if (_stats.HasStat(name))
            {
                return _stats.GetStatByName(name).BaseValue;
            }
            return null;
        }
        public float? GetResourceValue(String name)
        {
            if (_stats.HasResource(name))
            {
                return _stats.GetResourceByName(name).Value;
            }
            return null;
        }
        public float? GetResourceMaxValue(String name)
        {
            if (_stats.HasResource(name))
            {
                return _stats.GetResourceByName(name).MaxValue;
            }
            return null;
        }
        public bool? GetAbilityState(String name)
        {
            if (_stats.HasAbility(name))
            {
                return _stats.GetAbilityByName(name).Value;
            }
            return null;
        }
        public bool? GetAbilityInitState(String name)
        {
            if (_stats.HasAbility(name))
            {
                return _stats.GetAbilityByName(name).BaseValue;
            }
            return null;
        }
#nullable disable
        public IReadOnlyCollection<string> GetCurrentTypes()
        {
            return _stats.GetCurrentTypes();
        }
        public bool IsType(string type)
        {
            return _stats.IsType(type);
        }
    }
}
