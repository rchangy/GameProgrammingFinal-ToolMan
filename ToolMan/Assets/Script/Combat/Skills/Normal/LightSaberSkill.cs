using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/LightSaber")]
    public class LightSaberSkill : PlayerSkill
    {
        [SerializeField] private float _collidingTime;
        [SerializeField] private float _deformedOnAxis;

        [SerializeField] private float _deformingTime = 0.5f;
        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // Animation
            _toolController.AnimationAttack();
            Debug.Log("Light Saber Attack");
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
            Debug.Log(targetTrans.name);
            Vector3 originalScale = targetTrans.localScale;
            
            float targetZScale = targetTrans.localScale.z * _deformedOnAxis;
            float targetXScale = targetTrans.localScale.z * _deformedOnAxis;
            Vector3 deform;
            if (_deformingTime == 0) _deformingTime = 0.5f;
            
            deform = new Vector3((targetTrans.localScale.x - targetXScale) / _deformingTime, 0, (targetTrans.localScale.z - targetZScale) / _deformingTime);
            while (targetTrans.localScale.z >= targetZScale)
            {
                targetTrans.localScale -= deform * Time.deltaTime;
                //Debug.Log(targetTrans.localScale);
                yield return null;
            }

            while (targetTrans.localScale.z <= originalScale.z)
            {
                targetTrans.localScale += deform * Time.deltaTime;
                yield return null;
            }
            targetTrans.localScale = originalScale;
        }
    }
}