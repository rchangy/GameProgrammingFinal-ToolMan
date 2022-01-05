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
        public int HPAdded = 300;

        public void setObjective(RetrieveMemory RM)
        {
            this.RM = RM;
        }

        public override bool Attack()
        {
            return false;
        }

        public override int TakeDamage(float baseDmg, float pow, CombatUnit damager)
        {
            hitBy = damager.gameObject.GetComponent<PlayerController>();
            if (this.gameObject == null)
                return 0;
            if (hitBy != null)
            {
                if (hitBy.GetAnotherPlayer().inToolState() && hitBy.GetAnotherPlayer().getTool().getName().Equals("Pickaxe"))
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
                hitBy.AddHP(HPAdded);
                hitBy.GetAnotherPlayer().AddHP(HPAdded);
            }
            if (hitBy != null && crystalType == 1)
            {
                RM.CrystalDie();
            }
            Destroy(gameObject);
        }
    }
}

