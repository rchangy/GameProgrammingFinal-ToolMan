using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Stats.Buff;
using System.Collections.Generic;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/DogKnightShield")]
    public class DogKnightShield : Skill
    {
        private EnemyDogKnight _dogKnight;
        public List<ScriptableBuff> BuffList;

        public float Duration;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if(_dogKnight == null)
            {
                _dogKnight = combat.gameObject.GetComponent<EnemyDogKnight>();
                if (_dogKnight == null) yield break;
            }

            _dogKnight.Anim.SetBool("Defend", true);
            foreach(ScriptableBuff scriptableBuff in BuffList)
            {
                combat.AddBuff(scriptableBuff);
            }
            yield return new WaitForSeconds(Duration);

            foreach (ScriptableBuff scriptableBuff in BuffList)
            {
                combat.RemoveBuff(scriptableBuff);
            }

            _dogKnight.Anim.SetBool("Defend", true);
        }
    }

}
