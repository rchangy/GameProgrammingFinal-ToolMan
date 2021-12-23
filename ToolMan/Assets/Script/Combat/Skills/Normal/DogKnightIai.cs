using UnityEngine;
using System.Collections;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    public class DogKnightIai : Skill
    {
        public float ChargingTime;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // animation or vfx
            // enter charging time
            yield return new WaitForSeconds(ChargingTime);

            yield return null;
        }
    }
}
