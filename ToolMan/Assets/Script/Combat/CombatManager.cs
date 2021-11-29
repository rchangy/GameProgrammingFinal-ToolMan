using UnityEngine;
using System.Collections;

namespace ToolMan.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField]
        private CombatModel _model;
        public CombatModel Model
        {
            get => _model;
        }
        private void Awake()
        {
            //Model.DmgCalculator.Load();
            //Model.ComboSkills.Load();
        }
    }
}
