using UnityEngine;
using System.Collections;
using ToolMan.Util;
using System;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/DogKnightIai")]
    public class DogKnightIai : Skill
    {
        public float ChargingTime;
        private EnemyDogKnight _dogKnight;
        public float _rushSpeed = 100;
        public float DizzyTime = 5f;
        public float NotifyTime;

        [SerializeField] private float _attackRange;
        private HitFeel _hitFeel;
        private PlayerController[] _hitPlayers = new PlayerController[2];

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            Debug.Log("Iai!!!");
            if(_dogKnight == null)
            {
                _hitFeel = combat.Manager.Model.hitFeel;
                _dogKnight = combat.gameObject.GetComponent<EnemyDogKnight>();
                if (_dogKnight == null)
                {
                    Debug.Log("[DogKnightIai] enemy or animator not found");
                    yield break;
                }
            }
            _dogKnight.Anim.SetBool("Defend", true);
            _dogKnight.SwordTrailVfx.PlayEffect();
            _dogKnight.ChargingVfx.PlayEffect();
            // enter charging time
            yield return new WaitForSeconds(ChargingTime - NotifyTime);
            _dogKnight.NotifyVfx.PlayEffect();
            yield return new WaitForSeconds(NotifyTime);
            _dogKnight.NotifyVfx.StopEffect();
            _dogKnight.ChargingVfx.StopEffect();
            // vfx
            Transform targetPlayer = _dogKnight.GetRealClosestplayer();
            Debug.Log(targetPlayer.gameObject.name);
            _dogKnight.transform.LookAt(targetPlayer);
            Vector3 targetPos = targetPlayer.position;
            targetPos.y = _dogKnight.transform.position.y;
            while(Vector3.Distance(_dogKnight.transform.position, targetPos) > 3f)
            {
                _dogKnight.transform.position = Vector3.MoveTowards(_dogKnight.transform.position, targetPos, _rushSpeed * Time.deltaTime);
                yield return null;
            }
            _dogKnight.Anim.SetTrigger("Attack2");
            // animation
            yield return new WaitForSeconds(attackDelay);
            _dogKnight.Anim.SetBool("Defend", false);
            bool isDizzy = false;
            int playerNum = 0;
            
            Collider[] hitTargets = Physics.OverlapSphere(_dogKnight.IaiAttackPoint.position, _attackRange, combat.TargetLayers);

            foreach (Collider target in hitTargets)
            {
                Debug.Log("[DogKnightIai] hit " + target.name);
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    if (typeof(PlayerCombat).IsInstanceOfType(targetCombat))
                    {
                        
                        PlayerCombat playerCombat = (PlayerCombat)targetCombat;
                        playerNum++;
                        _hitPlayers[playerNum-1] = playerCombat.ThisPlayerController;
                        if (!playerCombat.Vulnerable)
                        {
                            isDizzy = true;
                        }
                        else
                        {
                            targetCombat.TakeDamage(combat.Atk, combat);
                        }
                    }
                    else
                    {
                        if (targetCombat.Vulnerable)
                        {
                            targetCombat.TakeDamage(combat.Atk, combat);
                        }
                    }
                }
            }
            _dogKnight.SwordTrailVfx.StopEffect();


            _hitFeel.MakeTimeStop();
            if (playerNum == 2)
            {
                _hitFeel.MakeCamShake(3);
            }
            else if(playerNum == 1)
            {
                _hitFeel.MakeCamShake(3, _hitPlayers[0]);
            }

            yield return new WaitForSeconds(0.1f);

            if (isDizzy)
            {
                _dogKnight.IaiSparkVfx.PlayEffect();
                //_hitFeel.MakeTimeStop();
                _dogKnight.Anim.SetTrigger("Hurt");
                _dogKnight.Anim.SetBool("Dizzy", true);
                _dogKnight.Rb.AddForce(-_dogKnight.transform.forward*1000*_dogKnight.Rb.mass);
                _hitFeel.MakeCamShake(4);
                yield return new WaitForSeconds(0.3f);

                _dogKnight.IaiSparkVfx.StopEffect();
                yield return new WaitForSeconds(DizzyTime);
                _dogKnight.Anim.SetBool("Dizzy", false);
            }
        }
    }
}
