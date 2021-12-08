using UnityEngine;
using System.Collections.Generic;
using ToolMan.Combat.Stats.Buff;


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
        [SerializeField] private ScriptableBuff _buff;


        private object _source;

        private bool _isSet = false;


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
            if (_isSet)
                _target.AddBuff(_buff);
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
            _isSet = true;
        }

        public void Init(int maxHp, float def)
        {
            _maxHp = maxHp;
            _defValue = def;
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            var dmg = 0;
            //var dmg = base.TakeDamage(5, damager);
            if (damager.IsType("LightSaber"))
            {
                _hp.ChangeValueBy(-10);
                dmg = 10;
            }
            else
            {
                _hp.ChangeValueBy(-3);
                dmg = 3;
            }
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
            Destroy(gameObject);
        }

    }
}

