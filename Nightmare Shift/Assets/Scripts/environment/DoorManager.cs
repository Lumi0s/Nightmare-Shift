using System.Collections;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject door;
    public GameObject Button;
    public bool isOpen = false;
    public float slideDuration = 1f;
    private BoxCollider buttonCollider;
    private float slideDistance; 
    private bool canChangeState = true;

    void Start()
    {
        slideDistance = door.transform.localScale.y;
        buttonCollider = Button.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == buttonCollider)
                {
                    if (canChangeState)
                    {
                        if (isOpen)
                        {
                            CloseDoor();
                        }
                        else
                        {
                            OpenDoor();
                        }
                    }
                }
            }
        }
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
}