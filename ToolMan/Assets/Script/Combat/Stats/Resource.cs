using System;
using UnityEngine;
namespace ToolMan.Combat.Stats
{
    [Serializable]
    public class Resource
    {
        private string _resourceName;
        public int MaxValue;
        private int InitValue;
        private int _currentValue;
        public int Value
        {
            get => _currentValue;
        }

        public Resource()
        {
            _currentValue = InitValue;
            CheckCurrentValue();
        }
        public Resource(string name, int maxValue, int initValue) : this()
        {
            MaxValue = maxValue;
            InitValue = initValue;
            _resourceName = name;
        }

        public void Reset(int maxValue, int initValue)
        {
            MaxValue = maxValue;
            InitValue = initValue;
            _currentValue = InitValue;
            CheckCurrentValue();
        }

        public string getName()
        {
            return _resourceName;
        }

        public void ChangeValueBy(int value)
        {
            _currentValue += value;
            CheckCurrentValue();
        }

        private void CheckCurrentValue()
        {
            _currentValue = Mathf.Min(_currentValue, MaxValue);
            _currentValue = Mathf.Max(_currentValue, 0);
        }


    }
}