using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;

namespace ToolMan.Combat.Skills.Normal
{
    public class BigChickenLayEgg : Skill
    {
        [SerializeField]
        private GameObject _eggPrefab;
        [SerializeField]
        private Vector3 _layingPointOffset;
        private EnemyBigChicken bigChicken;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);
            bigChicken = combat.gameObject.GetComponent<EnemyBigChicken>();

            GameObject newEgg = Instantiate(_eggPrefab, combat.gameObject.transform.position + _layingPointOffset, Quaternion.identity);
            newEgg.GetComponent<Egg>().setBigChicken(bigChicken);
        }
    }
}
