using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public Room chosenRoom;
    private EvaluateRoom evaluateRoom = new EvaluateRoom();
    // Difficultys
    public int mapTargetDifficulty;
    public int actualDifficultyScore;

    public Population roomPop;
    SelectRoom selectRoom = new SelectRoom();
    Crossover crossover = new Crossover();
    Mutation mutation = new Mutation();
    MergeSortRoom msr = new MergeSortRoom();
    public int generation = 0;
    public int MAX_GENERATIONS;
    public int mapInitMode = 1;

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
        newRoom.GetComponent<RoomGenerator>().EvoGenerateRoom(0, mapHolder, chosenRoom);

    }

    public void InitMap()
    {
        generation = 1;
        roomPop = new Population(mapInitMode, huristicMaps);

        foreach(Room r in roomPop.popRooms)
        {
            r.Fitness = evaluateRoom.Evaluate(DefaultRuleArguments.evaluationMode, r);
        }

        roomPop.topTwenty = msr.MergeSort(roomPop.popRooms.ToList());

        roomPop.topTwenty.Reverse();
        //Debug.Log(roomPop.topTwenty[0].Fitness + " " + roomPop.topTwenty[roomPop.topTwenty.Count - 1].Fitness);

        roomPop.topTwenty = roomPop.topTwenty.Take(20).ToList();
        //Debug.Log(roomPop.topTwenty[0].Fitness + " " + roomPop.topTwenty[roomPop.topTwenty.Count - 1].Fitness);

        roomPop.bestRooms = roomPop.topTwenty.Take(2).ToArray();
        //Debug.Log(roomPop.bestRooms[0].Fitness + " " + roomPop.bestRooms[1].Fitness);

        chosenRoom = roomPop.bestRooms[0];
        //Debug.Log(chosenRoom.Fitness);
    }

    public void IncrementEvolutionOfRoomAndDisplayBest()
    {
        List<Room> np = roomPop.topTwenty;
        //List<Room> np = new List<Room>();

        int numRoomsInGeneration = DefaultRuleArguments.populationSize;

        for (int p = 20; p < numRoomsInGeneration; p += 2)
        {
            np = EvoMode(np, 0, 0, 0);
        }

        np = msr.MergeSort(np);
        np.Reverse();

        roomPop.popRooms = np.ToArray();
        roomPop.topTwenty = np.Take(20).ToList();
        roomPop.bestRooms = np.Take(2).ToArray();

        chosenRoom = roomPop.bestRooms[0];
        generation++;
    }

    private List<Room> EvoMode(List<Room> pop, int selectMode, int crossoverMode, int mutationMode)
    {
        Room[] parents;
        switch(selectMode)
        {
            case 0:
                parents = selectRoom.SelectParentsRoulette(roomPop.popRooms);
                break;
            case 1:
                parents = selectRoom.SelectParentsTournament(roomPop.popRooms, 10);
                break;
            default:
                parents = selectRoom.SelectParentsRoulette(roomPop.popRooms);
                break;
        }
        Room[] rcrossover;
        switch(crossoverMode)
        {
            case 0:
                rcrossover = crossover.UniformCrossover(parents[0], parents[1], 50);
                break;
            case 1:
                rcrossover = crossover.MultiPointCrossover(parents[0], parents[1], 50);
                break;
            case 2:
                rcrossover = crossover.OnePointCrossover(parents[0], parents[1]);
                break;
            default:
                rcrossover = crossover.UniformCrossover(parents[0], parents[1], 50);
                break;

        }
        Room[] mutationResults = new Room[2];
        switch(mutationMode)
        {
            case 0:
                mutationResults[0] = mutation.RandomReseting(rcrossover[0]);
                mutationResults[1] = mutation.RandomReseting(rcrossover[1]);
                break;
            case 1:
                mutationResults[0] = mutation.SwapMutation(rcrossover[0]);
                mutationResults[1] = mutation.SwapMutation(rcrossover[1]);
                break;
            case 2:
                mutationResults[0] = mutation.ScrambleMutation(rcrossover[0]);
                mutationResults[1] = mutation.ScrambleMutation(rcrossover[1]);
                break;
            default:
                mutationResults[0] = mutation.RandomReseting(rcrossover[0]);
                mutationResults[1] = mutation.RandomReseting(rcrossover[1]);
                break;
        }
        //mutationResults[0] = mutation.SwapMutation(crossOver[0], evaluateRoom);
        //mutationResults[1] = mutation.SwapMutation(crossOver[1], evaluateRoom);

        mutationResults[0].Fitness = evaluateRoom.Evaluate(DefaultRuleArguments.evaluationMode, mutationResults[0]);
        mutationResults[1].Fitness = evaluateRoom.Evaluate(DefaultRuleArguments.evaluationMode, mutationResults[1]);

        pop.Add(mutationResults[0]);
        pop.Add(mutationResults[1]);
        return pop;
    }

    public int n = 0;
    public void SwitchToRoomNinPopulation(int n)
    {
        chosenRoom = roomPop.popRooms[n];
    }

    // add the difficulty of the each rooms to get the maps score
    public void CalculateDifficulty()
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

    public void EditorDebugLog(double i)
    {
        Debug.Log(i);
    }

    public void InvokeRepeatingEvolution()
    {
        InvokeRepeating("ToInvoke", 0f, 0.3f);
    }

    public void CancelInvokeEvolution()
    {
        CancelInvoke("ToInvoke");
    }

    public void ToInvoke()
    {
        if(generation == MAX_GENERATIONS)
        {
            CancelInvoke("ToInvoke");
        }
        IncrementEvolutionOfRoomAndDisplayBest();
        DisplayRoom();
    }
}

