using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPopulation {

    public Room[] bestRooms = new Room[2];

    public Room[] Generate(TextAsset huristicRooms)
    {
        InitRandomPopulation irp = new InitRandomPopulation();
        Room[] randomRooms = irp.population;

        InitHuristicRooms ihr = new InitHuristicRooms(huristicRooms);
        Room[] hRooms = ihr.Rooms;

        // join both together
        Room[] population = new Room[randomRooms.Length + hRooms.Length];
        Array.Copy(randomRooms, population, randomRooms.Length);
        Array.Copy(hRooms, 0, population, randomRooms.Length, hRooms.Length);

        bestRooms[0] = irp.bestRoom;
        bestRooms[1] = ihr.bestRoom;

        return population;
    }
}
