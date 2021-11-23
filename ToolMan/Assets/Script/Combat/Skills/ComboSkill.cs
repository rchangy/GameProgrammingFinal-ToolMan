using UnityEngine;
using System;
namespace ToolMan.Combat.Skills
{
    public abstract class ComboSkill : Skill
    {
        protected string _preTool;
        protected string _postTool;
        protected int _cost;
        public int Cost
        {
            get => _cost;
        }

        public string getPreTool() { return _preTool; }
        public string getPostTool() { return _postTool; }

        public bool CheckEnergyCost(PlayerCombat combat)
        {
            return combat.Energy >= Cost;
        }
    }
}