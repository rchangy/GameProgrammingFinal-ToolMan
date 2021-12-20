using UnityEngine;
using System.Collections;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Shield")]
    public class ShieldSkill : PlayerSkill
    {
        //public ScriptableBuff Buff;
        [SerializeField] private float _invulnerableTime;


        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            _toolController.AnimationAttack();
            yield return new WaitForSeconds(attackDelay);
            _toolCombat.Disable("Vulnerable");
            _manCombat.Disable("Vulnerable");
            yield return new WaitForSeconds(_invulnerableTime);
            _toolCombat.RemoveDisable("Vulnerable");
            _manCombat.RemoveDisable("Vulnerable");

        }

    }
}