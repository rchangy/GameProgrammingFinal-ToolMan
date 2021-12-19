using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Pickaxe")]
    public class PickaxeSkill : PlayerSkill
    {
        [SerializeField] private float _collidingTime;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // Animation
            Debug.Log("Pickaxe Attack");
            _toolController.AnimationAttack();
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