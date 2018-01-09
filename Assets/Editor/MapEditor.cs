using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGeneratorMain))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGeneratorMain map = target as MapGeneratorMain;
        Vector2 roomSize = new Vector2(24,10);
        map.GenerateMap(roomSize);
    }
}
