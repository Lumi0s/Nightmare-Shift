using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableWithCamera : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private List<GameObject> objectsToEnable;

    
    void Update()
    {
        if(camera.activeSelf)
        {
            foreach(GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach(GameObject obj in objectsToEnable)
            {
                obj.SetActive(false);
            }
        }
    }
}
