using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Stats.Buff;
using System.Collections.Generic;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/TurtleAddBuff")]
    public class TurtleAddBuff : Skill
    {
        [SerializeField] private ScriptableBuff _buff;
      
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return null;
        }


        public override IEnumerator Hit(SkillCombat combat, CombatUnit target)
        {
            yield return new WaitForSeconds(attackDelay);

            // Anim
            combat.gameObject.GetComponent<EnemyTurtle>().GetAnimator().SetTrigger("Eat");

            // Add buff
            target.AddBuff(_buff);
            Debug.Log("Add Buff!!");
            yield break;
        }
    }

    
}