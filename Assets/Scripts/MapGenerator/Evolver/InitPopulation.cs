using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPopulation {

    public Room[] Generate(TextAsset huristicRooms, float percentGround, EvaluateRoom evaluateRoom)
    {
        Room[] randomRooms = new InitRandomPopulation(percentGround, evaluateRoom).population;
        Room[] hRooms = new InitHuristicRooms(huristicRooms, evaluateRoom).Rooms;
        // join both together
        Room[] population = new Room[randomRooms.Length + hRooms.Length];
        Array.Copy(randomRooms, population, randomRooms.Length);
        Array.Copy(hRooms, 0, population, randomRooms.Length, hRooms.Length);

        Archive(population);
        return population;
    }

    private void Archive(Room[] pop)
    {
        TextFileWriter t = new TextFileWriter();
        t.WriteRoomsToFile(pop);
    }

}
