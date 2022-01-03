using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/WhaleBig")]
    public class WhaleBigSkill : Skill
    {
        [SerializeField]
        private float _lastingTime;
        [SerializeField]
        private float _warningLastingTime;
        EnemyWhale _whale;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if(_whale == null)
            {
                _whale = combat.gameObject.GetComponent<EnemyWhale>();
                if (_whale == null) yield break;
            }
            GameObject bigSkillObj = Instantiate(_whale.bigSkillPrefab, _whale.transform.position, Quaternion.identity);
            bigSkillObj.GetComponent<WhaleBigSkillPrefab>()._Play(_whale, _warningLastingTime, attackDelay, _lastingTime);
            yield return new WaitForSeconds(_warningLastingTime + _lastingTime);
        }
    }
}
