using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Equip
{
    [RequireComponent(typeof(Collider))]
    public class Crystal : CombatUnit
    {
        [SerializeField] private LayerMask _crystalTargetLayers;
        [SerializeField] private int crystalType = 0;
        private PlayerController hitBy = null;
        private int hitCount = 0;

        public override bool Attack()
        {
            return false;
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            if (hitBy != null)
            {
                if (hitBy.inToolState() && hitBy.getTool().getName().Equals("Pickaxe"))
                {
                    hitCount += 1;
                }
            }
            if (hitCount >= 2)
            {
                Die();
            }
            
            return 1;
        }

        protected override void Die()
        {
            if (hitBy != null && crystalType == 0)
            {
                hitBy.combat.AddHp(10);
                hitBy.GetAnotherPlayer().combat.AddHp(10);
            }
            if (hitBy != null && crystalType == 1)
            {
                CheckpointManager.NextLevel();
                GameObject cmObject = GameObject.Find("CombatManager");
                if (cmObject != null)
                {
                    cmObject.GetComponent<CombatManager>().UnLockTool();
                }
            }
            Destroy(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.layer == LayerMaskUtil.LayerBitMaskToLayerNumber(_crystalTargetLayers.value))
            {
                hitBy = other.gameObject.GetComponent<PlayerController>();
            }
        }
    }
}

