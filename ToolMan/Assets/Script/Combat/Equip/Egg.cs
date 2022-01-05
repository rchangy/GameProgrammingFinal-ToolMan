using UnityEngine;
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

        // explode
        [SerializeField]
        private float _explosionRange;
        [SerializeField]
        private float _atkMultiplier;
        [SerializeField] private GameObject _explosionVfx;

        [SerializeField]
        private LayerMask _targetLayers;

        private EnemyBigChicken bigChicken;

        public AudioSource audioSource;
        public AudioClip bombAudio;

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

        public override int TakeDamage(float baseDmg, float pow, CombatUnit damager)
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
            if(_explosionVfx != null)
            {
                GameObject vfx = Instantiate(_explosionVfx, transform.position, Quaternion.identity);
                Explosion explosion = vfx.GetComponent<Explosion>();
                if (audioSource && bombAudio)
                    audioSource.PlayOneShot(bombAudio);
                if(explosion != null)
                {
                    explosion.PlayEffect();
                }
            }
            Collider[] hitTargets = Physics.OverlapSphere(_rb.gameObject.transform.position, _explosionRange, _targetLayers);
            foreach (Collider target in hitTargets)
            {
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage(Atk, Pow, this);
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

