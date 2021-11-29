using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat.Equip
{
    public class Shield : CombatUnit
    {
        [SerializeField] string[] _collisionTag;
        float hitTime;
        Material mat = null;

        Dictionary<CombatUnit, ContactPoint[]> _contactPoints = new Dictionary<CombatUnit, ContactPoint[]>();

        protected override void Start()
        {
            base.Start();
            if (GetComponent<Renderer>())
            {
                mat = GetComponent<Renderer>().sharedMaterial;
            }
            if(mat == null)
            {
                Debug.Log("Shield has no mat set");
            }
            _stats.AddType("Shield");
        }

        public void Init(int maxHp, float def)
        {
            _hp.MaxValue = maxHp;
            
            _hp.Reset();
            _def.BaseValue = def;

        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            var dmg = base.TakeDamage(baseDmg, damager);

            if (_contactPoints.ContainsKey(damager))
            {
                ContactPoint[] contacts = _contactPoints[damager];
                for (int i2 = 0; i2 < contacts.Length; i2++)
                {
                    mat.SetVector("_HitPosition", transform.InverseTransformPoint(contacts[i2].point));
                    hitTime = 500;
                    mat.SetFloat("_HitTime", hitTime);
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

        public override void Attack()
        {
            Debug.Log("Shield cannot attack");
        }

        protected override void Die()
        {
            Debug.Log("Shield Finish");
            Destroy(this);
        }

    }
}

