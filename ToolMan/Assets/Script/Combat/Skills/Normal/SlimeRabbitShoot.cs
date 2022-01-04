using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/SlimeRabbitShoot")]
    public class SlimeRabbitShoot : Skill
    {
        public GameObject SlimeBallPrefab;
        public float attackSpeed = 1f;
        public float maxDis = 5f;
        public float AfterAttackDelay;
        private EnemySlimeRabbit1 _slimeRabbit;


        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if (_slimeRabbit == null)
            {
                _slimeRabbit = combat.gameObject.GetComponent<EnemySlimeRabbit1>();
                if (_slimeRabbit == null) yield break;
            }
            _slimeRabbit.Anim.SetTrigger("Attack");
            yield return new WaitForSeconds(attackDelay);
            if (_slimeRabbit != null && SlimeBallPrefab != null)
            {
                SlimeBall slimeBall = (Instantiate(SlimeBallPrefab, new Vector3(0, 0, 0), Quaternion.identity)).GetComponent<SlimeBall>();
                slimeBall.transform.position = _slimeRabbit.enemyCollider.bounds.center + _slimeRabbit.transform.forward * 0.5f;
                slimeBall.setDest(_slimeRabbit.GetTarget().transform.position, attackSpeed, maxDis, combat);
            }
            yield return new WaitForSeconds(AfterAttackDelay);
        }
    }
}