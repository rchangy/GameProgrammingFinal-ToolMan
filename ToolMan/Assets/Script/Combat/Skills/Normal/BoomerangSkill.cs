using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;
using ToolMan.Combat.Stats;

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

        public float MaxAimingDist = 10;

        public float MaxAimingAngle = 85;

        private readonly StatModifier _defStatMod = new StatModifier(20, StatModType.PercentMult);
        private readonly StatModifier _strStatMod = new StatModifier(100, StatModType.PercentMult);


        public float MaxShake;

        public float MaxMultiplier;
        public float MinMultiplier;
        public float MultDecrease;
        public float MultRevertDelay;
        public float MultRevertSpd;
        private float _lastTime = 0;

        //private float _currentMult;

        


        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            var passedTime = Time.time - _lastTime;
            _lastTime = Time.time;
            if (passedTime < MultRevertDelay)
            {
                if(Multiplier > MinMultiplier)
                {
                    Multiplier -= MultDecrease;
                    if (Multiplier < MinMultiplier) Multiplier = MinMultiplier;
                }
                
            }
            else
            {
                if(Multiplier < MaxMultiplier)
                {
                    var revertTime = passedTime - MultRevertDelay;
                    if(revertTime > 0)
                    {
                        Multiplier += MultRevertSpd * revertTime;
                        if (Multiplier > MaxMultiplier) Multiplier = MaxMultiplier;
                    }
                }
            }
            var shakeRange = MaxShake * (1 - ((Multiplier - MinMultiplier) / (MaxMultiplier - MinMultiplier)) );
            
            //Debug.Log(Multiplier + " " + shakeRange);
            _toolCombat.AddStatMod("DEF", _defStatMod);
            _toolCombat.AddStatMod("STR", _strStatMod);
            _tool = combat.gameObject;
            if (typeof(PlayerCombat).IsInstanceOfType(combat))
            {
                PlayerCombat toolCombat = (PlayerCombat)combat;
                PlayerCombat manCombat = toolCombat.TeamMateCombat;
                _man = manCombat.gameObject;

                _manController = _man.GetComponent<PlayerController>();
                _toolController = _tool.GetComponent<PlayerController>();
                _manController.getAnimator().SetTrigger("Attack");
                yield return new WaitForSeconds(attackDelay);
            }
            else
            {
                _toolCombat.RemoveStatMod("DEF", _defStatMod);
                _toolCombat.RemoveStatMod("STR", _strStatMod);
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
            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            Vector3 targetPos = _man.transform.position + _man.transform.forward * 10;
            if(enemies != null && enemies.Length > 0)
            {
                float minDist = float.MaxValue;
                foreach(Enemy e in enemies)
                {
                    float dist = Vector3.Distance(e.transform.position, _man.transform.position);
                    if(dist < minDist && dist < MaxAimingDist)
                    {
                        Vector3 dir = e.transform.position - _man.transform.position;
                        if (Vector3.Angle(_man.transform.forward, dir) <= MaxAimingAngle)
                        {
                            minDist = dist;
                            targetPos = e.transform.position;
                        }
                    }
                }
            }

            if(targetPos.y < combat.transform.position.y)
                targetPos.y = combat.transform.position.y;

            _tool.transform.Rotate(0, 180, 0);

            // to target
            float flyingTimeLast = _flyingTime;
            Vector3 nextStepPos;
            Vector3 nextStepDir;
            float tmp;
            RaycastHit m_Hit;
            bool m_HitDetect;
            float curShakeRange;

            while (flyingTimeLast > 0 && Vector3.Distance(combat.transform.position, targetPos) > 0.1f)
            {
                // audio
                if (player.playerAudioStat.lastAttackTime >= 1.1f)
                    Schedule<PlayerAttack>().player = player;

                _tool.transform.Rotate(0, 0, Time.deltaTime * -800);
                nextStepPos = Vector3.MoveTowards(_tool.transform.position, targetPos, Time.deltaTime * 40);
                //m_HitDetect = Physics.BoxCast(combat.transform.position, combat.transform.localScale, combat.transform.forward, out m_Hit, combat.transform.rotation, Vector3.Distance(nextStepPos, combat.transform.position));
                //if (m_HitDetect && !(m_Hit.collider.gameObject.CompareTag("Player")) && !m_Hit.collider.isTrigger)
                //{
                //    break;
                //}
                nextStepDir = Vector3.Normalize(nextStepPos - _tool.transform.position);
                tmp = nextStepDir.x;
                nextStepDir.x = nextStepDir.z;
                nextStepDir.z = -tmp;
                nextStepDir *= Random.Range(-1f, 1f);
                nextStepDir.y = Random.Range(-1f, 1f);
                curShakeRange = Random.Range(0, shakeRange);
                nextStepPos += nextStepDir * curShakeRange;
                m_HitDetect = Physics.BoxCast(_tool.transform.position, _tool.transform.localScale/3, _tool.transform.forward, out m_Hit, _tool.transform.rotation, Vector3.Distance(nextStepPos, _tool.transform.position));
                if (m_HitDetect && !m_Hit.collider.gameObject.CompareTag("Player") && !m_Hit.collider.isTrigger)
                {
                    nextStepPos -= 2 * shakeRange * nextStepDir;
                    break;
                }
                _tool.transform.position = nextStepPos;
                flyingTimeLast -= Time.deltaTime;
                yield return null;
            }

            flyingTimeLast = 1;

            while(flyingTimeLast > 0)
            {
                nextStepDir.x = Random.Range(-1f, 1f);
                nextStepDir.z = Random.Range(-1f, 1f);
                nextStepDir.y = Random.Range(-1f, 1f);
                curShakeRange = Random.Range(0, shakeRange);
                nextStepPos = _tool.transform.position + nextStepDir * curShakeRange;
                m_HitDetect = Physics.BoxCast(_tool.transform.position, _tool.transform.localScale / 3, _tool.transform.forward, out m_Hit, _tool.transform.rotation, Vector3.Distance(nextStepPos, _tool.transform.position));
                if (!m_HitDetect || m_Hit.collider.gameObject.CompareTag("Player") || m_Hit.collider.isTrigger)
                {
                    _tool.transform.position = nextStepPos;
                    //break;
                }
                _tool.transform.Rotate(0, 0, Time.deltaTime * -800);
                flyingTimeLast -= Time.deltaTime;
                yield return null;
            }

            // return
            flyingTimeLast = _flyingTime * 1.5f;
            
            while (flyingTimeLast > 0)
            {
                _tool.transform.Rotate(0, 0, Time.deltaTime * -800);
                //_tool.transform.position = Vector3.MoveTowards(_tool.transform.position, _manController.GetRightHand().transform.position, Time.deltaTime * 40);
                nextStepPos = Vector3.MoveTowards(_tool.transform.position, _manController.GetRightHand().transform.position, Time.deltaTime * 40);
                nextStepDir = Vector3.Normalize(nextStepPos - _tool.transform.position);
                tmp = nextStepDir.x;
                nextStepDir.x = nextStepDir.z;
                nextStepDir.z = -tmp;
                nextStepDir *= Random.Range(-1f, 1f);
                nextStepDir.y = Random.Range(-1f, 1f);
                curShakeRange = Random.Range(0, shakeRange);
                nextStepPos += nextStepDir * curShakeRange;
                m_HitDetect = Physics.BoxCast(_tool.transform.position, _tool.transform.localScale / 3, _tool.transform.forward, out m_Hit, _tool.transform.rotation, Vector3.Distance(nextStepPos, _tool.transform.position));
                if (m_HitDetect && !m_Hit.collider.gameObject.CompareTag("Player") && !m_Hit.collider.isTrigger)
                {
                    nextStepPos -= curShakeRange * nextStepDir;
                    //break;
                }
                _tool.transform.position = nextStepPos;
                if (Vector3.Distance(_man.transform.position, _tool.transform.position) < 1.5)
                {
                    _manController.forceGrabbing();
                    if (_toolController.IsGrabbed())
                    {
                        collisionEnable.Value = false;
                        _tool.GetComponent<Rigidbody>().useGravity = true;
                        _toolCombat.RemoveStatMod("DEF", _defStatMod);
                        _toolCombat.RemoveStatMod("STR", _strStatMod);
                        yield break;
                    }
                }
                flyingTimeLast -= Time.deltaTime;
                yield return null;
            }
            _tool.GetComponent<Rigidbody>().useGravity = true;
        }
        public override IEnumerator Hit(SkillCombat combat, CombatUnit target)
        {
            Transform targetTrans = target.gameObject.transform;
            Vector3 originalScale = targetTrans.localScale;

            float targetZScale = targetTrans.localScale.z * _deformedOnAxis;
            float targetXScale = targetTrans.localScale.z * _deformedOnAxis;
            Vector3 deform;
            if (_deformingTime == 0) _deformingTime = 0.5f;

            deform = new Vector3((targetTrans.localScale.x - targetXScale) / _deformingTime, 0, (targetTrans.localScale.z - targetZScale) / _deformingTime);
            while (targetTrans.localScale.z >= targetZScale)
            {
                targetTrans.localScale -= deform * Time.deltaTime;
                yield return null;
            }

            while (targetTrans.localScale.z <= originalScale.z)
            {
                targetTrans.localScale += deform * Time.deltaTime;
                yield return null;
            }
            targetTrans.localScale = originalScale;
        }


        public override void ResetObject()
        {
            _lastTime = 0;
            Multiplier = MaxMultiplier;
        }
    }


}
