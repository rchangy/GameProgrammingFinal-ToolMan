using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Boomerang")]
    public class BoomerangSkill : PlayerSkill
    {
        [SerializeField]
        private float _flyingTime;
        [SerializeField] private float _deformedOnAxis;

        [SerializeField] private float _deformingTime = 0.5f;

        private PlayerController player = null;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            _toolCombat.Disable("Vulnerable");
            yield return new WaitForSeconds(attackDelay);
            _tool = combat.gameObject;
            if (typeof(PlayerCombat).IsInstanceOfType(combat))
            {
                PlayerCombat toolCombat = (PlayerCombat)combat;
                PlayerCombat manCombat = toolCombat.TeamMateCombat;
                _man = manCombat.gameObject;

                _manController = _man.GetComponent<PlayerController>();
                _toolController = _tool.GetComponent<PlayerController>();
            }
            else
            {
                _toolCombat.RemoveDisable("Vulnerable");
                yield break;
            }

            collisionEnable.Value = true;

            // release
            _manController.GrabOrRelease();
            _tool.GetComponent<CapsuleCollider>().isTrigger = true;
            _tool.GetComponent<Rigidbody>().useGravity = false;

            if (player == null)
                player = combat.gameObject.GetComponent<PlayerController>();

            // set target (closest enemy or something)
            Vector3 targetPos = _man.transform.position + _man.transform.forward * 10;
            _tool.transform.Rotate(0, 180, 0);

            // to target
            float flyingTimeLast = _flyingTime;
            while (flyingTimeLast > 0)
            {
                // audio
                if (player.playerAudioStat.lastAttackTime >= 0.4f)
                    Schedule<PlayerAttack>().player = player;

                _tool.transform.Rotate(0, 0, Time.deltaTime * 800);
                _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, targetPos, Time.deltaTime * 40);
                flyingTimeLast -= Time.deltaTime;
                // return if collide with border 
                yield return null;
            }

            // return
            flyingTimeLast = _flyingTime * 1.5f;
            
            while (flyingTimeLast > 0)
            {
                _tool.transform.Rotate(0, 0, Time.deltaTime * 800);
                _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, _manController.GetRightHand().transform.position, Time.deltaTime * 40);
                flyingTimeLast -= Time.deltaTime;
                if (Vector3.Distance(_man.transform.position, _tool.transform.position) < 1.5)
                {
                    _manController.forceGrabbing();
                    if (_toolController.IsGrabbed())
                    {
                        collisionEnable.Value = false;
                        _tool.GetComponent<Rigidbody>().useGravity = true;
                        _toolCombat.RemoveDisable("Vulnerable");
                        yield break;
                    }
                }
                yield return null;
            }
            _tool.GetComponent<Rigidbody>().useGravity = true;
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
