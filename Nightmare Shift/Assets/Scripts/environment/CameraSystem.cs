using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private KeyCode openCameras;
    [SerializeField] private List<GameObject> cameras;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject cameraUI;
    private int currentCam = 0;
    private bool camerasOpen;





    void Start()
    {
        foreach(var camera in cameras)
        {
            camera.SetActive(false);
        }
        cameraUI.SetActive(false);
        SoundManager.Instance.PlaySound("Fan");
    }

   
    void Update()
    {
        if(Input.GetKeyDown(openCameras))
        {
            camerasOpen = !camerasOpen;
            ShowCamera();
            if(camerasOpen)
            {
                PowerSystem.Instance.usage++;
                SoundManager.Instance.PlaySound("CamerasUp");
                SoundManager.Instance.StopSound("Fan");
            }
            else
            {
                PowerSystem.Instance.usage--;
                SoundManager.Instance.PlaySound("CamerasDown");
                SoundManager.Instance.StopSound("CamerasUp");
                SoundManager.Instance.PlaySound("Fan");
            }

        }
    }

    private void ShowCamera()
    {
        if(camerasOpen)
        {
            cameras[currentCam].SetActive(true);
            mainCamera.SetActive(false);
            cameraUI.SetActive(true);


        }
        else
        {
            cameras[currentCam].SetActive(false);
            mainCamera.SetActive(true);
            cameraUI.SetActive(false);
        }
    }

    public void setCamera(int camera)
    {
        cameras[currentCam].SetActive(false);
        currentCam = camera;
        ShowCamera();
        SoundManager.Instance.PlaySound("CamerasSwitch");
    }
}
