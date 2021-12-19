using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject FreeLookCam;
    public GameObject MainCam;
    private static Vector3 _camPosition = new Vector3(0, 3, -10);
    public static Vector3 CamLocalPos
    {
        get => _camPosition;
    }

    private void Awake()
    {
        EnableMain();
    }
    //private void Update()
    //{
    //    MainCam.transform.localPosition = _camPosition;
    //    MainCam.transform.localRotation = Quaternion.Euler(10, 0, 0);
    //}

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
}
