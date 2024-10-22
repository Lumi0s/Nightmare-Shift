using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EnemyPosition
{
    public GameObject enemy;

    public Transform transform;

}

public class Room : MonoBehaviour
{
    public List<GameObject> enemies = new();
    public List<GameObject> connectedRooms = new();

    void Start()
    {

    }

    public void moveEnemy(GameObject enemy, GameObject room = null)
    {
        if (enemy == null || !enemies.Contains(enemy)) return;

        var availableRooms = connectedRooms.FindAll(r =>
        {
            foreach (Transform child in r.transform)
            {
                if (child.CompareTag(enemy.tag))
                {
                    return true;
                }
            }
            return false;
        });
        if (availableRooms.Count == 0) return;


        GameObject targetRoom = room ?? availableRooms[Random.Range(0, availableRooms.Count - 1)];

        Debug.Log(targetRoom.name);

        Room targetRoomComponent = targetRoom.GetComponent<Room>();

        enemies.Remove(enemy);
        targetRoomComponent.enemies.Add(enemy);
        enemy.GetComponent<Enemy>().currentRoom = targetRoomComponent;

        Transform targetChild = targetRoom.transform.Find(enemy.tag);
        Debug.Log(targetChild.name);
        targetChild.gameObject.SetActive(true);
        enemy.transform.SetParent(targetRoom.transform);
        this.transform.Find(enemy.tag).gameObject.SetActive(false);



    }
}

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    SerializedProperty enemiesProperty;
    SerializedProperty connectedRoomsProperty;


    void OnEnable()
    {
        enemiesProperty = serializedObject.FindProperty("enemies");
        connectedRoomsProperty = serializedObject.FindProperty("connectedRooms");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(enemiesProperty, true);
        EditorGUILayout.PropertyField(connectedRoomsProperty, true);
        serializedObject.ApplyModifiedProperties();
    }
}