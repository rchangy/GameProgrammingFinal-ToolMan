﻿using UnityEngine;
using System.Collections;
using ToolMan.Util;

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

        [SerializeField]
        private float _atkMultiplier;

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
                    _toolController.AnimationAttack();
                    Debug.Log("Explosion");
                    // Check collisions
                    Collider[] hitTargets = Physics.OverlapSphere(_rb.gameObject.transform.position, _explosionRange, combat.TargetLayers);
                    foreach (Collider target in hitTargets)
                    {
                        CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                        if (targetCombat != null)
                        {
                            targetCombat.TakeDamage((combat.Atk * _atkMultiplier), combat);
                        }
                    }
                    break;
                }
                yield return null;
            }
        }

    }
}