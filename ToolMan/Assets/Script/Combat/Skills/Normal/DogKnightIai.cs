using UnityEngine;
using System.Collections;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/DogKnightIai")]
    public class DogKnightIai : Skill
    {
        public float ChargingTime;
        private EnemyDogKnight _dogKnight;
        public float _rushSpeed = 100;
        private Animator _anim;
        private Transform _attackPoint;
        [SerializeField] private float _attackRange;
        private Explosion _chargingVfx;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            Debug.Log("Iai!!!");
            if(_dogKnight == null || _anim == null)
            {
                _dogKnight = combat.gameObject.GetComponent<EnemyDogKnight>();
                _anim = combat.gameObject.GetComponent<Animator>();
                if (_dogKnight == null || _anim == null)
                {
                    Debug.Log("[DogKnightIai] enemy or animator not found");
                    yield break;
                }
                _attackPoint = _dogKnight.IaiAttackPoint;
                _chargingVfx = _dogKnight.ChargingVfx;
                if (_attackPoint == null || _chargingVfx == null)
                {
                    Debug.Log("[DogKnightIai] attack point or charging effect not set");
                    yield break;
                }
            }

            _anim.SetBool("Defend", true);
            _chargingVfx.PlayEffect();
            // enter charging time
            yield return new WaitForSeconds(ChargingTime);
            _chargingVfx.StopEffect();
            // vfx
            Transform targetPlayer = _dogKnight.GetClosestplayer();
            _dogKnight.transform.LookAt(targetPlayer);
            Vector3 targetPos = targetPlayer.position;
            targetPos.y = _dogKnight.transform.position.y;
            while(Vector3.Distance(_dogKnight.transform.position, targetPos) > 2f)
            {
                _dogKnight.transform.position = Vector3.MoveTowards(_dogKnight.transform.position, targetPos, _rushSpeed * Time.deltaTime);
                yield return null;
            }
            _anim.SetTrigger("Attack2");
            // animation
            yield return new WaitForSeconds(attackDelay);
            _anim.SetBool("Defend", false);
            Collider[] hitTargets = Physics.OverlapSphere(_attackPoint.position, _attackRange, combat.TargetLayers);
            foreach (Collider target in hitTargets)
            {
                Debug.Log("[DogKnightIai] hit " + target.name);
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage(combat.Atk, combat);
                }
            }
            yield return null;
        }
    }
}
