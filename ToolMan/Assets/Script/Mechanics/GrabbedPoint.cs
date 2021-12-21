using UnityEngine;

namespace ToolMan.Mechanics
{
    public class GrabbedPoint : MonoBehaviour
    {
        public GameObject playerGameObject;
        public PlayerController anotherPlayer;
        PlayerController player;
        private Collider grabbedPointCollider;

        public void setPlayer(PlayerController player)
        {
            this.player = player;
        }

        public void setAnotherPlayer(PlayerController anotherPlayer)
        {
            this.anotherPlayer = anotherPlayer;
        }
        public PlayerController GetPlayerController()
        {
            return player;
        }
    }
}