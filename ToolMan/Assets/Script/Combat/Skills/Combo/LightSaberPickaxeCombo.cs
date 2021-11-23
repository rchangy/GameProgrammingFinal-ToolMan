using System;
using System.Collections;
using UnityEngine;
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
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            Debug.Log("performing lightSaber to Pickaxe combo skill");
            yield return new WaitForSeconds(attackDelay);

        }
    }
}