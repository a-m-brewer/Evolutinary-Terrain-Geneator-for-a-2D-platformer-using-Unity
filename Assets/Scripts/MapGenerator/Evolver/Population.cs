﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population {
    public Room[] bestRooms;
    public Room[] popRooms;
    public List<Room> topTwenty;
    public float averageFitness;

    /// <summary>
    /// InitPopulation
    /// </summary>
    /// <param name="evaluateRoom"></param>
    /// <param name="setting">0: huristic, 1: random, 2: both</param>
    public Population(EvaluateRoom evaluateRoom, int setting, TextAsset huristicMaps)
    {
        switch(setting)
        {
            case 0:
                InitHuristicRooms ihr = new InitHuristicRooms(huristicMaps, evaluateRoom);
                this.popRooms = ihr.Rooms;
                this.bestRooms = new Room[2] { ihr.bestRoom, ihr.bestRoom };
                break;
            case 1:
                InitRandomPopulation irp = new InitRandomPopulation(evaluateRoom);
                this.popRooms = irp.Generate(evaluateRoom);
                this.bestRooms = new Room[2] { irp.bestRoom, irp.bestRoom };
                break;
            case 2:
                InitPopulation ip = new InitPopulation();
                this.popRooms = ip.Generate(huristicMaps, evaluateRoom);
                bestRooms = ip.bestRooms;
                break;
            default:
                Debug.Log("INCORRECT SETTING");
                break;
        }

    }
}
