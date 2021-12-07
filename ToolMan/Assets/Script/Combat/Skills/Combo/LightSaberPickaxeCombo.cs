using System;
using System.Collections;
using UnityEngine;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Combo
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Combo/LightSabertoPickaxe")]
    public class LightSaberPickaxeCombo : ComboSkill
    {
        private void Awake()
        {
            _preTool = "LightSaber";
            _postTool = "Pickaxe";
            _cost = 0;
        }
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            Debug.Log("performing lightSaber to Pickaxe combo skill");
            yield return new WaitForSeconds(attackDelay);
        }
    }
}