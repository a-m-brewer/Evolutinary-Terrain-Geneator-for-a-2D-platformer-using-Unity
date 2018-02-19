using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenDisplay : MonoBehaviour, IDifficulty
{
    // links to the room and flag prefabs
    public Transform room;
    // as storing of the data needed to create maps
    public int[][] allTemplateRoomData;
    public int[] roomToDisplay;
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
    // a link to the file storing all of the roomData
    public TextAsset huristicMaps;
    private Room chosenRoom;
    private EvaluateRoom evaluateRoom = new EvaluateRoom(0.75f);
    // Difficultys
    public int mapTargetDifficulty;
    public int actualDifficultyScore;

    Room[] roomPop;
    SelectRoom selectRoom = new SelectRoom();
    Crossover crossover = new Crossover();
    Mutation mutation = new Mutation();

    private void Start()
    {
        InitMap();
        // create a map
        DisplayRoom();
        // caculate the difficutly of the map that is created
        CalculateDifficulty();
    }

    // main method for creating a map that calls other methods
    public void DisplayRoom()
    {
        Debug.Log(chosenRoom.Fitness);
        // chooses the set of rooms that are closest in difficulty to the desired difficulty of the map
        actualDifficultyScore = CalculateRoomDifficulty(chosenRoom.Data, room.GetComponent<RoomGenerator>().GetRoomTiles());
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
        Vector3 rPos = new Vector3(0, 0, 0);
        Transform newRoom;
        newRoom = Instantiate(room, rPos, Quaternion.Euler(Vector3.right));
        newRoom.parent = mapHolder;
        newRoom.GetComponent<RoomGenerator>().EvoGenerateRoom(0, mapHolder, chosenRoom.Data);

    }

    public void InitMap()
    {
        //InitPopulation ip = new InitPopulation();
        //roomPop = ip.Generate(huristicMaps, evaluateRoom.GetGroundPercent(), evaluateRoom);
        //chosenRoom = ip.bestRoom;

        InitRandomPopulation irp = new InitRandomPopulation(0.75f, evaluateRoom);
        roomPop = irp.Generate(evaluateRoom.GetGroundPercent(), evaluateRoom);
        chosenRoom = irp.bestRoom;

    }

    public void IncrementEvolutionOfRoomAndDisplayBest()
    {
        int numRoomsInGeneration = 80;
        Room[] np = new Room[numRoomsInGeneration];
        for (int p = 0; p < numRoomsInGeneration; p += 2)
        {
            Room[] parents = selectRoom.SelectParents(roomPop);
            Room[] crossOver = crossover.UniformCrossover(parents[0], parents[1], 50, evaluateRoom);
            Room[] mutationResults = new Room[2];
            mutationResults[0] = mutation.RandomReseting(crossOver[0], evaluateRoom);
            mutationResults[1] = mutation.RandomReseting(crossOver[1], evaluateRoom);
            np[p] = mutationResults[0];
            np[p + 1] = mutationResults[1];

            if(np[p].Fitness < np[p + 1].Fitness)
            {
                if(p == 0)
                {
                    chosenRoom = np[p + 1];
                } else
                {
                    if(chosenRoom.Fitness < np[p + 1].Fitness)
                    {
                        chosenRoom = np[p + 1];
                    }
                }
            } else
            {
                if (p == 0)
                {
                    chosenRoom = np[p];
                }
                else
                {
                    if (chosenRoom.Fitness < np[p].Fitness)
                    {
                        chosenRoom = np[p];
                    }
                }
            }
        }
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
        foreach (GameObject c in components)
        {
            AddToDifficulty(c.transform);
        }
    }

    public int GetNumRooms()
    {
        return TileInformation.numRooms;
    }
        // works through each tile of a room and if it implements IDifficulty
    // add it's difficulty to the room score
    public int CalculateRoomDifficulty(int[,] roomData, Transform[] tileTypes)
    {
        int difficulty = 0;

        for (int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for (int x = 0; x < TileInformation.roomSizeX; x++)
            {
                Transform cur = tileTypes[roomData[y, x]];
                if(cur.gameObject.GetComponent<IDifficulty>() != null)
                {
                    difficulty += TileInformation.difficultyScores[roomData[y, x]];
                }
            }
        }

        return difficulty;
    }
}

