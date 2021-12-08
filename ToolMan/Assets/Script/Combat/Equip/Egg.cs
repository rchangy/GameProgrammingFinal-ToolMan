﻿using UnityEngine;
using System.Collections;

namespace ToolMan.Combat.Equip
{
    public class Egg : CombatUnit
    {
        [SerializeField]
        private float _hatchTime;
        private float _lastingTimeToHatch;

        [SerializeField]
        private GameObject _chickPrefab;
        [SerializeField]
        private Vector3 _chickHatchOffset;

        [SerializeField]
        private int _bombWeight;
        [SerializeField]
        private int _chichWeight;

        private Rigidbody _rb;

        // explode
        [SerializeField]
        private float _explosionRange;
        [SerializeField]
        private float _atkMultiplier;

        [SerializeField]
        private LayerMask _targetLayers;

        private EnemyBigChicken bigChicken;

        protected override void Start()
        {
            base.Start();
            _lastingTimeToHatch = _hatchTime;
            _rb = gameObject.GetComponent<Rigidbody>();
            gameObject.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        }

        protected override void Update()
        {
            base.Update();
            _lastingTimeToHatch -= Time.deltaTime;
            if(_lastingTimeToHatch <= 0)
            {
                Hatch();
            }
        }

        public override bool Attack()
        {
            return false;
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            _hp.ChangeValueBy(-1);
            return 1;
        }

        private void Hatch()
        {
            int rand = Random.Range(0, _bombWeight + _chichWeight);
            if(rand < _bombWeight)
            {
                Bomb();
            }
            else
            {
                Chick();
            }
        }

        private void Bomb()
        {
            Collider[] hitTargets = Physics.OverlapSphere(_rb.gameObject.transform.position, _explosionRange, _targetLayers);
            foreach (Collider target in hitTargets)
            {
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage(Atk * _atkMultiplier, this);
                }
            }
            Destroy(gameObject);
        }

        private void Chick()
        {
            GameObject newChick = Instantiate(_chickPrefab, gameObject.transform.position + _chickHatchOffset, Quaternion.identity);
            newChick.GetComponent<EnemyChick>().setMom(bigChicken);
            Destroy(gameObject);
        }

        public void setBigChicken(EnemyBigChicken bigChicken)
        {
            this.bigChicken = bigChicken;
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }
        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRange);
        }
    }
}

