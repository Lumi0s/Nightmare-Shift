using System.Collections;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject door;
    public GameObject Button;
    [SerializeField] private GameObject lightButton;
    [SerializeField] private GameObject lightObject;
    [SerializeField] private GameObject mainRoom;
    [SerializeField] private GameObject sideRoom;
    [SerializeField] private GameObject enemyToTriggerSound; 
 
    public bool isOpen = false;
    public float slideDuration = 1f;
    private BoxCollider buttonCollider;
    private BoxCollider lightButtonCollider;
    private Room mainRoomManager;
    private Room sideRoomManager;
    private float slideDistance; 
    private bool canChangeState = true;


    void Start()
    {
        slideDistance = transform.localScale.y * 2f;
        buttonCollider = Button.GetComponent<BoxCollider>();
        lightButtonCollider = lightButton.GetComponent<BoxCollider>();
        mainRoomManager = mainRoom.GetComponent<Room>();
        sideRoomManager = sideRoom.GetComponent<Room>();
        sideRoomManager.connectedRooms.Add(mainRoom);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main == null) {return;}
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
                else if (hit.collider == lightButtonCollider)
                {
                    if (lightObject.activeSelf)
                    {
                        lightObject.SetActive(!lightObject.activeSelf);
                        PowerSystem.Instance.usage--;
                        SoundManager.Instance.StopSound("Light");
                    }
                    else
                    {
                        lightObject.SetActive(!lightObject.activeSelf);
                        PowerSystem.Instance.usage++;
                        SoundManager.Instance.PlaySound("Light");
                        if (sideRoomManager.enemies.Exists(enemy => enemy == enemyToTriggerSound))
                        {
                            SoundManager.Instance.PlaySound("DoorScare");
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
            sideRoomManager.connectedRooms.Add(mainRoom);
            PowerSystem.Instance.usage--;
            SoundManager.Instance.PlaySound("Door");
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            canChangeState = false;
            StartCoroutine(SlideDoor(Vector3.down * slideDistance));
            isOpen = false;
            sideRoomManager.connectedRooms.Remove(mainRoom);
            PowerSystem.Instance.usage++;
            SoundManager.Instance.PlaySound("Door");
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