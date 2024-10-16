using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveInterval = 5.0f;
    private float timeSinceLastMove = 0.0f;
    public Room currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize currentRoom if needed
        currentRoom = GetComponentInParent<Room>();
    }

    // Update is called once per frames
    void Update()
    {
        timeSinceLastMove += Time.deltaTime;

        if (timeSinceLastMove >= moveInterval)
        {
            move();
            timeSinceLastMove = 0.0f;
        }
    }

    public void move()
    {

        if (currentRoom != null)
        {
            currentRoom.moveEnemy(this.gameObject);
            Debug.Log("Enemy moved to another room");
        }
        else
        {
            Debug.LogError("currentRoom is null. Cannot move enemy.");
        }
    }
}
