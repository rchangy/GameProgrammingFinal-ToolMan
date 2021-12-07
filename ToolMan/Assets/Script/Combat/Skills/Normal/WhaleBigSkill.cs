using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    public class WhaleBigSkill : Skill
    {
        [SerializeField]
        private float _lastingTime;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // sound
            yield return new WaitForSeconds(attackDelay);

            // anim
            EnemyWhale whale = combat.gameObject.GetComponent<EnemyWhale>();
            whale.GetAnimator().SetTrigger("Attack");

            collisionEnable.Value = true;
            yield return new WaitForSeconds(_lastingTime);
            // effect
            collisionEnable.Value = false;
        }
    }
}
