﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoom {

    private int populationToSelect = 40;
    public int ToSelect { get { return this.populationToSelect; } }


    private Room[] SelectRandom(Room[] rooms, int amount)
    {
        Room[] randomMaps = new Room[amount];
        for (int i = 0; i < amount; i++)
        {
            int indexRandom = UnityEngine.Random.Range(0, rooms.Length - 1);
            randomMaps[i] = rooms[indexRandom];
        }
        return randomMaps;
    }

    private Room SelectRandomRoom(Room[] rooms)
    {
        return rooms[UnityEngine.Random.Range(0, rooms.Length - 1)];
    }

    /// <summary>
    /// Select a random room with higher fitness rooms getting selected more often
    /// </summary>
    /// <param name="populationPool"></param>
    /// <returns></returns>
    public Room RouletteWheelSelectionOfRoom(Room[] populationPool)
    {
        float sumOfAllRoomsFitness = 0f;
        for (int i = 0; i < populationPool.Length; i++)
        {
            sumOfAllRoomsFitness += populationPool[i].Fitness;
        }
        float randomNumberInFitnessPool = UnityEngine.Random.Range(0, sumOfAllRoomsFitness);
        float partialFitness = 0f;
        for (int i = 0; i < populationPool.Length; i++)
        {
            partialFitness += populationPool[i].Fitness;
            if(randomNumberInFitnessPool < partialFitness)
            {
                Debug.Log("Returned a room from the roulette style");
                return populationPool[i];
            }
        }
        Debug.Log("Returned Random");
        return SelectRandomRoom(populationPool);
    }

    public Room[] SelectParents(Room[] populationPool)
    {
        Room[] parents = new Room[2];
        for (int i = 0; i < parents.Length; i++)
            parents[i] = RouletteWheelSelectionOfRoom(populationPool);
        return parents;
    }
    
}
