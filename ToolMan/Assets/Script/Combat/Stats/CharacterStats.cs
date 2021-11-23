using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ToolMan.Combat.Stats.Buff;

namespace ToolMan.Combat.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        // mandatory stats
        [SerializeField]
        private int HpMaxValue = 1;
        [SerializeField]
        private int HpInitValue = 1;
        [SerializeField]
        private float AtkBaseValue = 1f;
        [SerializeField]
        private float DefBaseValue = 1f;
        [SerializeField]
        private float StrBaseValue = 1f;


        public List<Ability> AbilityInfos = new List<Ability>();
        public List<Stat> StatInfos = new List<Stat>();
        public List<Resource> ResourceInfos = new List<Resource>();


        private Dictionary<string, Stat> _stats = new Dictionary<string, Stat>();
        private Dictionary<string, Resource> _resources = new Dictionary<string, Resource>();
        private Dictionary<string, Ability> _abilities = new Dictionary<string, Ability>();

        private readonly Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();

        [SerializeField]
        private List<string> _inherentTypes;

        private HashSet<string> _types = new HashSet<string>();
        public IReadOnlyCollection<string> Types;

        private void Awake()
        {
            // must have
            _stats.Add("ATK", new Stat("ATK", AtkBaseValue));
            _stats.Add("ASPD", new Stat("ASPD", 1));
            _stats.Add("DEF", new Stat("DEF", DefBaseValue));
            _stats.Add("STR", new Stat("STR", StrBaseValue));
            _resources.Add("HP", new Resource(HpMaxValue, HpInitValue));

            _abilities.Add("AttackEnabled", new Ability("AttackEnabled", true));
            _abilities.Add("Movable", new Ability("Movable", true));
            _abilities.Add("Vulnerable", new Ability("Vulnerable", true));

            foreach (Stat s in StatInfos)
            {
                if (!_stats.ContainsKey(s.getName()))
                    _stats.Add(s.getName(), s);
            }
            foreach (Resource r in ResourceInfos)
            {
                if (!_resources.ContainsKey(r.getName()))
                    _resources.Add(r.getName(), r);
            }
            foreach (Ability a in AbilityInfos)
            {
                if (!_abilities.ContainsKey(a.getName()))
                    _abilities.Add(a.getName(), a);
            }

            if (_inherentTypes != null)
            {
                foreach (string s in _inherentTypes)
                {
                    _types.Add(s);
                }
            }

            Types = _types;
        }

        void Update()
        {
            foreach (var buff in _buffs.Values.ToList())
            {
                buff.Tick(Time.deltaTime);
                if (buff.IsFinished)
                {
                    _buffs.Remove(buff.Buff);
                }
            }
        }

        public void AddBuff(ScriptableBuff scriptableBuff, TimedBuff buff)
        {
            if (_buffs.ContainsKey(scriptableBuff))
            {
                _buffs[scriptableBuff].Activate();
            }
            else
            {
                _buffs.Add(scriptableBuff, buff);
                buff.Activate();
            }
        }

        public bool HasBuff(ScriptableBuff buff)
        {
            return _buffs.ContainsKey(buff);
        }

        public Stat GetStatByName(string name)
        {
            if (_stats.ContainsKey(name))
            {
                return _stats[name];
            }
            else
            {
                Debug.Log(this.gameObject.name + " has no stat " + name);
                return null;
            }
        }

        public bool HasStat(string name)
        {
            return _stats.ContainsKey(name);
        }

        public Resource GetResourceByName(string name)
        {
            if (_resources.ContainsKey(name))
            {
                return _resources[name];
            }
            else
            {
                Debug.Log(this.gameObject.name + " has no resource " + name);
                return null;
            }
        }

        public bool HasResource(string name)
        {
            return _resources.ContainsKey(name);
        }

        public Ability GetAbilityByName(string name)
        {
            if (_abilities.ContainsKey(name))
            {
                return _abilities[name];
            }
            else
            {
                Debug.Log(this.gameObject.name + " has no ability " + name);
                return null;
            }
        }

        public bool HasAbility(string name)
        {
            return _abilities.ContainsKey(name);
        }

        public IReadOnlyCollection<string> GetCurrentTypes()
        {
            return Types;
        }

        public void AddType(string type)
        {
            _types.Add(type);
        }

        public void RemoveType(string type)
        {
            if (_types.Contains(type) && !_inherentTypes.Contains(type))
            {
                _types.Remove(type);
            }
        }


    }
}
