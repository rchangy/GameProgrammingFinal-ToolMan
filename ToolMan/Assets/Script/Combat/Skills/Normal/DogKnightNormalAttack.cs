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
        public float AfterAttackDelay;
        private EnemyDogKnight _dogKnight;
        

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if(_dogKnight == null)
            {
                _dogKnight = combat.gameObject.GetComponent<EnemyDogKnight>();
                if (_dogKnight == null) yield break;
            }
            _dogKnight.Anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(attackDelay);
            Collider[] hitTargets = Physics.OverlapSphere(combat.gameObject.transform.position + AttackPointOffset, AttackRange, combat.TargetLayers);
            foreach (Collider target in hitTargets)
            {
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage(combat.Atk * Multiplier, combat.Pow * PowMuliplier, combat);
                }
            }
            yield return new WaitForSeconds(AfterAttackDelay);
        }
    }
}
