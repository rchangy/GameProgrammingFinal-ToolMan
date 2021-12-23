using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    public class DogKnightShockWave : Skill
    {
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);
            // instantiate multiple wave move toward player
            // and vfx
            // animation
        }
    }
}
