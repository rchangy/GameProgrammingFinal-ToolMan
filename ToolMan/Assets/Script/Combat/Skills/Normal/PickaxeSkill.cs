using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Pickaxe")]
    public class PickaxeSkill : PlayerSkill
    {
        [SerializeField] private float _collidingTime;
        [SerializeField] private float _deformedOnYAxis;

        [SerializeField] private float _deformingTime = 0.5f;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // Animation
            Debug.Log("Pickaxe Attack");
            _toolController.AnimationAttack();
            // delay
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            SetColor(true);

            yield return new WaitForSeconds(_collidingTime);

            collisionEnable.Value = false;
            SetColor(false);
        }

        public override IEnumerator Hit(SkillCombat combat, CombatUnit target)
        {
            
            Transform targetTrans = target.gameObject.transform;
            Vector3 originalScale = targetTrans.localScale;
            float targetYScale = targetTrans.localScale.y * _deformedOnYAxis;
            if (_deformingTime == 0) _deformingTime = 1;
            Vector3 deform = new Vector3(0, (targetTrans.localScale.y - targetYScale) / _deformingTime, 0);

            while(targetTrans.localScale.y >= targetYScale)
            {
                targetTrans.localScale -= deform;
                yield return null;
            }

            while (targetTrans.localScale.y <= originalScale.y)
            {
                targetTrans.localScale += deform;
                yield return null;
            }
            targetTrans.localScale = originalScale;
        }
    }
}