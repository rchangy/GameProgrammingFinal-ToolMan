using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
namespace ToolMan.Combat.Skills
{
    // save a set of skills
    [CreateAssetMenu(menuName = "ToolMan/Skill/SkillSet")]
    public class SkillSet : ScriptableObject
    {
        // contains every skill that a combat unit can use 
        protected Dictionary<string, Skill> skillSet = new Dictionary<string, Skill>();

        // for serialize
        [SerializeField]
        protected List<Skill> skills;


        public bool HasSkill(string name)
        {
            return skillSet.ContainsKey(name);
        }

        public Skill GetSkillbyName(string name)
        {
            return skillSet[name];
        }

        public virtual void Load()
        {
            foreach(Skill s in skills)
            {
                if (!skillSet.ContainsKey(s.getName()))
                {
                    skillSet.Add(s.getName(), s);
                    s.ResetObject();
                }
            }
        }

        public List<string> GetSkills()
        {
            return skillSet.Keys.ToList();
        }
    }
}