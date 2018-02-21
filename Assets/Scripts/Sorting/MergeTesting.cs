using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTesting : MonoBehaviour {

    public TextAsset huristicMaps;
	// Use this for initialization
	void Start () {
        InitHuristicRooms ihr = new InitHuristicRooms(huristicMaps, new EvaluateRoom(0.75f));
        Room[] hRooms = ihr.Rooms;
        
    }
	
}
