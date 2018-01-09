using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour, IDifficulty
{
    public Transform[] mapTiles = new Transform[4];

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

    public void GenerateRoom(Vector2 size, int rNumber, Transform parent, int[] roomData)
    {

        // set the name of the game object to group map tiles with
        string holderName = "RoomGen";
        // Each time the map generates destroy the old game objects
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        // create the new holder
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        float rOffset = CalculateRoomOffset(rNumber, (int) size.x);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 tilePos = new Vector3(x + rOffset, y, 0);

                int mappos = ColRowToArrayIndex(x, y, size.x);

                int toInstantiate = TileTypeToSpawn(roomData[mappos]);

                Transform newTile = SpawnNewTileInRoom(mapTiles[toInstantiate], tilePos, mapHolder);

                AddToDifficulty(newTile);
            }
        }
    }

    private Transform SpawnNewTileInRoom(Transform maptile, Vector3 tilePosition, Transform parentTransform)
    {
        Transform newTile;
        newTile = Instantiate(maptile, tilePosition, Quaternion.Euler(Vector3.right));
        newTile.parent = parentTransform;
        return newTile;
    }

    private float CalculateRoomOffset(int room, int roomSize)
    {
        return (room * roomSize);
    }

    void AddToDifficulty(Transform t)
    {
        if (t.gameObject.GetComponent<IDifficulty>() != null)
        {
            DifficultyScore += t.GetComponent<IDifficulty>().DifficultyScore;
            Debug.Log(DifficultyScore + " " + t.gameObject.name);
        }
    }

    private int ColRowToArrayIndex(int x, int y, float roomSizeX)
    {
        return x + (int) roomSizeX * y;
    }

    private int TileTypeToSpawn(int mapDataIndex)
    {
        int toInstantiate = 0;
        switch (mapDataIndex)
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
        return toInstantiate;
    }

}

