using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet Hit " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
