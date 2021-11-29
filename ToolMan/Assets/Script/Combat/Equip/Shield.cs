using UnityEngine;
using System.Collections;

namespace ToolMan.Combat.Equip
{
    public class Shield : CombatUnit
    {
        [SerializeField] string[] _collisionTag;
        float hitTime;
        Material mat;

        private ContactPoint[] _contacts;



        protected override void Start()
        {
            base.Start();
            if (GetComponent<Renderer>())
            {
                mat = GetComponent<Renderer>().sharedMaterial;
            }

        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            if (Vulnerable) return 0;

            float typeEffectedDmg = damageCalculator.CalculateDmg(baseDmg, damager.GetCurrentTypes(), this.GetCurrentTypes());
            float dmg = typeEffectedDmg - Def;

            for (int i2 = 0; i2 < _contacts.Length; i2++)
            {
                mat.SetVector("_HitPosition", transform.InverseTransformPoint(_contacts[i2].point));
                hitTime = 500;
                mat.SetFloat("_HitTime", hitTime);
            }
            return 0;

        }

        private void OnCollisionEnter(Collision collision)
        {
            _contacts = collision.contacts;
        }

        public override void Attack()
        {
            Debug.Log("Shield cannot attack");
        }

        protected override void Die()
        {
            Debug.Log("Shield Finish");
        }

    }
}

