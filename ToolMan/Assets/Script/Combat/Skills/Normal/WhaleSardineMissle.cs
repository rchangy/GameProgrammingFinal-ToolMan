using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;

namespace ToolMan.Combat.Skills.Normal
{
    public class WhaleSardineMissle : Skill
    {
        [SerializeField]
        private GameObject _sardinePrefab;
        [SerializeField]
        private Vector3 _launchPositionOffset;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);
            Launch(combat);
        }

        private void Launch(CombatUnit whaleCombat)
        {
            GameObject sardineObject = Instantiate(_sardinePrefab, whaleCombat.gameObject.transform.position + _launchPositionOffset, Quaternion.identity);
            Sardine sardine = sardineObject.GetComponent<Sardine>();
            if(sardine != null)
            {
                sardine.SetWhale(whaleCombat);
            }
        }
    }
}
