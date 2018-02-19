using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPopulation {

    public Room bestRoom;

    public Room[] Generate(TextAsset huristicRooms, float percentGround, EvaluateRoom evaluateRoom)
    {
        InitRandomPopulation irp = new InitRandomPopulation(percentGround, evaluateRoom);
        Room[] randomRooms = irp.population;

        InitHuristicRooms ihr = new InitHuristicRooms(huristicRooms, evaluateRoom);
        Room[] hRooms = ihr.Rooms;

        // join both together
        Room[] population = new Room[randomRooms.Length + hRooms.Length];
        Array.Copy(randomRooms, population, randomRooms.Length);
        Array.Copy(hRooms, 0, population, randomRooms.Length, hRooms.Length);

        SetBestRoom(irp, ihr);

        return population;
    }

    public void SetBestRoom(InitRandomPopulation _irp, InitHuristicRooms _ihr)
    {
        bestRoom = (_irp.bestRoom.Fitness < _ihr.bestRoom.Fitness) ? _ihr.bestRoom : _irp.bestRoom;
    }

}
