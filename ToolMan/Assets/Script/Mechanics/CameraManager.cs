using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject FreeLookCam;
    public GameObject MainCam;
    private static Vector3 _camPosition = new Vector3(0, 3, -10);

    [SerializeField] private LayerMask layers;

    [SerializeField] private Transform FollowTarget;
    private RaycastHit hit;

    [SerializeField] private float _resetTime;
    private float _timePassedToReset;

    public static Vector3 CamLocalPos
    {
        get => _camPosition;
    }

    private void Awake()
    {
        EnableMain();
    }

    private void Update()
    {
        if (Physics.Linecast(FollowTarget.position, FollowTarget.position + FollowTarget.localRotation * _camPosition, out hit, layers))
        {
            float dist = Vector3.Distance(FollowTarget.position, hit.point);
            MainCam.transform.localPosition = new Vector3(0, 3 * (dist / 10), -(dist- 0.3f));
        }
        else
        {
            if(_timePassedToReset >= _resetTime)
            {
                _timePassedToReset = 0;
            }
            else
            {
                _timePassedToReset += Time.deltaTime;
            }
            MainCam.transform.localPosition = Vector3.Lerp(MainCam.transform.localPosition, _camPosition, _timePassedToReset / _resetTime);
        }
    }

    public void EnableFreeLook() {
        FreeLookCam.SetActive(true);
        MainCam.SetActive(true);
    }

    public void EnableMain() {
        FreeLookCam.SetActive(false);
        MainCam.SetActive(true);
        MainCam.transform.localPosition = _camPosition;
        MainCam.transform.localRotation = Quaternion.Euler(10, 0, 0);
    }
    public void LookAtFace()
    {
        EnableMain();
        Debug.Log("main cam: " + MainCam.gameObject.name);
        MainCam.transform.eulerAngles = new Vector3(MainCam.transform.eulerAngles.x, MainCam.transform.eulerAngles.y + 180f, MainCam.transform.eulerAngles.z);
        _camPosition = new Vector3(0, 3, 10);
    }
}
