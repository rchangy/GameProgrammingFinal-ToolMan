using UnityEngine;
using System.Collections.Generic;
using ToolMan.Combat.Stats;

namespace ToolMan.Combat.Equip
{
    public class ForceShield : CombatUnit
    {
        [SerializeField] string[] _collisionTag;
        //float hitTime;
        //private Material _mat;

        //private bool _init = false;


        private CombatUnit _target;

        private object _source;


        Dictionary<CombatUnit, ContactPoint[]> _contactPoints = new Dictionary<CombatUnit, ContactPoint[]>();

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
            //_mat = mat;
            _target = target;
            _target.Disable("Vulnerable");
            GetComponent<Renderer>().sharedMaterial = mat;
            _source = src;
        }

        public void Reset()
        {
            _stats.GetResourceByName("HP").Reset();
        }

        

        public override int TakeDamage(float baseDmg, float pow, CombatUnit damager)
        {
            int dmg = 0;
            if (damager.gameObject.CompareTag("Player"))
            {

                PlayerController player = damager.gameObject.GetComponent<PlayerController>();
                if(player.GetAnotherPlayer().inToolState() && player.GetAnotherPlayer().getTool().getName() == "LightSaber")
                {
                    dmg = base.TakeDamage(baseDmg, pow, damager);
                }
            }

            return dmg;
        }

        protected override void FixedUpdate() { }
        protected override void Interrupted()
        {
            Debug.Log("no interrupted action for force shield");
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    CombatUnit combat = collision.gameObject.GetComponent<CombatUnit>();
        //    if (combat == null) return;

        //    if (_contactPoints.ContainsKey(combat))
        //    {
        //        _contactPoints[combat] = collision.contacts;
        //    }
        //    else
        //    {
        //        _contactPoints.Add(combat, collision.contacts);
        //    }
        //}

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

