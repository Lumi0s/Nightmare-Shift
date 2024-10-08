using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private GameObject door;
    public bool isOpen = false;
    public float slideDistance; // Distance to slide the door
    public float slideDuration = 1f; // Duration of the slide

    public bool canChangeState = true;


    void Start()
    {
        door = transform.Find("the door").gameObject;
        slideDistance = door.transform.localScale.y;
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            canChangeState = false;
            StartCoroutine(SlideDoor(Vector3.up * slideDistance));
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            canChangeState = false;
            StartCoroutine(SlideDoor(Vector3.down * slideDistance));
            isOpen = false;
        }
    }

    private IEnumerator SlideDoor(Vector3 direction)
    {
        Vector3 startPosition = door.transform.position;
        Vector3 endPosition = startPosition + direction;
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            door.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = endPosition;
        canChangeState = true;
    }

    void Update()
    {
        // You can add input handling here to open/close the door
    }
}
