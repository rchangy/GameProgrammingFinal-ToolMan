using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Stats.Buff;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/TurtleAddBuff")]
    public class TurtleAddBuff : Skill
    {
        [SerializeField] private ScriptableBuff _buff;
        [SerializeField] private float _lastingTime;
      
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);

            Debug.Log("start buff");

            // Anim
            combat.gameObject.GetComponent<EnemyTurtle>().GetAnimator().SetTrigger("Eat");

            // Effect
            Effect e = combat.gameObject.GetComponent<EnemyTurtle>().GetEffectController().effectList.Find(e => e.name == "TurtleBuff");
            e.PlayEffect();

            collisionEnable.Value = true;
            yield return new WaitForSeconds(_lastingTime);

            collisionEnable.Value = false;
            // Stop Effect
            e.StopEffect();

            yield return null;
        }


        public override IEnumerator Hit(SkillCombat combat, CombatUnit target)
        {    
            // Add buff
            target.AddBuff(_buff);
            Debug.Log("Add Buff to " + target.gameObject.name);
            yield break;
        }
    }

    
}