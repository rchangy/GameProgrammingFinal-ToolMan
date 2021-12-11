using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Equip
{
    [RequireComponent(typeof(Collider))]
    public class Crystal : CombatUnit
    {
        [SerializeField]
        private LayerMask _targetLayers;

        private PlayerController hitBy = null;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override bool Attack()
        {
            return false;
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            //Debug.Log("crystal take damage");
            if (hitBy != null)
            {
                if (hitBy.inToolState() && hitBy.getTool().getName().Equals("Pickaxe"))
                    _hp.ChangeValueBy(-1);
            }
            
            return 1;
        }

        protected override void Die()
        {
            if (hitBy != null)
            {
                hitBy.combat.AddHp(10);
                hitBy.GetAnotherPlayer().combat.AddHp(10);
            }
            Destroy(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == Converter.LayerBitMaskToLayerNumber(_targetLayers.value))
            {
                hitBy = other.gameObject.GetComponent<PlayerController>();
            }
        }
    }
}

