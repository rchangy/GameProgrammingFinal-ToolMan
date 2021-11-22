using UnityEngine;
using System;

namespace ToolMan.Combat.Stats
{
    [Serializable]
    public class Ability
    {
        [SerializeField]
        private string _statName;
        public bool BaseValue = true;

        private int _disability;

        private bool isDirty = true;

        [SerializeField]
        private bool _value;
        [SerializeField]
        public bool Value
        {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }


        public Ability()
        {
            _disability = 0;
            isDirty = true;
        }
        public Ability(string name, bool baseValue) : this()
        {
            BaseValue = baseValue;
            _statName = name;
        }

        public string getName()
        {
            return _statName;
        }


        public void Disable()
        {
            _disability += 1;
            isDirty = true;
        }

        public void RemoveDisability()
        {
            _disability -= 1;
            isDirty = true;
        }

        private bool CalculateFinalValue()
        {
            if (_disability > 0) return !BaseValue;
            else return BaseValue;
        }

    }

}