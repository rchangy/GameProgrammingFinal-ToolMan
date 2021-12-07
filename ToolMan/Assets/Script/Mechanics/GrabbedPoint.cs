using UnityEngine;

namespace ToolMan.Mechanics
{
    public class GrabbedPoint : MonoBehaviour
    {
        public GameObject playerGameObject;
        public PlayerController anotherPlayer;
        PlayerController player;
        Rigidbody rb;
        private Collider grabbedPointCollider;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabbedPointCollider = GetComponent<SphereCollider>();
            FixedJoint fj = playerGameObject.AddComponent<FixedJoint>();
            fj.connectedBody = rb;
            fj.breakForce = 2147483647;
        }

        public void setPlayer(PlayerController player)
        {
            this.player = player;
        }

        public void resetRigidBody()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Rigidbody playerRb = player.getRigidbody();
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        public void setAnotherPlayer(PlayerController anotherPlayer)
        {
            this.anotherPlayer = anotherPlayer;
        }

        private void Update()
        {
            if (anotherPlayer != null)
            {
                //player.gameObject.transform.rotation = Quaternion.Euler(0, anotherPlayer.gameObject.transform.eulerAngles.y, -26f);
            }
        }
        public PlayerController GetPlayerController()
        {
            return player;
        }
    }
}