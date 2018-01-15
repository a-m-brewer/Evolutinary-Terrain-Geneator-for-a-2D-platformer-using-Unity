using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is resposible for spawning a new room in the world 
 * at a position specified and then calculating the difficulty of 
 * that room.
 */ 

public class RoomGenerator : MonoBehaviour, IDifficulty
{
    // list of the tiles that will be spawned into the room 
    public Transform[] roomTiles = new Transform[7];
    // implementation of IDifficulty interface
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

    // main method called to create a room 
    public void GenerateRoom(Vector2 size, int rNumber, Transform parent, int[] roomData)
    {
        // set the name of the game object to group map tiles with
        string holderName = "RoomGen";
        // Each time the room generates destroy the old game objects
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        // create the new holder
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        // each room needs to be offset so that it does not spawn the new room in the same
        // position as the old one
        float rOffset = CalculateRoomOffset(rNumber, (int) size.x);
        // work through the room and create all of the tiles for that room
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                // where the tile will be placed in the map
                Vector3 tilePos = new Vector3(x + rOffset, y, 0);
                // work out the array index of a tile in a certain column and row
                int mappos = ColRowToArrayIndex(x, y, size.x);
                // roomData holds what kind of tile should be spawned for an array index
                // So check what this array says the current index should be
                int toInstantiate = roomData[mappos];
                // check if the tile is an enemy that needs a background tile placed behind it
                if (NeedsBackgroundTile(toInstantiate))
                {
                    InstatiateBackground(tilePos, mapHolder);
                }
                // place the tile into the room
                Transform newTile = SpawnNewTileInRoom(roomTiles[toInstantiate], tilePos, mapHolder);
                // add to the overall difficulty of the room
                AddToDifficulty(newTile);
            }
        }

    }

    // places a new tile in the room at a certain place as well as a given type of tile worked out in generateroom
    private Transform SpawnNewTileInRoom(Transform maptile, Vector3 tilePosition, Transform parentTransform)
    {
        Transform newTile;
        newTile = Instantiate(maptile, tilePosition, Quaternion.Euler(Vector3.right));
        newTile.parent = parentTransform;
        return newTile;
    }

    // off set is the width of a room plus the room number 
    // e.g. offset of second room (1 * 24) = 24
    // so new tiles will be spawned from x:24 onwards
    private float CalculateRoomOffset(int room, int roomSize)
    {
        return (room * roomSize);
    }

    // if the tile has a component that implements IDifficulty add it's score to the room score
    void AddToDifficulty(Transform t)
    {
        if (t.gameObject.GetComponent<IDifficulty>() != null)
        {
            DifficultyScore += t.GetComponent<IDifficulty>().DifficultyScore;
        }
    }

    // work out given an x,y coordanate what the posistion in the data array the coordanate is
    private int ColRowToArrayIndex(int x, int y, float roomSizeX)
    {
        return x + (int) roomSizeX * y;
    }

    // if the tile is an enemy it needs a background behind it after it has spawned
    private bool NeedsBackgroundTile(int mapTileID)
    {
        if (mapTileID == 4 || mapTileID == 5)
        {
            return true;
        }
        return false;
    }

    // create the background at a given point in the map
    private Transform InstatiateBackground(Vector3 tilePosition, Transform parentTransform)
    {
        return SpawnNewTileInRoom(roomTiles[0], tilePosition, parentTransform);
    }

    // get a list of the prefabs of tiles
    public Transform[] GetRoomTiles()
    {
        return roomTiles;
    }
}

