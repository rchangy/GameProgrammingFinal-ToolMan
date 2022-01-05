using ToolMan.Core;
using ToolMan.Player;
using UnityEngine;

namespace ToolMan.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerWalking"></typeparam>
    public class PlayerWalking : Simulation.Event<PlayerWalking>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.audioSource && player.walkAudio)
                player.audioSource.PlayOneShot(player.walkAudio);
            player.playerAudioStat.lastWalkTime = 0;
        }
    }
}