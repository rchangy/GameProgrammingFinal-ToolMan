using UnityEngine;
using ToolMan.Util;

namespace ToolMan.Mechanics
{
    public class GrabPoint : MonoBehaviour
    {
        [SerializeField] private float grabRange = 1.5f;
        [SerializeField] private LayerMask grabbedPointLayer;
        
        private bool grabbing = false;
        private GameObject targetPoint; // Tool's grabbed point
        private PlayerController player;
        private PlayerController anotherPlayer;

        // unused?
        private Rigidbody rb;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void setPlayer(PlayerController p)
        {
            player = p;
        }

        // Update is called once per frame
        void Update()
        {
            if (!player.inToolState())
            {
                DetectCollisionWithGrabbedPoint();
            }
        }

        public void GrabOrRelease()
        {
            if (grabbing)
            {
                Debug.Log("Press Release");
                Release();
            }
            else
            {
                Debug.Log("Press Grab");
                Grab();
            }
        }

        private void DetectCollisionWithGrabbedPoint()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, grabbedPointLayer);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    anotherPlayer = collider.gameObject.GetComponent<GrabbedPoint>().GetPlayerController();
                    if (anotherPlayer != null && anotherPlayer.inToolState())
                    {
                        Debug.Log("can grab grabbed : )");
                        targetPoint = collider.gameObject;
                    }
                }

            }
            if (colliders.Length == 0 && !grabbing)
            {
                targetPoint = null;
            }
        }

        public void Grab()
        {
            if (targetPoint != null)
            {
                int playerLayer = Converter.LayerBitMaskToLayerNumber(player.GetLayerMask().value);
                Debug.Log("playerLayer = " + playerLayer);
                Physics.IgnoreLayerCollision(playerLayer, playerLayer, true);
                // change position first
                Vector3 grabbedPointLocalPosition = anotherPlayer.GetGrabbedPoint().transform.localPosition;
                Vector3 anotherPlayerNewPosition = player.GetRightHand().transform.position - grabbedPointLocalPosition;
                anotherPlayer.transform.position = anotherPlayerNewPosition;


                //ConfigurableJoint confJ = gameObject.AddComponent<ConfigurableJoint>();
                ConfigurableJoint confJ = player.gameObject.AddComponent<ConfigurableJoint>();
                confJ.connectedBody = anotherPlayer.getRigidbody();
                confJ.autoConfigureConnectedAnchor = false;
                confJ.anchor = player.GetRightHand().transform.localPosition;
                confJ.connectedAnchor = anotherPlayer.getTool().getPoint();
                confJ.enableCollision = false;
                confJ.xMotion = ConfigurableJointMotion.Locked;
                confJ.yMotion = ConfigurableJointMotion.Locked;
                confJ.zMotion = ConfigurableJointMotion.Locked;
                //confJ.targetRotation = Quaternion.Euler(60f, 0, 30f);

                //// set FixedJoint
                //FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                ////FixedJoint fj = player.gameObject.AddComponent<FixedJoint>();
                //fj.connectedBody = anotherPlayer.getRigidbody();
                //fj.breakForce = 2147483847;
                //fj.autoConfigureConnectedAnchor = false;
                ////fj.anchor = player.GetRightHand().transform.localPosition;
                //fj.connectedAnchor = anotherPlayer.getTool().getPoint();
                //fj.enableCollision = false;

                // set Tool in the status of being grabbed
                anotherPlayer.BeGrabbed(player);

                grabbing = true;
                Debug.Log("grab");
            }
        }

        public void Release()
        {
            if (targetPoint != null)
            {
                //Destroy(gameObject.GetComponent<ConfigurableJoint>());
                Destroy(player.gameObject.GetComponent<ConfigurableJoint>());
                //Destroy(gameObject.GetComponent<FixedJoint>());
                // Reset grabbed player rigidbody
                //targetPoint.GetComponent<GrabbedPoint>().resetRigidBody();

                // Reset grabbing player rigidbody? not sure if need this
                player.Release();

                // set Tool in the status of being released
                anotherPlayer.BeReleased();

                grabbing = false;
                int playerLayer = Converter.LayerBitMaskToLayerNumber(player.GetLayerMask().value);
                Physics.IgnoreLayerCollision(playerLayer, playerLayer, false);
                Debug.Log("release");
            }
        }

        public void setAnotherPlayerAndTarget(PlayerController anotherPlayer)
        {
            this.anotherPlayer = anotherPlayer;
            this.targetPoint = anotherPlayer.GetGrabbedPoint().gameObject;
        }

        public bool IsGrabbing()
        {
            return grabbing;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, grabRange);
        }
    }
}