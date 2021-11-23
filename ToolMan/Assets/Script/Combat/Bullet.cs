using UnityEngine;
using System.Collections;
namespace ToolMan.Combat
{
    public class Bullet : MonoBehaviour
    {
        public CombatUnit shooter;
        public int Atk = 0;
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Bullet Hit " + collision.gameObject.name);
            var target = collision.gameObject.GetComponent<CombatUnit>();
            if (target != null)
            {
                target.TakeDamage(Atk, shooter);
            }
            Destroy(gameObject);
        }
    }
}