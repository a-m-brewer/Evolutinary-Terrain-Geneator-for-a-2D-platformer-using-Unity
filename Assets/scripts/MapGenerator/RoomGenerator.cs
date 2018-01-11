using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour, IDifficulty
{
    public Transform[] roomTiles = new Transform[6];

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

                int toInstantiate = roomData[mappos];

                if (NeedsBackgroundTile(toInstantiate))
                {
                    InstatiateBackground(tilePos, mapHolder);
                }

                Transform newTile = SpawnNewTileInRoom(roomTiles[toInstantiate], tilePos, mapHolder);

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
        }
    }

    private int ColRowToArrayIndex(int x, int y, float roomSizeX)
    {
        return x + (int) roomSizeX * y;
    }

    private bool NeedsBackgroundTile(int mapTileID)
    {
        if (mapTileID == 4 || mapTileID == 5)
        {
            return true;
        }
        return false;
    }

    private Transform InstatiateBackground(Vector3 tilePosition, Transform parentTransform)
    {
        return SpawnNewTileInRoom(roomTiles[0], tilePosition, parentTransform);
    }


    public Transform[] GetRoomTiles()
    {
        return roomTiles;
    }
}

