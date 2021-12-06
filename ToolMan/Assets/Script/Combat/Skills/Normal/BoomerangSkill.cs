using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Boomerang")]
    public class BoomerangSkill : PlayerSkill
    {
        [SerializeField]
        private float _flyingTime;
        [SerializeField]
        private float _attackRange;
        [SerializeField]
        private float _sightRangeAngle;
        [SerializeField]
        private float _floatingTime;

        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
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
                yield break;
            }

            collisionEnable.Value = true;

            // release
            _manController.GetGrabPoint().GrabOrRelease();


            Vector3 targetPos = Vector3.zero;
            bool targetSelected = false;
            float minDist = float.MaxValue;
            // set target (closest enemy or something)
            Collider[] targets = Physics.OverlapSphere(_man.transform.position, _attackRange, targetLayer);
            foreach(Collider t in targets)
            {
                
                Vector3 targetDirection = t.gameObject.transform.position - _man.transform.position;
                if (Vector3.Angle(_man.transform.forward, targetDirection) <= _sightRangeAngle)
                {
                    float dist = 0;
                    if((dist = Vector3.Distance(t.gameObject.transform.position, _man.transform.position)) <= _attackRange)
                    {
                        if(dist < minDist)
                        {
                            targetSelected = true;
                            targetPos = t.gameObject.transform.position;
                        }
                    }
                }
            }
            if(!targetSelected)
            {
                targetPos = _man.transform.position + _man.transform.forward * _attackRange;
            }
            else
            {
                targetPos = _man.transform.position + Vector3.Normalize(targetPos - _man.transform.position) * _attackRange;
            }
            _tool.transform.Rotate(0, 180, 0);

            // to target
            float flyingTimeLast = _flyingTime;
            while (flyingTimeLast > 0)
            {
                _tool.transform.Rotate(0, 0, Time.deltaTime * 800);
                _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, targetPos, Time.deltaTime * 40);
                flyingTimeLast -= Time.deltaTime;
                if(_tool.transform.position == targetPos)
                {
                    break;
                }
                // return if collide with border 
                yield return null;
            }

            float floatingTime = _floatingTime;
            while(floatingTime > 0)
            {
                _tool.transform.Rotate(0, 0, Time.deltaTime * 800);
                floatingTime -= Time.deltaTime;
                yield return null;
            }

            // return
            flyingTimeLast = _flyingTime * 1.5f;
            while (flyingTimeLast > 0)
            {
                _tool.transform.Rotate(0, 0, Time.deltaTime * 800);
                _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, _man.transform.position, Time.deltaTime * 40);
                flyingTimeLast -= Time.deltaTime;
                if (Vector3.Distance(_man.transform.position, _tool.transform.position) < 1.5)
                {
                    _manController.GetGrabPoint().GrabOrRelease();
                    if (_toolController.IsGrabbed())
                    {
                        collisionEnable.Value = false;
                        yield break;
                    }
                }
                yield return null;
            }
        }
    }
}
