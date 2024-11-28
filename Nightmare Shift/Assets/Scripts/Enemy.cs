using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveInterval = 5.0f;
    public Room currentRoom;

    [Range(0f, 1f)]
    public float chanceToMove = 0.5f;
    private float timeSinceLastMove = 0.0f;
    private GameObject mainCamera;
    [SerializeField]private GameObject jumpscareAnimation;

    // Start is called before the first frame update
    void Start()
    {
        currentRoom = GetComponentInParent<Room>();
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frames
    void Update()
    {
        if (GameManager.Instance.lostGame) { return; }
        timeSinceLastMove += Time.deltaTime;

        if (timeSinceLastMove >= moveInterval)
        {
            float random = UnityEngine.Random.Range(0.0f, 1.0f);
            if (random < chanceToMove)
            {
                move();
            }
            timeSinceLastMove = 0.0f;
        }
    }

    public void move()
    {

        if (currentRoom != null)
        {
            currentRoom.moveEnemy(this.gameObject);
            if(currentRoom.gameObject.name == "Office")
            {
                Jumpscare();
            }
        }
        else
        {
            Debug.LogError("currentRoom is null. Cannot move enemy.");
        }
    }
    void Jumpscare()
    {
        GameManager.Instance.lostGame = true;
        if (CameraSystem.Instance.camerasOpen)
        {
            CameraSystem.Instance.camerasOpen = false;
            CameraSystem.Instance.ShowCamera();
        }
        if (PlaceholderWinningSystem.Instance.minigameActive)
        {
            PlaceholderWinningSystem.Instance.ShowUI();
        }
        StartCoroutine(Shaking());
        jumpscareAnimation.SetActive(true);
        SoundManager.Instance.PlaySound("Jumpscare");
        mainCamera.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    IEnumerator Shaking()
    {
        float elapsed = 0.0f;
        float duration = 1.0f;
        Vector3 originalPos = mainCamera.transform.position;

        while (elapsed < duration)
        {

            mainCamera.transform.position = originalPos + UnityEngine.Random.insideUnitSphere * 0.05f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        SoundManager.Instance.StopSound("Jumpscare");
        GameManager.Instance.GameOver();
    }
}
