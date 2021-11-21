using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ToolMan.Combat.Skills.NormalSkill
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardMeleeSkill")]
    public class StandardMeleeSkill : Skill
    {
        public float attackRange;
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            // Animation
            anim.SetTrigger("Attack");
            // delay
            yield return new WaitForSeconds(attackDelay);

            // Check collisions
            Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayer);
            foreach (Collider target in hitTargets)
            {
                // implement Character stat later and one Object can have at most one stat, but multiple type of combat unit
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage((int)combat.Atk, combat);
                }
            }
        }

    }
}