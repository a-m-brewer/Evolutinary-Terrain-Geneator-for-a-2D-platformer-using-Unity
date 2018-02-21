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

    public Population roomPop;
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
        Debug.Log("BEST MAP FITNESS: " + chosenRoom.Fitness);
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
        roomPop = new Population(evaluateRoom, 1, huristicMaps);
        roomPop.bestRooms = SortBest(roomPop.bestRooms);
        chosenRoom = roomPop.bestRooms[0];
    }

    public void IncrementEvolutionOfRoomAndDisplayBest()
    {
        int numRoomsInGeneration = 100;
        Room[] np = new Room[numRoomsInGeneration];
        Room[] newBest = new Room[2];

        np[0] = newBest[0] = roomPop.bestRooms[0];
        np[1] = newBest[1] = roomPop.bestRooms[1];

        for (int p = 2; p < numRoomsInGeneration; p += 2)
        {
            Room[] parents = selectRoom.SelectParents(roomPop.popRooms);
            Room[] crossOver = crossover.UniformCrossover(parents[0], parents[1], 50, evaluateRoom);
            Room[] mutationResults = new Room[2];
            mutationResults[0] = mutation.RandomReseting(crossOver[0], evaluateRoom);
            mutationResults[1] = mutation.RandomReseting(crossOver[1], evaluateRoom);
            np[p] = mutationResults[0];
            np[p + 1] = mutationResults[1];

            newBest = CalculateBest(newBest, np[p]);
            newBest = SortBest(newBest);
            newBest = CalculateBest(newBest, np[p + 1]);
            newBest = SortBest(newBest);
        }
        roomPop.popRooms = np;
        roomPop.bestRooms = newBest;
        chosenRoom = roomPop.bestRooms[0];
    }


    private Room[] SortBest(Room[] _room)
    {
        Room[] sorted = new Room[2];

        if(_room[1].Fitness <= _room[0].Fitness)
        {
            return _room;
        } else
        {
            sorted[0] = _room[1];
            sorted[1] = _room[0];
            return sorted;
        }
    }

    private Room[] CalculateBest(Room[] currBest, Room toCheck)
    {
        Room[] newBest = new Room[2];
        if(currBest[0].Fitness < toCheck.Fitness)
        {
            newBest[0] = toCheck;
            newBest[1] = currBest[1];
            return newBest;
        } else if (currBest[1].Fitness < toCheck.Fitness)
        {
            newBest[0] = currBest[0];
            newBest[1] = toCheck;
            return newBest;
        }
        return currBest;
    }

    public int n = 0;
    public void SwitchToRoomNinPopulation(int n)
    {
        chosenRoom = roomPop.popRooms[n];
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

