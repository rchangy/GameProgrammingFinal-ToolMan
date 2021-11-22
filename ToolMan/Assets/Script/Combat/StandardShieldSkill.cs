using UnityEngine;
using System.Collections;

public class StandardShieldSkill : Skill
{
    public float attackRange;
    public CharacterStats friend;
    public ScriptableBuff Buff;

    public override IEnumerator Attack(Animator anim, LayerMask targetLayer, Stat atk, Stat attackSpeed, CharacterStats stats)
    {
        Buff.AddBuff(stats);
        Buff.AddBuff(friend);
        
        // Animation
        anim.SetTrigger("Attack");
        // delay
        yield return new WaitForSeconds(attackDelay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
