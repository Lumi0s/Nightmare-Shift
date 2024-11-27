using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float maxAngle = 45f; 
    [SerializeField] private float rotationSpeed;
    private float currentAngle = 180f;

    void Update()
    {
        float mouseX = Input.mousePosition.x / Camera.main.pixelWidth;
        float rotationAmount = 0f;

        if (mouseX < 0.2f)
        {
            rotationAmount = -rotationSpeed * Time.deltaTime;
        }
        else if (mouseX > 0.8f)
        {
            rotationAmount = rotationSpeed * Time.deltaTime;
        }

        currentAngle += rotationAmount;
        currentAngle = Mathf.Clamp(currentAngle, 180f - maxAngle, 180f + maxAngle);
        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }
}
