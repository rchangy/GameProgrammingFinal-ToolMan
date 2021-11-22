using UnityEngine;
using System.Collections;

public class StandardShieldSkill : Skill
{
    public float attackRange;
    public CombatUnit friend;

    public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, int Atk)
    {
        combat.CanBeHurt = false;
        friend.CanBeHurt = false;
        // Animation
        anim.SetTrigger("Attack");
        // delay
        yield return new WaitForSeconds(attackDelay);

        combat.CanBeHurt = true;
        friend.CanBeHurt = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
