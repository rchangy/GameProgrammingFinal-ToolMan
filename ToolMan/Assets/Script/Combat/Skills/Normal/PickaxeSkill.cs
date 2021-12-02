using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Pickaxe")]
    public class PickaxeSkill : PlayerSkill
    {
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            // Animation
            Debug.Log("Pickaxe Attack");
            // delay
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            if (!_toolController.ToolWave)
            {
                _toolController.SetToolWave(new Vector3(0, _man.transform.eulerAngles.y + 90, 90f), 1f, true);
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