using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeTesting : MonoBehaviour {

    public TextAsset huristicMaps;
	// Use this for initialization
	void Start () {
        InitHuristicRooms ihr = new InitHuristicRooms(huristicMaps);
        Room[] hRooms = ihr.Rooms;

        foreach(Room r in hRooms)
        {
            r.Fitness = Random.Range(0, 99);
            Debug.Log(r.Fitness);
        }

        Debug.Log("Random Ends");

        List<Room> hRoomList = hRooms.ToList();

        MergeSortRoom msr = new MergeSortRoom();
        List<Room> merge = msr.MergeSort(hRoomList);

        foreach(Room r in merge)
        {
            Debug.Log(r.Fitness);
        }
    }
	
}
