using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorMain : MonoBehaviour {

    private const int mapSizeX = 10;
    private const int mapSizeY = 10;

    public Transform background;
    public Transform ground;

    private int[] mapdata;

    [Range(0,1)]
    public float outlinePercent;

    public MapGeneratorMain()
    {

        mapdata = new int[mapSizeX * mapSizeY] {
            1,1,1,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,1,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
        };
    }


    private void Start()
    {
        GenerateMap();
    }

    private int ColRowToArrayIndex(int x, int y)
    {
        return x + mapSizeX * y;
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

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                // place the tile
                Vector3 tilePos = new Vector3(-mapSizeX/2 + 0.5f + x, -mapSizeY/2 + 0.5f + y, 0);
                Transform newTile;
                int mappos = ColRowToArrayIndex(x,y);
                Debug.Log("x: " + x + " y: " + y + " id: " + mapdata[mappos]);
                if (mapdata[mappos] == 0) 
                {
                    newTile = Instantiate(background, tilePos, Quaternion.Euler(Vector3.right)) as Transform;
                    // add the gap by making the tile smaller
                    newTile.localScale = Vector3.one * (1 - outlinePercent);
                    // add to the list of map tiles
                    newTile.parent = mapHolder;

                } else if (mapdata[mappos] == 1)
                {
                    newTile = Instantiate(ground, tilePos, Quaternion.Euler(Vector3.right)) as Transform;
                    // add the gap by making the tile smaller
                    newTile.localScale = Vector3.one * (1 - outlinePercent);
                    // add to the list of map tiles
                    newTile.parent = mapHolder;

                }
            }
        }
    }
}
