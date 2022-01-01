using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ToolMan.Combat.Stats
{
    [Serializable]
    public class Ability
    {
        [SerializeField]
        private string _statName;
        public bool BaseValue = true;

        private Dictionary<object, int> _disabilities = new Dictionary<object, int>();

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

        public void Disable(object source)
        {
            if (!_disabilities.ContainsKey(source))
            {
                _disabilities.Add(source, 0);
            }
            _disabilities[source]++;
        }

        public void RemoveAllDisabilities(object source)
        {
            if (_disabilities.ContainsKey(source))
            {
                _disabilities[source] = 0;
            }
        }

        public void RemoveDisability()
        {
            _disability -= 1;
            isDirty = true;
        }

        public void RemoveDisability(object source)
        {
            if (_disabilities.ContainsKey(source))
            {
                _disabilities[source]--;
            }
        }

        private bool CalculateFinalValue()
        {
            if (_disability > 0) return !BaseValue;
            foreach(int disable in _disabilities.Values.ToList())
            {
                if(disable > 0) return !BaseValue;
            }
            return BaseValue;
        }

    }

}