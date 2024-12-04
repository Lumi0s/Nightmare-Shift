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

        monitorPosition = new Vector3(-0.00999999978f, 0.0270000007f, -2.19300008f);
        monitorRotation = Quaternion.Euler(7.068995f, 180f, 0f);
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
            if (GameManager.Instance.lostGame) { yield break; }
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
        if (CameraSystem.Instance.camerasOpen) { return; }
        if (Input.GetKeyDown(PlaceholderWinningSystem.Instance.openUI) && !isMoving)
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