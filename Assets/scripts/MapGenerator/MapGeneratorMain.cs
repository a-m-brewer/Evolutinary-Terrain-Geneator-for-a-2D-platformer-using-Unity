using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Responsible for creating a set of rooms in the world 
 * This is done through using the LevelSelector Class
 * and Roomgenerator script
 */ 
public class MapGeneratorMain : MonoBehaviour, IDifficulty {
    // the size of each room
    private const int roomSizeX = 24;
    private const int roomSizeY = 10;
    public Vector2 rSize;
    // number of rooms
    public int numRooms = 4;
    // links to the room and flag prefabs
    public Transform room;
    public Transform endFlag;
    // as storing of the data needed to create maps
    public int[][] allTemplateRoomData;
    public int[][] chosenMap;
    // implimentation of IDifficulty
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
    // create a global object of the levelSelector script
    LevelSelector levelSelector = new LevelSelector();
    // a link to the file storing all of the roomData
    public TextAsset mapDataText;

    // Difficultys
    public int mapTargetDifficulty;
    public int actualDifficultyScore;

    private void Awake()
    {
        // set the size of the room
        rSize = new Vector2(TileInformation.roomSizeX, TileInformation.roomSizeY);
    }

    private void Start()
    {
        // load in all of the data of the rooms
        allTemplateRoomData = LoadMaps(mapDataText);
        // create a map
        GenerateMap(rSize);
        // caculate the difficutly of the map that is created
        CalculateDifficulty();
    }

    // main method for creating a map that calls other methods
    public void GenerateMap(Vector2 rSize)
    {
        // chooses the set of rooms that are closest in difficulty to the desired difficulty of the map

        // Prototype version
        chosenMap = levelSelector.SelectMap(allTemplateRoomData, TileInformation.numRooms, mapTargetDifficulty, room);

        //InitMap initMap = new InitMap();

        //chosenMap = initMap.Generate();

        actualDifficultyScore = levelSelector.DifficultyScoreOfMap(chosenMap, room.GetComponent<RoomGenerator>().GetRoomTiles());

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

        // create each of the rooms next to each other
        for (int i = 0; i < TileInformation.numRooms; i++)
        {
            Vector3 rPos = new Vector3(i, 0, 0);
            Transform newRoom;
            newRoom = Instantiate(room, rPos,Quaternion.Euler(Vector3.right));
            newRoom.parent = mapHolder;
            newRoom.GetComponent<RoomGenerator>().GenerateRoom(rSize, i, mapHolder, chosenMap[i]);
        }
        // place the end flag at the end of the level and place borders so the player does not escape
        // the map.
        PlaceEndFlag(endFlag, mapHolder);
        PlaceBorders();
    }

    // add the difficulty of the each rooms to get the maps score
    void CalculateDifficulty()
    {
        AddComponentWithTagToDifficulty("Room");
    }


    void AddToDifficulty(Transform t)
    {
        if (t.gameObject.GetComponent<IDifficulty>() != null)
        {
            DifficultyScore += t.GetComponent<IDifficulty>().DifficultyScore;
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

    // work through the text file and add the rooms to a int array
    // ',' represents the seperation of room tiles
    // '.' represents the seperation of rooms
    // represents rooms in array array[roomNumber][tileNumber]
    public int[][] LoadMaps(TextAsset inFile)
    {
        string[][] levels = new string[1][];
        int[][] levelsInt = new int[1][];

        if(inFile != null)
        {
            string[] wholeLevels = (inFile.text.Split('.'));
            levels = new string[wholeLevels.Length][];
            levelsInt = new int[wholeLevels.Length][];

            for (int i = 0; i < wholeLevels.Length; i++)
            {
                levels[i] =  wholeLevels[i].Split(',');
                levelsInt[i] = new int[levels[i].Length];
                for (int j = 0; j < levels[i].Length; j++)
                {
                    int.TryParse(levels[i][j], out levelsInt[i][j]);
                }
            }

        }
        return levelsInt;
    }

    // places the flag at the end of a level
    void PlaceEndFlag(Transform flag, Transform holder)
    {
        float x = (float)(roomSizeX * (TileInformation.numRooms)) - 2f;
        float y = 1.5f;
        Vector3 place = new Vector3(x, y, 0);
        Transform flagPlace = Instantiate(flag, place, Quaternion.Euler(Vector3.right));
        flagPlace.parent = holder;
    }
    // creates a wall at a point
    void CreateAndPlaceBorder(string name, Vector2 pos, Vector2 size, Transform parent)
    {
        GameObject newGO = new GameObject();
        newGO.AddComponent<BoxCollider2D>();
        newGO.name = name;
        newGO.transform.position = new Vector3(pos.x, pos.y, 0);
        newGO.transform.localScale = new Vector3(size.x, size.y, 1f);
        newGO.transform.parent = parent;
    }
    // places walls around the map so that the player does not fall out of the map
    void PlaceBorders()
    {

        // set the name of the game object to group map tiles with
        string holderName = "Borders";
        // Each time the map generates destroy the old game objects
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        // create the new holder
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        CreateAndPlaceBorder("leftWall", new Vector2(-1f, 5f), new Vector2(1f, roomSizeY), mapHolder);
        //CreateAndPlaceBorder("topWall", new Vector2((roomSizeX * numRooms) / 2, roomSizeY), new Vector2((roomSizeX * numRooms), 1f), mapHolder);
        CreateAndPlaceBorder("rightWall", new Vector2((roomSizeX * TileInformation.numRooms), 5f), new Vector2(1f, roomSizeY), mapHolder);
    }

    public int GetNumRooms()
    {
        return TileInformation.numRooms;
    }

    public float GetMapSize()
    {
        return (rSize.x * GetNumRooms()) - 0.5f;
    }
}
