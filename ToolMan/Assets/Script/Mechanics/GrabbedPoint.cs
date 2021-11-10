using UnityEngine;

public class GrabbedPoint : MonoBehaviour
{
    public GameObject playerGameObject;
    public PlayerController anotherPlayer;
    PlayerController player;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        //Debug.Log("ccc");
    }

    public void setAnotherPlayer(PlayerController anotherPlayer)
    {
        this.anotherPlayer = anotherPlayer;
        //Vector3 distance;
        //if (anotherPlayer != null)
        //{
        //    distance = anotherPlayer.RightHand.transform.position - gameObject.transform.position;
        //    gameObject.transform.position = anotherPlayer.RightHand.transform.position;
        //    distance = anotherPlayer.RightHand.transform.position - gameObject.transform.position;
        //    playerGameObject.transform.position += distance;
        //}
        
        //
        //Debug.Log("distance = " + distance);
        //
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
}
