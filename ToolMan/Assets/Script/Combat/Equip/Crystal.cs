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
        private RetrieveMemory RM;

        public void setObjective(RetrieveMemory RM)
        {
            this.RM = RM;
        }

        public override bool Attack()
        {
            return false;
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            Debug.Log("hit by: " + hitBy.gameObject.name);
            if (this.gameObject == null)
                return 0;
            if (hitBy != null)
            {
                Debug.Log("hit tool: " + hitBy.getTool().getName());
                if (hitBy.inToolState() && hitBy.getTool().getName().Equals("Pickaxe"))
                {
                    hitCount += 1;
                    gameObject.GetComponent<Animator>().SetTrigger("hit");
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
            StartCoroutine(WaitDie(0.3f));
        }

        private IEnumerator WaitDie(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (hitBy != null && crystalType == 0)
            {
                hitBy.AddHP(10);
                hitBy.GetAnotherPlayer().AddHP(10);
            }
            if (hitBy != null && crystalType == 1)
            {
                RM.CrystalDie();
            }
            Destroy(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("crystal hit!");
            if (other.gameObject.layer == LayerMaskUtil.LayerBitMaskToLayerNumber(_crystalTargetLayers.value))
            {
                hitBy = other.gameObject.GetComponent<PlayerController>();
            }
        }
    }
}

