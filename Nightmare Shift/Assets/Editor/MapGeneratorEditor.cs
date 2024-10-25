using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Add a space before the button
        GUILayout.Space(10);

        // Add the Generate button
        if (GUILayout.Button("Generate Map"))
        {
            // Reference the target object and call the RecreateBoard function
            MapGenerator mapGenerator = (MapGenerator)target;
            mapGenerator.RecreateBoard();
        }
    }
}
