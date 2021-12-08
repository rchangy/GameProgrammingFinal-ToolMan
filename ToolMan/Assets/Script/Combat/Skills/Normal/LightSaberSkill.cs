using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/LightSaber")]
    public class LightSaberSkill : PlayerSkill
    {
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // Animation
            _toolController.AnimationAttack();
            Debug.Log("Light Saber Attack");
            // delay
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            if (!_toolController.ToolWave)
            {
                _toolController.SetToolWave(new Vector3(0, 45f, 90f), 6f, true);
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