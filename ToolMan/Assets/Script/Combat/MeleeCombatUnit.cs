using UnityEngine;
using System.Collections.Generic;

public class MeleeCombatUnit : CombatUnit
{
    public Transform AttackPoint;
    public float AttackDelay;
    public float AttackRange;

    public override void Attack()
    {
        if (HasAttacked) return;

        // Animation

        // delay
        StartCoroutine(ExecuteAfterTime(AttackDelay));

        // Check collisions
        Collider[] hitTargets = Physics.OverlapSphere(AttackPoint.position, AttackRange, TargetLayers);
        foreach (Collider target in hitTargets)
        {
            Debug.Log(gameObject.name + " hit " + target.name);
            // implement Character stat later and one Object can have at most one stat, but multiple type of combat unit
            CombatUnit combat = target.GetComponent<CombatUnit>();
            if(combat != null)
            {
                combat.TakeDamage(Atk);
            }
        }

        HasAttacked = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }


}
