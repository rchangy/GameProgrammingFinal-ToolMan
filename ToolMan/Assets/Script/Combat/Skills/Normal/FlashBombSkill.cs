using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/FlashBomb")]
    public class FlashBombSkill : PlayerSkill
    {
        [SerializeField]
        private float _force;
        public float Force
        {
            get => _force;
        }

        private Rigidbody _rb;

        [SerializeField]
        private float _explosionRange;
        public float ExplosionRange
        {
            get => _explosionRange;
        }

        //[SerializeField]
        //private float _atkMultiplier;

        [SerializeField] private GameObject _explosionVfx;

        [SerializeField] private ScriptableBuff _buff;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            _rb = _tool.GetComponent<Rigidbody>();

            yield return new WaitForSeconds(attackDelay);
            //Debug.Log(_manController.gameObject.name);
            _manController.Release();
            var dir = _man.transform.forward;
            dir.y = 0;
            dir = Vector3.Normalize(dir);
            dir.y = 1;
            _rb.AddForce(dir * _force);
            yield return new WaitForSeconds(0.3f);
            while (true)
            {
                if (Input.GetButtonDown("JumpOrAttack2"))
                {
                    // audio
                    Schedule<PlayerAttack>().player = _toolController;
                    _toolController.AnimationAttack();
                    // vfx
                    if(_explosionVfx != null)
                    {
                        var explosion = Instantiate(_explosionVfx, _tool.transform.position, Quaternion.identity);
                        Explosion vfx = explosion.GetComponent<Explosion>();
                        if(vfx != null)
                        {
                            vfx.PlayEffect();
                        }
                    }
                    Debug.Log("Explosion");
                    // Check collisions
                    _toolCombat.HitFeel.MakeCamShake(HitFeelMul, _toolController);
                    _manCombat.HitFeel.MakeCamShake(HitFeelMul, _manController);
                    if (_buff == null) yield break;
                    Collider[] hitTargets = Physics.OverlapSphere(_rb.gameObject.transform.position, _explosionRange, combat.TargetLayers);
                    foreach (Collider target in hitTargets)
                    {
                        Debug.Log("flash bomb hit " + target.name);
                        CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                        if (targetCombat != null)
                        {
                            targetCombat.TakeDamage(0, 0, combat);
                            targetCombat.AddBuff(_buff);
                        }
                    }
                    break;
                }
                yield return null;
            }
        }

    }
}