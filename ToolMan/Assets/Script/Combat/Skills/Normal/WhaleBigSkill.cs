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


        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            EnemyWhale whale = combat.gameObject.GetComponent<EnemyWhale>();
            GameObject bigSkillObj = Instantiate(whale.bigSkillPrefab, whale.transform.position, Quaternion.identity);
            bigSkillObj.GetComponent<WhaleBigSkillPrefab>()._Play(whale, _warningLastingTime, attackDelay, _lastingTime);
            yield return new WaitForSeconds(_warningLastingTime + _lastingTime);

            //// Warning
            //Effect warningEffect = whale.effectController.effectList.Find(e => e.name == "WhaleBigSkillWarning");
            //warningEffect.PlayEffect();
            //// play warning sound

            //yield return new WaitForSeconds(_warningLastingTime);

            //// anim
            //yield return new WaitForSeconds(attackDelay);
            //whale.GetAnimator().SetTrigger("Attack");
            
            //collisionEnable.Value = true;
            //// effect
            //Effect bigSkillEffect =  whale.effectController.effectList.Find(e => e.name == "WhaleBigSkillEffect");
            //bigSkillEffect.PlayEffect();
            //yield return new WaitForSeconds(_lastingTime);
            //bigSkillEffect.StopEffect();

            //collisionEnable.Value = false;
        }
    }
}
