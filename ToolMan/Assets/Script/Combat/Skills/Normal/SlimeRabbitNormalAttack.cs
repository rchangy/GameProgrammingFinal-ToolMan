using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/SlimeRabbitNormal")]
    public class SlimeRabbitNormalAttack : Skill
    {
        [SerializeField]
        private float _collisionTime;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            combat.gameObject.GetComponent<Animator>().SetTrigger("NormalAttack");
            yield return new WaitForSeconds(_collisionTime);
            collisionEnable.Value = false;
            yield return new WaitForSeconds(1.33f - _collisionTime);
        }

    }
}