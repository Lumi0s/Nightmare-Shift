using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float maxAngle = 45f; 

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.mousePosition.x / Camera.main.pixelWidth;
        float rotationAngle = (mouseX - 0.5f) * 2 * maxAngle;
        transform.rotation = Quaternion.Euler(0, rotationAngle+180, 0);
    }
}
