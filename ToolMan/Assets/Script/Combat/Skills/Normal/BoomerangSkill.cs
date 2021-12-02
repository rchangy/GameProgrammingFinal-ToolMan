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

            // set target (closest enemy or something)
            Vector3 targetPos = _man.transform.position + _man.transform.forward * 10;
            _tool.transform.Rotate(0, 180, 0);

            // to target
            float flyingTimeLast = _flyingTime;
            while (flyingTimeLast > 0)
            {
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
