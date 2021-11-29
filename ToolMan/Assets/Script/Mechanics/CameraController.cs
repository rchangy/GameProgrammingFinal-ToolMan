using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float shakeRange = 0.3f;
    public float shakeSpan = 0.3f;

    public void CameraShake() {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 originalPosition = transform.position;
        transform.position += new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange));

        yield return new WaitForSeconds(shakeSpan);

        transform.position = originalPosition;
    }
}
