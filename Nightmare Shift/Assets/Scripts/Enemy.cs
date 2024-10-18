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

    // Start is called before the first frame update
    void Start()
    {
        currentRoom = GetComponentInParent<Room>();
    }

    // Update is called once per frames
    void Update()
    {
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
        }
        else
        {
            Debug.LogError("currentRoom is null. Cannot move enemy.");
        }
    }
}
