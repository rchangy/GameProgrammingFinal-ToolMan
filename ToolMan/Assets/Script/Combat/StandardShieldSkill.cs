using UnityEngine;
using System.Collections;

public class StandardShieldSkill : Skill
{
    public float attackRange;
    public CharacterStats friend;

    public override IEnumerator Attack(Animator anim, LayerMask targetLayer, Stat atk, Stat attackSpeed, CharacterStats stats)
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
