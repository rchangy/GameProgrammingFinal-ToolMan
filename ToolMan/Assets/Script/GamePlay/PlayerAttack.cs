using ToolMan.Core;
using ToolMan.Player;

namespace ToolMan.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerAttack"></typeparam>
    public class PlayerAttack : Simulation.Event<PlayerAttack>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.audioSource)
            {
                if (player.boomerangWhirlAudio)
                    player.audioSource.PlayOneShot(player.boomerangWhirlAudio);
                player.playerAudioStat.lastAttackTime = 0;
            }
        }
    }
}