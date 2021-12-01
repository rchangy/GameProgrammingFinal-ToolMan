using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat.Equip
{
    public class ForceShield : CombatUnit
    {
        [SerializeField] string[] _collisionTag;
        float hitTime;
        private Material _mat;

        private bool _init = false;

        private int _maxHp;
        private float _defValue;

        private CombatUnit _target;

        private object _source;


        Dictionary<CombatUnit, ContactPoint[]> _contactPoints = new Dictionary<CombatUnit, ContactPoint[]>();

        protected override void Start()
        {
            base.Start();
            _stats.AddType("Shield");
        }

        protected override void Update()
        {
            base.Update();
            if (_init && _hp != null && _def != null)
            {
                _hp.Reset(_maxHp, _maxHp);
                _def.BaseValue = _defValue;
                _init = false;
            }
        }

        public bool SetBy(object obj)
        {
            return obj == _source;
        }

        public void Setup(Material mat, CombatUnit target, object src)
        {
            _mat = mat;
            _target = target;
            GetComponent<Renderer>().sharedMaterial = mat;
            _source = src;
        }

        public void Init(int maxHp, float def)
        {
            _maxHp = maxHp;
            _defValue = def;
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
            return (int)dmg;
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
            Destroy(this);
        }

    }
}

