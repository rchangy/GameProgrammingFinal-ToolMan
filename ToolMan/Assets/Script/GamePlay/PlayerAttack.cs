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
                if (player.inToolState() && player.getTool().getName().Equals("Boomerang") && player.boomerangWhirlAudio)
                    player.audioSource.PlayOneShot(player.boomerangWhirlAudio);
                if (player.inToolState() && player.getTool().getName().Equals("FlashBomb") && player.flashBombAttackAudio)
                    player.audioSource.PlayOneShot(player.flashBombAttackAudio);
                if (player.inToolState() && player.getTool().getName().Equals("Shield") && player.shieldDefendAudio)
                    player.audioSource.PlayOneShot(player.shieldDefendAudio);
                player.playerAudioStat.lastAttackTime = 0;
            }
        }
    }
}