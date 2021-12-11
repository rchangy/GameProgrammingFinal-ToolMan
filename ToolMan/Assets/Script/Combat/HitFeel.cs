using System.Collections;
using UnityEngine;
namespace ToolMan.Combat
{
    public class HitFeel : MonoBehaviour
    {
        private bool stopping = false;
        public float stopTimeSpan = 0.2f;
        public float slowTimeSpan = 0.2f;
        public float slowTimeScale = 0.01f;
        float originalTimeScale;

        private float _mul;

        public Camera cam;
        public float shakeRange = 0.2f;
        public float shakeSpan = 0.05f;

        private void Awake()
        {
            originalTimeScale = Time.timeScale;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                MakeHitFeel(1);
        }

        public void MakeHitFeel(float mul)
        {
            
            if (!stopping)
            {
                _mul = mul;
                stopping = true;
                StartCoroutine(TimeStop());
                stopping = false;
            }
        }

        IEnumerator TimeStop()
        {
            // Stop for a while
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(stopTimeSpan * _mul);

            // Slow down for a while
            Time.timeScale = slowTimeScale;
            yield return new WaitForSecondsRealtime(slowTimeSpan * _mul);

            Time.timeScale = originalTimeScale;

            StartCoroutine(CamShake());
        }

        IEnumerator CamShake()
        {
            Vector3 originalPosition = transform.position;

            cam.transform.position += new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange));
            yield return new WaitForSeconds(shakeSpan);
            cam.transform.position += new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange));
            yield return new WaitForSeconds(shakeSpan);

            cam.transform.position = originalPosition;
        }
    }
}