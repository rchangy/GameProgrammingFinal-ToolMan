using UnityEngine;
using System.Collections;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Shield")]
    public class ShieldSkill : PlayerSkill
    {
        //public ScriptableBuff Buff;
        [SerializeField] private float _invulnerableTime;
        private PlayerShield _playerShield;
        private PlayerShield _playerShieldBig;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if(_playerShield == null)
            {
                _playerShield = _toolCombat.gameObject.transform.Find("Shield Canvas").GetComponent<PlayerShield>();
                _playerShieldBig = _toolCombat.gameObject.transform.Find("Shield Canvas Big").GetComponent<PlayerShield>();
                if (_playerShield == null)
                {
                    yield break;
                }
            }
            _toolController.AnimationAttack();
            yield return new WaitForSeconds(attackDelay);
            _playerShield.Fire(_invulnerableTime);
            _playerShieldBig.Fire(0);
            _toolCombat.Disable("Vulnerable");
            _manCombat.Disable("Vulnerable");
            yield return new WaitForSeconds(_invulnerableTime);
            _toolCombat.RemoveDisable("Vulnerable");
            _manCombat.RemoveDisable("Vulnerable");

        }

    }
}