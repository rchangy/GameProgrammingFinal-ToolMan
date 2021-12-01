using UnityEngine;
using System.Collections;
using ToolMan.Mechanics;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/BoomerangSkill")]
    public class BoomerangSkill : Skill
    {
        private GameObject _man;
        private GameObject _tool;

        [SerializeField]
        private float _flyingTime;

        private PlayerController _manController;
        private PlayerController _toolController;



        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            attackInterval = _flyingTime * 2.5f;
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

            // release
            _manController.GetGrabPoint().GrabOrRelease();

            // set target (closest enemy or something)
            Vector3 targetPos = _man.transform.position + _man.transform.forward * 10;

            _tool.transform.Rotate(0, 180, 0);

            float flyingTimeLast = _flyingTime;
            while (flyingTimeLast > 0)
            {
                _tool.transform.Rotate(0, 0, Time.deltaTime * 800);
                _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, targetPos, Time.deltaTime * 40);
                flyingTimeLast -= Time.deltaTime;
                // return if collide with border 
                yield return null;
            }
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
                        yield break;
                    }
                }
                yield return null;
            }


        }
    }
}
