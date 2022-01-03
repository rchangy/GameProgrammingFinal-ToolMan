using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;


namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/WhaleFlood")]
    public class WhaleFlood : Skill
    {
        private EnemyWhale _whale;
        public float FloodDuration;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if(_whale == null)
            {
                _whale = combat.gameObject.GetComponent<EnemyWhale>();
                if (_whale == null) yield break;
            }
            _whale.flood.StartFloodingForSeconds(FloodDuration);
        }
    }
}
