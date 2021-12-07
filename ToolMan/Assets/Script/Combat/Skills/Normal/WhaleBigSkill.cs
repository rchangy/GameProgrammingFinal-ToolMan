using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/WhaleBig")]
    public class WhaleBigSkill : Skill
    {
        [SerializeField]
        private float _lastingTime;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // sound
            yield return new WaitForSeconds(attackDelay);
            // anim
            collisionEnable.Value = true;
            yield return new WaitForSeconds(_lastingTime);
            // effect
            collisionEnable.Value = false;
        }
    }
}
