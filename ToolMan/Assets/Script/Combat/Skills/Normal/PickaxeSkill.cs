using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Pickaxe")]
    public class PickaxeSkill : PlayerSkill
    {
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // Animation
            Debug.Log("Pickaxe Attack");
            _toolController.AnimationAttack();
            // delay
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            if (!_toolController.ToolWave)
            {
                _toolController.SetToolWave(new Vector3(0, 90, 90f), 1f, true);
            }
            while (!_toolController.WaveEnd)
            {
                yield return null;
            }
            _toolController.ResetToolWave();
            collisionEnable.Value = false;
        }
    }
}