using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Vector3 basePosition;
    private Quaternion baseRotation;

    private Vector3 monitorPosition;
    private Quaternion monitorRotation;

    public bool isAtMonitor = false;
    public bool isMoving = false;

    public float transitionDuration = 2f;

    [SerializeField] private AnimationCurve positionCurve;
    [SerializeField] private AnimationCurve rotationCurveX;  // Separate curve for X rotation
    [SerializeField] private AnimationCurve rotationCurveY;  // Separate curve for Y rotation
    [SerializeField] private AnimationCurve returnRotationCurveX; // Return X rotation
    [SerializeField] private AnimationCurve returnRotationCurveY; // Return Y rotation

    private float lastYRotation;

    void Start()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;

        monitorPosition = new Vector3(-0.024f, 0.066f, -2.07f);
        monitorRotation = Quaternion.Euler(13.863f, 180.594f, 359.959f);
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation, bool toMonitor)
    {
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.localPosition;
        Vector3 startEuler = transform.localRotation.eulerAngles;
        Vector3 targetEuler = targetRotation.eulerAngles;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float posT = positionCurve.Evaluate(t);

            // Separate X and Y rotation interpolation
            float rotTX = toMonitor ? rotationCurveX.Evaluate(t) : returnRotationCurveX.Evaluate(t);
            float rotTY = toMonitor ? rotationCurveY.Evaluate(t) : returnRotationCurveY.Evaluate(t);

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, posT);

            Vector3 newEuler = new Vector3(
                Mathf.LerpAngle(startEuler.x, targetEuler.x, rotTX),
                Mathf.LerpAngle(startEuler.y, targetEuler.y, rotTY),
                startEuler.z  // Keep Z rotation unchanged
            );

            transform.localRotation = Quaternion.Euler(newEuler);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;

        isAtMonitor = toMonitor;
        isMoving = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMoving)
        {
            if (isAtMonitor)
            {
                StartCoroutine(MoveToPosition(basePosition, baseRotation, false));
            }
            else
            {
                StartCoroutine(MoveToPosition(monitorPosition, monitorRotation, true));
            }
        }
    }
}