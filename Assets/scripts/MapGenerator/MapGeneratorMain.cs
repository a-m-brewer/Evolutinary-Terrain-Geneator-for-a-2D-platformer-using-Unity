using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorMain : MonoBehaviour, IDifficulty {

    private const int roomSizeX = 24;
    private const int roomSizeY = 10;
    private Vector2 rSize;

    private const int numRooms = 4;

    public Transform room;

    private int[] mapdata;

    private int difficutly;
    public int DifficultyScore
    {
        get
        {
            return difficutly;
        }

        set
        {
            difficutly = value;
        }
    }

    public MapGeneratorMain()
    {
        mapdata = new int[roomSizeX * roomSizeY] {
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,
            0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        };
    }


    private void Start()
    {
        rSize = new Vector2(roomSizeX, roomSizeY);
        GenerateMap(rSize);
        CalculateDifficulty();
    }

    public void GenerateMap(Vector2 rSize)
    {
        // set the name of the game object to group map tiles with
        string holderName = "MapGen";
        // Each time the map generates destroy the old game objects
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        // create the new holder
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        
        for (int i = 0; i < numRooms; i++)
        {
            Vector3 rPos = new Vector3(i, 0, 0);
            Transform newRoom;
            newRoom = Instantiate(room, rPos,Quaternion.Euler(Vector3.right));
            newRoom.parent = mapHolder;
            newRoom.GetComponent<RoomGenerator>().GenerateRoom(rSize, i, mapHolder, mapdata);
            Debug.Log(newRoom.GetComponent<RoomGenerator>().DifficultyScore);
        }
             
    }

    void CalculateDifficulty()
    {
        AddComponentWithTagToDifficulty("Room");
        AddComponentWithTagToDifficulty("Enemy");
    }

    void AddToDifficulty(Transform t)
    {
        if (t.gameObject.GetComponent<IDifficulty>() != null)
        {
            DifficultyScore += t.GetComponent<IDifficulty>().DifficultyScore;
            Debug.Log(DifficultyScore + " " + t.gameObject.name);                       
        }
    }

    void AddComponentWithTagToDifficulty(string componentTag)
    {
        GameObject[] components = GameObject.FindGameObjectsWithTag(componentTag);
        foreach(GameObject c in components)
        {
            AddToDifficulty(c.transform);
        }
    }

}
