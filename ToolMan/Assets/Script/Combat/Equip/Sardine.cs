using UnityEngine;
using System.Collections;

namespace ToolMan.Combat.Equip
{
    public class Sardine : MonoBehaviour
    {
        private GameObject _targetPlayer;

        protected override void Start()
        {
            base.Start();
            // find target
            GameObject.FindGameObjectsWithTag("Player");
        }
        protected override void Update()
        {

        }
    }
}
