using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private KeyCode openCameras;
    [SerializeField] private List<GameObject> cameras;
    private int currentCam = 0;
    private bool camerasOpen;




    void Start()
    {
        foreach(var camera in cameras)
        {
            camera.SetActive(false);
        }
    }

   
    void Update()
    {
        if(Input.GetKeyDown(openCameras))
        {
            camerasOpen = !camerasOpen;
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        if(camerasOpen)
        {
            cameras[currentCam].SetActive(true);
        }
        else
        {
            cameras[currentCam].SetActive(false);
        }
    }
}
