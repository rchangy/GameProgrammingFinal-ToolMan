using ToolMan.Core;
using ToolMan.Player;
using UnityEngine;

namespace ToolMan.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerToTool"></typeparam>
    public class PlayerToTool : Simulation.Event<PlayerToTool>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.audioSource && player.toToolAudio)
                player.audioSource.PlayOneShot(player.toToolAudio);
        }
    }
}