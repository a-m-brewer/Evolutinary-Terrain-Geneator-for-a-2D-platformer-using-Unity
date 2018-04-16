using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population {
    public Room[] bestRooms;
    public Room[] popRooms;
    public List<Room> topTwenty;

    /// <summary>
    /// InitPopulation
    /// </summary>
    /// <param name="evaluateRoom"></param>
    /// <param name="setting">0: huristic, 1: random, 2: both</param>
    public Population(int setting, TextAsset huristicMaps)
    {
        switch(setting)
        {
            case 0:
                InitHuristicRooms ihr = new InitHuristicRooms(huristicMaps);
                this.popRooms = ihr.Rooms;
                this.bestRooms = new Room[2] { ihr.bestRoom, ihr.bestRoom };
                break;
            case 1:
                InitRandomPopulation irp = new InitRandomPopulation();
                this.popRooms = irp.Generate();
                this.bestRooms = new Room[2] { irp.bestRoom, irp.bestRoom };
                break;
            case 2:
                InitPopulation ip = new InitPopulation();
                this.popRooms = ip.Generate(huristicMaps);
                bestRooms = ip.bestRooms;
                break;
            default:
                Debug.Log("INCORRECT SETTING");
                break;
        }

    }
}
