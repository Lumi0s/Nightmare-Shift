using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EnemyPosition
{
    public GameObject enemy;
    public Vector3 position;
}

public class Room : MonoBehaviour
{
    public List<GameObject> enemies = new();
    public List<GameObject> connectedRooms = new();
    public List<EnemyPosition> predefinedPositions = new();

    private Dictionary<GameObject, Vector3> enemyPositions = new();

    void Start()
    {
        foreach (EnemyPosition enemyPosition in predefinedPositions)
        {
            if (enemyPosition.enemy != null)
            {
                enemyPositions[enemyPosition.enemy] = enemyPosition.position;
            }
        }

        // Clean up the enemies list to remove any null entries
        enemies.RemoveAll(e => e == null);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.currentRoom = this;
                    if (!enemyPositions.ContainsKey(enemy))
                    {
                        enemyPositions[enemy] = enemy.transform.localPosition;
                    }
                }
            }
        }
    }

    public void moveEnemy(GameObject enemy, GameObject room = null)
    {
        if (enemy == null || !enemies.Contains(enemy)) return;

        GameObject targetRoom = room ?? connectedRooms[Random.Range(0, connectedRooms.Count)];
        Room targetRoomComponent = targetRoom.GetComponent<Room>();

        enemies.Remove(enemy);
        targetRoomComponent.enemies.Add(enemy);
        enemy.GetComponent<Enemy>().currentRoom = targetRoomComponent;
        enemy.transform.SetParent(targetRoom.transform);

        if (targetRoomComponent.enemyPositions.TryGetValue(enemy, out Vector3 targetPosition))
        {
            enemy.transform.localPosition = targetPosition;
        }
        else
        {
            targetRoomComponent.enemyPositions[enemy] = enemy.transform.localPosition;
        }

        // Clean up the enemies list to remove any null entries
        enemies.RemoveAll(e => e == null);
    }
}

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    SerializedProperty enemiesProperty;
    SerializedProperty connectedRoomsProperty;
    SerializedProperty predefinedPositionsProperty;

    void OnEnable()
    {
        enemiesProperty = serializedObject.FindProperty("enemies");
        connectedRoomsProperty = serializedObject.FindProperty("connectedRooms");
        predefinedPositionsProperty = serializedObject.FindProperty("predefinedPositions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(enemiesProperty, true);
        EditorGUILayout.PropertyField(connectedRoomsProperty, true);
        EditorGUILayout.PropertyField(predefinedPositionsProperty, new GUIContent("Predefined Positions"), true);

        serializedObject.ApplyModifiedProperties();
    }
}