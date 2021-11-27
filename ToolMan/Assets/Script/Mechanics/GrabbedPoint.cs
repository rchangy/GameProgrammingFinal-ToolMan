using UnityEngine;

namespace ToolMan.Mechanics
{
    public class GrabbedPoint : MonoBehaviour
    {
        public GameObject playerGameObject;
        public PlayerController anotherPlayer;
        PlayerController player;
        Rigidbody rb;

        public void FixGrabbedPoint()
        {
            rb = GetComponent<Rigidbody>();
            //FixedJoint fj = playerGameObject.AddComponent<FixedJoint>();
            //fj.connectedBody = rb;
            //fj.breakForce = 2147483647;
            //FixedJoint fj = gameObject.AddComponent<FixedJoint>();
            //fj.connectedBody = player.getRigidbody();
            //fj.breakForce = 2147483647;
            //fj.autoConfigureConnectedAnchor = false;
            //fj.connectedAnchor = new Vector3(0.0f, -1.2f, 0.0f);
        }

        public void SetPlayer(PlayerController player)
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
                playerGameObject.transform.forward = anotherPlayer.gameObject.transform.forward;
                //player.getGrabbedPoint().transform.position = anotherPlayer.RightHand.transform.position;
                //playerGameObject.transform.localRotation = Quaternion.E
                //playerGameObject.transform.rotation = Quaternion.Euler(playerGameObject.transform.rotation.x, playerGameObject.transform.rotation.y, 26f);
                //playerGameObject.transform.rotation = Quaternion.LookRotation(new Vector3())
                //playerGameObject.transform.rotation = Quaternion.Euler(playerGameObject.transform.rotation.x, playerGameObject.transform.rotation.y, 26f);
                //Debug.Log(anotherPlayer.RightHand.transform.position);
                //Debug.Log(gameObject.transform.position);
            }
        }

        // ==== getters ====
        public Rigidbody GetRigidbody()
        {
            return rb;
        }
    }
}