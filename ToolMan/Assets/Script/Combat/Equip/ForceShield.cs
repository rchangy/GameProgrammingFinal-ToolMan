using UnityEngine;
using System.Collections.Generic;
using ToolMan.Combat.Stats;

namespace ToolMan.Combat.Equip
{
    public class ForceShield : CombatUnit
    {
        [SerializeField] string[] _collisionTag;
        float hitTime;
        private Material _mat;

        private bool _init = false;


        private CombatUnit _target;

        private object _source;


        Dictionary<CombatUnit, ContactPoint[]> _contactPoints = new Dictionary<CombatUnit, ContactPoint[]>();

        //protected override void Awake()
        //{
        //    manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        //    damageCalculator = manager.Model.DmgCalculator;
        //    _stats = GetComponent<CharacterStats>();
        //    _atk = _stats.AddStat(new Stat("ATK", AtkBaseValue));
        //    _aspd = _stats.AddStat(new Stat("ASPD", 1));
        //    _def = _stats.AddStat(new Stat("DEF", DefBaseValue));
        //    _str = _stats.AddStat(new Stat("STR", StrBaseValue));
        //    _spd = _stats.AddStat(new Stat("SPD", SpdBaseValue));

        //    _hp = _stats.AddResource(new Resource("HP", HpMaxValue, HpInitValue));

        //    _attackEnabled = _stats.AddAbility(new Ability("AttackEnabled", AttackEnableBaseValue));
        //    _movable = _stats.AddAbility(new Ability("Movable", MovableBaseValue));
        //    _vulnerable = _stats.AddAbility(new Ability("Vulnerable", VulnerableBaseValue));
        //}

        protected override void Start()
        {
            base.Start();
            _stats.AddType("Shield");
        }

        public bool SetBy(object obj)
        {
            return obj == _source;
        }

        public void Setup(Material mat, CombatUnit target, object src)
        {
            _mat = mat;
            _target = target;
            _target.Disable("Vulnerable");
            GetComponent<Renderer>().sharedMaterial = mat;
            _source = src;
        }

        public void Reset()
        {
            _stats.GetResourceByName("HP").Reset();
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            var dmg = base.TakeDamage(baseDmg, damager);
            if (_contactPoints.ContainsKey(damager))
            {
                ContactPoint[] contacts = _contactPoints[damager];
                for (int i2 = 0; i2 < contacts.Length; i2++)
                {
                    _mat.SetVector("_HitPosition", transform.InverseTransformPoint(contacts[i2].point));
                    hitTime = 500;
                    _mat.SetFloat("_HitTime", hitTime);
                }
            }

            return dmg;
        }

        private void OnCollisionEnter(Collision collision)
        {
            CombatUnit combat = collision.gameObject.GetComponent<CombatUnit>();
            if (combat == null) return;

            if (_contactPoints.ContainsKey(combat))
            {
                _contactPoints[combat] = collision.contacts;
            }
            else
            {
                _contactPoints.Add(combat, collision.contacts);
            }
        }

        public override bool Attack()
        {
            Debug.Log("Shield cannot attack");
            return false;
        }

        protected override void Die()
        {
            Debug.Log("Shield Finish");
            _target.RemoveDisable("Vulnerable");
            Destroy(gameObject);
        }

    }
}

