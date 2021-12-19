using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/LightSaber")]
    public class LightSaberSkill : PlayerSkill
    {
        [SerializeField] private float _collidingTime;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // Animation
            _toolController.AnimationAttack();
            Debug.Log("Light Saber Attack");
            // delay
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            SetColor(true);

            yield return new WaitForSeconds(_collidingTime);

            collisionEnable.Value = false;
            SetColor(false);
        }
    }
}