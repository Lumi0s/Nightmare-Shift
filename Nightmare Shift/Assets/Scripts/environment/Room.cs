using System.Collections;
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

    // public GameObject camera;
    // private CCTVNoiseEffect cctvNoiseEffect;

    void Start()
    {
        // if (camera != null)
        //     cctvNoiseEffect = camera.GetComponent<CCTVNoiseEffect>();
    }

    // private IEnumerator SetNoiseForDuration(float duration)
    // {
    //     // if (GetComponent<Camera>() != null && cctvNoiseEffect != null)
    //     // {
    //     //     float baseNoise = cctvNoiseEffect.noiseAmount;
    //     //     cctvNoiseEffect.noiseAmount = 1.0f;
    //     //     yield return new WaitForSeconds(duration);
    //     //     cctvNoiseEffect.noiseAmount = baseNoise; // Reset to default or desired value
    //     // }
    // }

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

        GameObject targetRoom = room ?? availableRooms[Random.Range(0, availableRooms.Count)];
        Room targetRoomComponent = targetRoom.GetComponent<Room>();

        enemies.Remove(enemy);
        targetRoomComponent.enemies.Add(enemy);

        enemy.GetComponent<Enemy>().currentRoom = targetRoomComponent;

        // Find the child of targetRoom with the same tag as the enemy
        Transform targetChild = null;
        foreach (Transform child in targetRoom.transform)
        {
            if (child.CompareTag(enemy.tag))
            {
                targetChild = child;
                break;
            }
        }
        // Enable all SkinnedMeshRenderers in the target child
        SkinnedMeshRenderer[] targetMeshRenderers = targetChild.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in targetMeshRenderers)
        {
            renderer.enabled = true;
        }

        enemy.transform.SetParent(targetRoom.transform);

        // Disable all SkinnedMeshRenderers in the previous room's enemy
        Transform enemyFromThisRoom = null;
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag(enemy.tag))
            {
                enemyFromThisRoom = child;
                break;
            }
        }

        if (enemyFromThisRoom != null)
        {
            SkinnedMeshRenderer[] previousMeshRenderers = enemyFromThisRoom.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in previousMeshRenderers)
            {
                renderer.enabled = false;
            }
        }

        // Start the coroutine to set noise amount to 1 for 2 seconds
        // StartCoroutine(SetNoiseForDuration(2.0f));
    }
}

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    SerializedProperty enemiesProperty;
    SerializedProperty connectedRoomsProperty;

    // SerializedProperty cameraProperty;


    void OnEnable()
    {
        enemiesProperty = serializedObject.FindProperty("enemies");
        connectedRoomsProperty = serializedObject.FindProperty("connectedRooms");
        // cameraProperty = serializedObject.FindProperty("camera");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(enemiesProperty, true);
        EditorGUILayout.PropertyField(connectedRoomsProperty, true);
        // EditorGUILayout.PropertyField(cameraProperty, true);
        serializedObject.ApplyModifiedProperties();

    }
}