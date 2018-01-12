using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorMain : MonoBehaviour, IDifficulty {

    private const int roomSizeX = 24;
    private const int roomSizeY = 10;
    public Vector2 rSize;

    public const int numRooms = 4;
    public Transform room;
    public Transform endFlag;

    public int[][] allTemplateRoomData;
    public int[][] chosenMap;

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
    LevelSelector levelSelector = new LevelSelector();
    public TextAsset mapDataText;

    public int mapTargetDifficulty;
    public int actualDifficultyScore;

    private void Start()
    {
        allTemplateRoomData = LoadMaps(mapDataText);

        rSize = new Vector2(roomSizeX, roomSizeY);

        GenerateMap(rSize);
        CalculateDifficulty();
    }

    public void GenerateMap(Vector2 rSize)
    {
        chosenMap = levelSelector.SelectMap(allTemplateRoomData, numRooms, mapTargetDifficulty, room);
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
        
        for (int i = 0; i < numRooms; i++)
        {
            Vector3 rPos = new Vector3(i, 0, 0);
            Transform newRoom;
            newRoom = Instantiate(room, rPos,Quaternion.Euler(Vector3.right));
            newRoom.parent = mapHolder;
            newRoom.GetComponent<RoomGenerator>().GenerateRoom(rSize, i, mapHolder, chosenMap[i]);
        }

        PlaceEndFlag(endFlag, mapHolder);
        PlaceBorders();
    }

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

    void PlaceEndFlag(Transform flag, Transform holder)
    {
        float x = (float)(roomSizeX * (numRooms)) - 2f;
        float y = 1.5f;
        Vector3 place = new Vector3(x, y, 0);
        Transform flagPlace = Instantiate(flag, place, Quaternion.Euler(Vector3.right));
        flagPlace.parent = holder;
    }

    void CreateAndPlaceBorder(string name, Vector2 pos, Vector2 size, Transform parent)
    {
        GameObject newGO = new GameObject();
        newGO.AddComponent<BoxCollider2D>();
        newGO.name = name;
        newGO.transform.position = new Vector3(pos.x, pos.y, 0);
        newGO.transform.localScale = new Vector3(size.x, size.y, 1f);
        newGO.transform.parent = parent;
    }

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
        CreateAndPlaceBorder("topWall", new Vector2((roomSizeX * numRooms) / 2, roomSizeY), new Vector2((roomSizeX * numRooms), 1f), mapHolder);
        CreateAndPlaceBorder("rightWall", new Vector2((roomSizeX * numRooms), 5f), new Vector2(1f, roomSizeY), mapHolder);
    }
}
