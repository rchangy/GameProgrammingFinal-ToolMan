using UnityEngine;

namespace ToolMan.Mechanics
{
    public class GrabPoint : MonoBehaviour
    {
        public float grabRange = 1.5f;
        public LayerMask grabbedPointLayer;
        GameObject targetTool;
        public bool grabbing = false;
        private PlayerController anotherPlayer;

        Rigidbody rb;
        PlayerController player;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void setPlayer(PlayerController p) { player = p; }

        // Update is called once per frame
        void Update()
        {
            if (!player.inToolState())
            {
                if ((Input.GetButtonDown("Grab1") && (player.playerNum == 1)) || (Input.GetButtonDown("Grab2") && (player.playerNum == 2)))
                {
                    Debug.Log("Press Grab1");
                    if (grabbing)
                    {
                        Debug.Log("Press Release");
                        Release();
                    }
                    else
                        Grab();
                }

                Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, grabbedPointLayer);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != gameObject)
                    {
                        anotherPlayer = collider.transform.parent.gameObject.GetComponent<PlayerController>();
                        if (anotherPlayer != null && anotherPlayer.inToolState()) targetTool = collider.gameObject;
                    }

                }
                if (colliders.Length == 0 && !grabbing)
                {
                    targetTool = null;
                }
            }
        }

        public void Grab()
        {
            Debug.Log("player: " + player.playerNum + " grab player " + anotherPlayer.playerNum);
            Debug.Log("In Grab");
            if (targetTool != null)
            {
                //LayerMask mask = LayerMask.GetMask("Player");
                //int layer = mask.value;
                Physics.IgnoreCollision(player.gameObject.GetComponent<Collider>(), anotherPlayer.gameObject.GetComponent<Collider>(), true);
                //Physics.IgnoreLayerCollision(layer, layer, true);
                anotherPlayer.getGrabbedPoint().GetComponent<Collider>().isTrigger = true;
                player.getGrabbedPoint().GetComponent<Collider>().isTrigger = true;


                grabbing = true;
                anotherPlayer.transform.position = new Vector3(player.getRightHand().transform.position.x - anotherPlayer.getGrabbedPoint().transform.localPosition.x, player.getRightHand().transform.position.y - anotherPlayer.getGrabbedPoint().transform.localPosition.y + 1, player.getRightHand().transform.position.z - anotherPlayer.getGrabbedPoint().transform.localPosition.z);
                //anotherPlayer.transform.position = new Vector3(player.getRightHand().transform.position.x, player.getRightHand().transform.position.y - 10, player.getRightHand().transform.position.z);
                //Debug.Log("grab");
                //// set FixedJoint
                ////FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                //FixedJoint fj = player.getRightHand().gameObject.AddComponent<FixedJoint>();
                //fj.connectedBody = anotherPlayer.getRigidbody();
                //fj.breakForce = 2147483847;
                //fj.autoConfigureConnectedAnchor = false;
                //fj.connectedAnchor = anotherPlayer.getTool().getPoint();
                //fj.enableCollision = false;

                //// set Tool in the status of being grabbed


                //---
                Vector3 orignialPos = player.gameObject.transform.position;
                //CharacterJoint cj = player.getRightHand().gameObject.AddComponent<CharacterJoint>();
                //cj.connectedBody = anotherPlayer.getRigidbody();
                //cj.autoConfigureConnectedAnchor = false;
                //cj.connectedAnchor = anotherPlayer.getTool().getPoint();
                //cj.enableCollision = false;
                
                //SoftJointLimitSpring newLimit = new SoftJointLimitSpring();
                //newLimit.damper = 10;
                //cj.swingLimitSpring = newLimit;
                //cj.anchor = Vector3.zero;
                //cj.targetRotation = Quaternion.Euler(0, 0, 26f);

                ConfigurableJoint confJ = player.getRightHand().gameObject.AddComponent<ConfigurableJoint>();
                confJ.connectedBody = anotherPlayer.getRigidbody();
                confJ.autoConfigureConnectedAnchor = false;
                confJ.connectedAnchor = anotherPlayer.getTool().getPoint();
                confJ.enableCollision = false;

                anotherPlayer.beGrabbed(player);
                player.transform.position = orignialPos;
                player.ResetRigidbody();
            }
        }

        public void Release()
        {
            if (targetTool != null)
            {
                //Destroy(player.getRightHand().gameObject.GetComponent<FixedJoint>());
                //Destroy(player.getRightHand().gameObject.GetComponent<CharacterJoint>());
                Destroy(player.getRightHand().gameObject.GetComponent<ConfigurableJoint>());
                //Destroy(gameObject.GetComponent<FixedJoint>());
                // Reset grabbed player rigidbody
                targetTool.GetComponent<GrabbedPoint>().resetRigidBody();

                // Reset grabbing player rigidbody? not sure if need this
                player.Release();

                // set Tool in the status of being released
                anotherPlayer.beReleased();
                //LayerMask mask = LayerMask.GetMask("Player");
                //int layer = mask.value;
                Physics.IgnoreCollision(player.gameObject.GetComponent<Collider>(), anotherPlayer.gameObject.GetComponent<Collider>(), false);
                //Physics.IgnoreLayerCollision(layer, layer, false);
                anotherPlayer.getGrabbedPoint().GetComponent<Collider>().isTrigger = false;
                player.getGrabbedPoint().GetComponent<Collider>().isTrigger = false;
                anotherPlayer.getRigidbody().isKinematic = false;

                grabbing = false;
                Debug.Log("release");
            }
        }

        public void setAnotherPlayerAndTarget(PlayerController anotherPlayer)
        {
            this.anotherPlayer = anotherPlayer;
            this.targetTool = anotherPlayer.getGrabbedPoint();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, grabRange);
        }
    }
}