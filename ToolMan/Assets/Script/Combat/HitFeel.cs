using System.Collections;
using UnityEngine;
namespace ToolMan.Combat
{
    public class HitFeel : MonoBehaviour
    {
        private bool stopping = false;
        public float stopTimeSpan;
        public float slowTimeSpan;
        public float slowTimeScale = 0.01f;
        float originalTimeScale;

        private float _mul;

        public Camera cam;
        private bool shaking = false;
        public float shakeRange = 0.2f;
        public float shakeSpan = 0.05f;
        public float shakeFreq = 0.01f;

        private void Awake()
        {
            originalTimeScale = Time.timeScale;
        }

        public void MakeCamShake(float mul)
        {
            if (!shaking)
            {
                _mul = mul;
                shaking = true;
                StartCoroutine(CamShake());
                shaking = false;
            }
        }

        public void MakeTimeStop()
        {
            if (!stopping)
            {
                stopping = true;
                StartCoroutine(TimeStop());
                stopping = false;
            }
        }

        IEnumerator TimeStop()
        {
            // Stop for a while
            Time.timeScale = 0;
            Debug.Log("HitFeel stop");
            yield return new WaitForSecondsRealtime(stopTimeSpan);

            // Slow down for a while
            Debug.Log("HitFeel slow");
            Time.timeScale = slowTimeScale;
            //for (int i = 0; i < 0; i++)
            //{
            //    yield return null;
            //}
            yield return new WaitForSecondsRealtime(slowTimeSpan);

            Debug.Log("HitFeel resume");
            Time.timeScale = originalTimeScale;
        }

        IEnumerator CamShake()
        {
            float shakeTime = 0f;
            float curShakeSpan = shakeSpan * _mul;
            float curShakeRange;
            bool shaked = false;
            while(shakeTime < curShakeSpan)
            {
                curShakeRange = shakeRange * _mul * (1 - shakeTime / curShakeSpan);
                if (shaked)
                {
                    cam.transform.localPosition = CameraManager.CamLocalPos;
                    shaked = false;
                }
                else
                {
                    cam.transform.localPosition += new Vector3(Random.Range(-curShakeRange, curShakeRange), Random.Range(-curShakeRange, curShakeRange), Random.Range(-curShakeRange, curShakeRange));
                    shaked = true;
                }
                shakeTime += shakeFreq;
                yield return new WaitForSeconds(shakeFreq);
            }

            if(shaked)
                cam.transform.localPosition = CameraManager.CamLocalPos;
        }
    }
}