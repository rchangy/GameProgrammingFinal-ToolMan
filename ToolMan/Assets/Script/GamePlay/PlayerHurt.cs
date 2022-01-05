using ToolMan.Core;
using ToolMan.Player;
using UnityEngine;

namespace ToolMan.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerHit"></typeparam>
    public class PlayerHurt : Simulation.Event<PlayerHurt>
    {
        public PlayerController player;
        public float waitForNext = 0.2f;

        public override void Execute()
        {
            if (player.audioSource && player.playerAudioStat.lastHurtTime > waitForNext)
            {
                if (player.hurtAudio)
                    player.audioSource.PlayOneShot(player.hurtAudio);
                player.playerAudioStat.lastHurtTime = 0;
            }
        }
    }
}