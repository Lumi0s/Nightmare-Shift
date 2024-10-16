using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> enemies = new();
    public List<GameObject> connectedRooms = new();


    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().currentRoom = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void moveEnemy(GameObject enemy, GameObject room = null)
    {
        //if (!enemies.Contains(enemy)) return;

        GameObject targetRoom = room ?? connectedRooms[Random.Range(0, connectedRooms.Count)];
        targetRoom.GetComponent<Room>().enemies.Add(enemy);
        enemies.Remove(enemy);
        enemy.GetComponent<Enemy>().currentRoom = targetRoom.GetComponent<Room>();
        enemy.transform.SetParent(targetRoom.transform);
        Vector3 localPosition = transform.InverseTransformPoint(enemy.transform.position);

        // Set the enemy's position to the same local position in the target room
        enemy.transform.position = targetRoom.transform.TransformPoint(localPosition);
    }
}