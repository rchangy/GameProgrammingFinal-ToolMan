using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject FreeLookCam;
    public GameObject MainCam;

    private void Awake()
    {
        EnableMain();
    }

    public void EnableFreeLook() {
        FreeLookCam.SetActive(true);
        MainCam.SetActive(true);
    }

    public void EnableMain() {
        FreeLookCam.SetActive(false);
        MainCam.SetActive(true);
        MainCam.transform.localPosition = new Vector3(0, 3, -10);
        MainCam.transform.localRotation = Quaternion.Euler(10, 0, 0);
    }
}
