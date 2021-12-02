using UnityEngine;
using System.Collections;
using ToolMan.Combat.Equip;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/SetForceShieldSkill")]
    public class SetForceShieldSkill : Skill
    {
        [SerializeField]
        private GameObject shieldPrefab;
        [SerializeField]
        private Material mat;
        
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            yield return new WaitForSeconds(attackDelay);
            var shields = combat.gameObject.GetComponentsInChildren<ForceShield>();
            bool isSet = false;
            foreach(ForceShield s in shields)
            {
                if (s.SetBy(this))
                {
                    isSet = true;
                    break;
                }
            }

            if (!isSet)
            {
                var newShield = Instantiate(shieldPrefab, combat.gameObject.transform);
                ForceShield s = newShield.GetComponent<ForceShield>();
                s.Setup(mat, combat, this);
            }
        }
    }
}
