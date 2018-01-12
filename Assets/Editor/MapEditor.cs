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
