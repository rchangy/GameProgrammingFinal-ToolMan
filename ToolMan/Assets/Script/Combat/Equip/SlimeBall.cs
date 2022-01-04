using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;

namespace ToolMan.Combat.Equip
{
    public class SlimeBall : MonoBehaviour
    {
        private CombatUnit _slimeRabbitCombat;
        [SerializeField] private Skill _skill;

        [SerializeField]
        private float _atkMultiplier;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private EffectController effectController;

        [SerializeField]
        private GameObject _explosionPrefab;

        private bool _destIsSet = false;
        private Vector3 _dest;
        private float _maxDis;
        private float _currentDis;

        private bool explodeOnDeath;

        public void setDest(Vector3 dest, float speed, float maxDis, CombatUnit slimeRabbitCombat)
        {
            _dest = dest;
            _destIsSet = true;
            _maxDis = maxDis;
            _currentDis = 0f;
            _slimeRabbitCombat = slimeRabbitCombat;
            explodeOnDeath = true;
        }

        private void FixedUpdate()
        {
            if (_destIsSet)
            {
                MoveTowardDest();
                if (_currentDis >= _maxDis || transform.position == _dest) Die();
            }
        }

        private void MoveTowardDest()
        {
            Vector3 tmp = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _dest, Time.deltaTime * _speed);
            _currentDis += (transform.position - tmp).magnitude;
        }

        private void Die()
        {
            // Explosion effect
            if (explodeOnDeath)
            {
                Effect effect = Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<Effect>();
                effect.PlayEffect();
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            PlayerCombat playerCombat = other.gameObject.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                Debug.Log("vulnerable = " + playerCombat.Vulnerable);
                if (playerCombat.Vulnerable)
                {
                    playerCombat.TakeDamage(_slimeRabbitCombat.Atk * _skill.Multiplier, _slimeRabbitCombat.Pow * _skill.PowMuliplier,_slimeRabbitCombat);
                    Debug.Log(other.name);
                    Die();
                }
            }
            else
            {
                //Debug.Log(other.name);
                Die();
            }
        }
    }
}
