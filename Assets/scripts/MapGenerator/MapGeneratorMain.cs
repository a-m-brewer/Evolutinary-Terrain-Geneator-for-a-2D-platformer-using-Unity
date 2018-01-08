using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorMain : MonoBehaviour, IDifficulty {

    private const int roomSizeX = 24;
    private const int roomSizeY = 10;
    private const int numRooms = 4;

    public Transform[] mapTiles = new Transform[4];
    GameObject[] enemies;

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
            0,0,0,0,1,2,3,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        };
    }


    private void Start()
    {
        GenerateMap();
        AddEnemiesToDifficulty();
    }

    private int ColRowToArrayIndex(int x, int y)
    {
        return x + roomSizeX * y;
    }

    public void GenerateMap()
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
            GenerateRoom(i, mapHolder, mapdata);
        }
        
    }

    private void GenerateRoom(int room, Transform i_mapRoomHolder, int[] roomData) 
    {
        int rOffset = room * roomSizeX;
        for (int x = 0; x < roomSizeX + 0; x++)
        {
            for (int y = 0; y < roomSizeY; y++)
            {
                // place the tile
                Vector3 tilePos = new Vector3((-roomSizeX / 2 + 0.5f + x) + rOffset , -roomSizeY / 2 + 0.5f + y, 0);
                //Vector3 tilePos = new Vector3(x + rOffset, y, 0);
                Transform newTile;
                int mappos = ColRowToArrayIndex(x, y);

                int toInstantiate = 0;

                switch (roomData[mappos])
                {
                    case 0:
                        toInstantiate = 0;
                        break;
                    case 1:
                        toInstantiate = 1;
                        break;
                    case 2:
                        toInstantiate = 2;
                        break;
                    case 3:
                        toInstantiate = 3;
                        break;
                }

                newTile = Instantiate(mapTiles[toInstantiate], tilePos, Quaternion.Euler(Vector3.right)) as Transform;
                // add the gap by making the tile smaller
                newTile.localScale = Vector3.one;
                // add to the list of map tiles
                newTile.parent = i_mapRoomHolder;

                AddToDifficulty(newTile);
            }
        }
    }

    void AddToDifficulty(Transform t)
    {
        if (t.gameObject.GetComponent<IDifficulty>() != null)
        {
            DifficultyScore += t.GetComponent<IDifficulty>().DifficultyScore;
            Debug.Log(DifficultyScore + " " + t.gameObject.name);                       
        }
    }

    void AddEnemiesToDifficulty()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies)
        {
            AddToDifficulty(e.transform);
        }
    }

}
