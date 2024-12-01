using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Vector3 basePosition;
    private Quaternion baseRotation;

    private Vector3 monitorPosition;
    private Quaternion monitorRotation;

    private bool movingToMonitor = false;
    private bool isMoving = false;
    private RotateCamera rotateCamera;

    public float transitionSpeed = 5f;
    public float positionThreshold = 0.05f;
    public float rotationThreshold = 1f;
    public float maxTransitionTime = 2f;

    // Add new field to store rotation
    private float lastYRotation;

    void Start()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;
        rotateCamera = GetComponent<RotateCamera>();

        monitorPosition = new Vector3(-0.024f, 0.066f, -2.07f);
        monitorRotation = Quaternion.Euler(13.863f, 180.594f, 359.959f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMoving)
        {
            if (movingToMonitor)
            {
                StartCoroutine(MoveToPosition(basePosition, baseRotation, false));
            }
            else
            {
                if (rotateCamera != null)
                {
                    // Store current Y rotation before disabling
                    lastYRotation = transform.localRotation.eulerAngles.y;
                    rotateCamera.enabled = false;
                }
                StartCoroutine(MoveToPosition(monitorPosition, monitorRotation, true));
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation, bool toMonitor)
    {
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        // Store target euler angles
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        if (!toMonitor)
        {
            // Use stored Y rotation when returning from monitor
            targetEulerAngles.y = lastYRotation;
        }

        while (elapsedTime < maxTransitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / maxTransitionTime;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);

            Vector3 currentEuler = transform.localRotation.eulerAngles;
            Vector3 newEuler = new Vector3(
                Mathf.LerpAngle(currentEuler.x, targetEulerAngles.x, smoothT),
                Mathf.LerpAngle(currentEuler.y, targetEulerAngles.y, smoothT),
                Mathf.LerpAngle(currentEuler.z, targetEulerAngles.z, smoothT)
            );
            transform.localRotation = Quaternion.Euler(newEuler);

            if (Vector3.Distance(transform.localPosition, targetPosition) <= positionThreshold &&
                Quaternion.Angle(transform.localRotation, Quaternion.Euler(targetEulerAngles)) <= rotationThreshold)
            {
                break;
            }

            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = Quaternion.Euler(targetEulerAngles);

        if (!toMonitor && rotateCamera != null)
        {
            // Update RotateCamera's current angle before re-enabling
            rotateCamera.currentAngle = lastYRotation;
            rotateCamera.enabled = true;
        }

        movingToMonitor = toMonitor;
        isMoving = false;
    }
}