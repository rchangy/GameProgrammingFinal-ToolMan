using System.Collections;
using UnityEngine;

public class HitFeel : MonoBehaviour
{
    private bool stopping = false;
    public float stopTimeSpan = 0.2f;
    public float slowTimeSpan = 0.2f;
    public float slowTimeScale = 0.01f;
    float originalTimeScale;

    public CameraController cam;
    public float shakeSpan = 0.1f;
    public float shakeAmplitude = 1f;
    public float shakeFrequency = 1f;

    private void Awake()
    {
        originalTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            MakeHitFeel();
    }

    public void MakeHitFeel()
    {
        if (!stopping)
        {
            stopping = true;            
            StartCoroutine(TimeStop());
            cam.CameraShake(shakeSpan, shakeAmplitude, shakeFrequency);
            stopping = false;
        }
    }

    IEnumerator TimeStop()
    {
        // Stop for a while
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(stopTimeSpan);
        
        // Slow down for a while
        Time.timeScale = slowTimeScale;
        yield return new WaitForSecondsRealtime(slowTimeSpan);
        
        Time.timeScale = originalTimeScale;
    }
}
