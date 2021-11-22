using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    CinemachineFreeLook cam;
    CinemachineInputProvider inputProvider;

    public InputActionReference lookAction;
    public int index = 0;
    public int GetIndex() { return index; }

    private void Awake()
    {
        cam = gameObject.GetComponent<CinemachineFreeLook>();
        inputProvider = gameObject.GetComponent<CinemachineInputProvider>();
        DisableCam();
    }

    public void EnableCam() {
        inputProvider.XYAxis = lookAction;
    }

    public void DisableCam() {
        inputProvider.XYAxis = null;
    }

    public void CameraShake(float shakeSpan, float amplitude, float frequency) {
        StartCoroutine(_Shake(shakeSpan, amplitude, frequency));
    }

    IEnumerator _Shake(float shakeSpan, float amplitude, float frequency)
    {
        Shake(amplitude, frequency);
        yield return new WaitForSeconds(shakeSpan);
        Shake(0, 0);
    }
    void Shake(float amplitude, float frequency)
    {
        cam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        cam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        cam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }
}
