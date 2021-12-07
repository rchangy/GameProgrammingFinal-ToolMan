using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    public class BigChickenLayEgg : Skill
    {
        [SerializeField]
        private GameObject _eggPrefab;
        [SerializeField]
        private Vector3 _layingPointOffset;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);

            Instantiate(_eggPrefab, combat.gameObject.transform.position + _layingPointOffset, Quaternion.identity);
        }
    }
}
