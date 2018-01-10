using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorMain : MonoBehaviour, IDifficulty {

    private const int roomSizeX = 24;
    private const int roomSizeY = 10;
    private Vector2 rSize;

    private const int numRooms = 4;

    public Transform room;

    public int[][] mapdata;

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

    public TextAsset mapDataText;
    public string[] testData;
    public string[][] testData2;

    public int mapTargetDifficulty;
    LevelSelector levelSelector = new LevelSelector();

    private void Start()
    {
        mapdata = LoadMaps(mapDataText);
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
            newRoom.GetComponent<RoomGenerator>().GenerateRoom(rSize, i, mapHolder, mapdata[i]);
        }
             
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

}
