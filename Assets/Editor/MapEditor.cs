using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGeneratorMain))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGeneratorMain myScript = (MapGeneratorMain)target;

        Vector2 rSize = myScript.rSize;

        if (GUILayout.Button("Generate Level"))
        {
            myScript.GenerateMap(rSize);
        }
    }
}

[CustomEditor (typeof(MapGenDisplay))]
public class EvoMapEditor : Editor
{
    SerializedProperty n;

    private void OnEnable()
    {
        n = serializedObject.FindProperty("n");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();
        DrawDefaultInspector();

        MapGenDisplay mgd = (MapGenDisplay)target;

        if (GUILayout.Button("Increment Evolution"))
        {
            mgd.IncrementEvolutionOfRoomAndDisplayBest();
            mgd.DisplayRoom();
        }

        EditorGUILayout.IntSlider(n, 0, 99);
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("View Map N"))
        {
            mgd.SwitchToRoomNinPopulation(mgd.n);
            mgd.DisplayRoom();
        }
    }
}
