using ToolMan.Core;
using ToolMan.Player;
using UnityEngine;

namespace ToolMan.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerAddHP"></typeparam>
    public class PlayerAddHP : Simulation.Event<PlayerAddHP>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.audioSource && player.addHPAudio)
                player.audioSource.PlayOneShot(player.addHPAudio);
        }
    }
}