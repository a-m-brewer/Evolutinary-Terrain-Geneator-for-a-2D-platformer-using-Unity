using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

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
    bool canPress = true;

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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            mgd.IncrementEvolutionOfRoomAndDisplayBest();
            mgd.DisplayRoom();
            sw.Stop();
            mgd.EditorDebugLog(sw.ElapsedMilliseconds);
        }

        if (GUILayout.Button("Increment Evolution by 10"))
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                mgd.IncrementEvolutionOfRoomAndDisplayBest();
            }
            mgd.DisplayRoom();
            sw.Stop();
            mgd.EditorDebugLog(sw.ElapsedMilliseconds);
        }

        if (GUILayout.Button("Evolve Until Max") && canPress)
        {
            canPress = false;
            mgd.InvokeRepeatingEvolution();
        }

        if (GUILayout.Button("Cancel Invoke"))
        {
            mgd.CancelInvokeEvolution();
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
