using UnityEngine;
using System.Collections;
public class StandardMeleeSkill : Skill
{
    public float attackRange;
    public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, int Atk)
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
                targetCombat.TakeDamage(Atk);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, attackRange);
    }
}
