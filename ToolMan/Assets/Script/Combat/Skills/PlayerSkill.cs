using System;
using UnityEngine;

namespace ToolMan.Combat.Skills
{
    public abstract class PlayerSkill : Skill
    {

        protected GameObject _man;
        protected GameObject _tool;

        protected PlayerController _manController;
        protected PlayerController _toolController;

        protected PlayerCombat _manCombat;
        protected PlayerCombat _toolCombat;

        public bool UsingHitFeel;
        public float HitFeelMul;

        public void SetPlayers(PlayerCombat manCombat, PlayerCombat toolCombat)
        {
            _manCombat = manCombat;
            _toolCombat = toolCombat;

            _manController = manCombat.ThisPlayerController;
            _toolController = toolCombat.ThisPlayerController;

            _man = manCombat.gameObject;
            _tool = toolCombat.gameObject;
        }
    }
}
