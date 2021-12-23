using UnityEngine;
using System.Collections;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/DogKnightNormal")]
    public class DogKnightNormalAttack : Skill
    {
        public Vector3 AttackPointOffset;
        public float AttackRange;
        public float AtkMul;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);
            Collider[] hitTargets = Physics.OverlapSphere(combat.gameObject.transform.position + AttackPointOffset, AttackRange, combat.TargetLayers);
            foreach (Collider target in hitTargets)
            {
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage(combat.Atk * AtkMul, combat);
                }
            }
        }
    }
}
