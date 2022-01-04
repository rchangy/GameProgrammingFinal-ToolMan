using ToolMan.Core;
using ToolMan.Player;
using UnityEngine;

namespace ToolMan.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerHit"></typeparam>
    public class PlayerHit : Simulation.Event<PlayerHit>
    {
        public PlayerController player;
        public float waitForNext = 0.4f;

        public override void Execute()
        {
            if (player.audioSource && player.playerAudioStat.lastHitTime > waitForNext)
            {
                if (player.getTool().getName().Equals("Pickaxe") && player.pickaxeHitAudio)
                    player.audioSource.PlayOneShot(player.pickaxeHitAudio);
                player.playerAudioStat.lastHitTime = 0;
            }
        }
    }
}