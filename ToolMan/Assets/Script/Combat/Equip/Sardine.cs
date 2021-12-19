using UnityEngine;
using System.Collections;

namespace ToolMan.Combat.Equip
{
    public class Sardine : MonoBehaviour
    {
        private GameObject _target;
        private GameObject _whale;
        private CombatUnit _whaleCombat;

        private bool _returning = false;
        [SerializeField]
        private float _atkMultiplier;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private EffectController effectController;

        [SerializeField]
        private GameObject _explosionPrefab;

        private bool explodeOnDeath;
        
        protected void Start()
        {
            // find target
            GameObject[] players =  GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 0)
            {
                _target = findClosestPlayer(players);
            }
            if(_target == null)
            {
                Destroy(gameObject);
            }
            explodeOnDeath = true;
        }

        private void FixedUpdate()
        {
            MoveTowardTarget();
        }

        public void SetWhale(CombatUnit whaleCombat)
        {
            _whale = whaleCombat.gameObject;
            _whaleCombat = whaleCombat;
        }

        private void MoveTowardTarget()
        {
            if (_returning)
            {
                transform.Rotate(0, 0, Time.deltaTime * 800);
            }
            else
            {
                transform.LookAt(_target.transform);
                // adapting rotation after lookat target
                //transform.Rotate();
            }
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
        }

        private GameObject findClosestPlayer(GameObject[] players)
        {
            if(players.Length == 1) { return players[0]; }
            float minDist = float.MaxValue;
            float dist;
            GameObject ret = null;
            foreach(GameObject p in players){
                dist = Vector3.Distance(p.transform.position, transform.position);
                if(dist < minDist)
                {
                    minDist = dist;
                    ret = p;
                }
            }
            return ret;
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

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject == _whale)
            {
                if (_returning)
                {
                    // hit whale
                    _whale.GetComponent<EnemyWhale>().TakeSardine();
                    explodeOnDeath = false;
                    Die();
                }
                return;
            }
            PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();
            if(playerCombat != null)
            {
                if (_returning) return;
                if (!playerCombat.Vulnerable)
                {
                    _target = _whale;
                    _returning = true;
                }
                else
                {
                    playerCombat.TakeDamage(_whaleCombat.Atk * _atkMultiplier, _whaleCombat);
                    // maybe add buff?
                    Die();
                }
            }
            else
            {
                Die();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _whale)
            {
                if (_returning)
                {
                    // hit whale
                    _whale.GetComponent<EnemyWhale>().TakeSardine();
                    explodeOnDeath = false;
                    Die();
                }
                return;
            }
            PlayerCombat playerCombat = other.gameObject.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                if (_returning) return;
                if (!playerCombat.Vulnerable)
                {
                    _target = _whale;
                    _returning = true;
                }
                else
                {
                    playerCombat.TakeDamage(_whaleCombat.Atk * _atkMultiplier, _whaleCombat);
                    // maybe add buff?
                    Die();
                }
            }
            else
            {
                Die();
            }
        }
    }
}
