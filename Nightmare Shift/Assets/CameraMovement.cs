using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField] private float pauseDuration = 1f;

    private float currentAngle = 0f;
    private bool rotatingLeft = true;
    private bool isPaused = true;
    private Coroutine pauseCoroutine;
    private float remainingPauseTime;
    private float pauseCoroutineStartTime;

    void Start()
    {
        remainingPauseTime = pauseDuration;
        pauseCoroutineStartTime = pauseDuration;
    }

    void Update()
    {
        if (!isPaused)
        {
            float rotationStep = rotationSpeed * Time.deltaTime * (rotatingLeft ? -1 : 1);
            currentAngle += rotationStep;
            currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

            transform.Rotate(0f, rotationStep, 0f);

            if (Mathf.Abs(currentAngle) >= maxAngle)
            {
                isPaused = true;
                pauseCoroutine = StartCoroutine(PauseRotation());
            }
        }
    }

    IEnumerator PauseRotation()
    {
        pauseCoroutineStartTime = Time.time;
        while (Time.time - pauseCoroutineStartTime < remainingPauseTime)
        {
            yield return null;
        }
        rotatingLeft = !rotatingLeft;
        isPaused = false;
        remainingPauseTime = pauseDuration;
    }

    void OnDisable()
    {
        if (isPaused && Time.time > 0.1f)
        {
            StopCoroutine(pauseCoroutine);
            remainingPauseTime = Time.time - pauseCoroutineStartTime;
            isPaused = false;
        }
        else
        {
            remainingPauseTime = pauseDuration;
        }
    }

    void OnEnable()
    {
        if (isPaused && Time.time > 0.1f)
        {
            pauseCoroutine = StartCoroutine(PauseRotation());
        }
    }
}