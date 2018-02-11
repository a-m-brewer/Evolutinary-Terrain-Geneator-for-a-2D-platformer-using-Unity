using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossover {

    /// <summary>
    /// Cross over two rooms using uniform crossover
    /// https://www.tutorialspoint.com/genetic_algorithms/genetic_algorithms_crossover.htm
    /// </summary>
    /// <param name="roomOne">The first room</param>
    /// <param name="roomTwo">The Second room</param>
    /// <param name="bias">How likely it is to choose the first room out of 100</param>
    /// <returns></returns>
    public Room[] UniformCrossover(Room roomOne, Room roomTwo, int bias, EvaluateRoom er)
    {
        int[] out1 = new int[roomOne.Data.Length];
        int[] out2 = new int[roomTwo.Data.Length];
        
        for(int i = 0; i < roomOne.Data.Length; i++)
        {
            if(UsefulMethods.RandomChance(bias))
            {
                out1[i] = roomOne.Data[i];
                out2[i] = roomTwo.Data[i];
            } else
            {
                out1[i] = roomTwo.Data[i];
                out2[i] = roomOne.Data[i];
            }
        }

        Room[] newRooms = new Room[2]
        {
            new Room(out1,er),
            new Room(out2,er)
        };

        return newRooms;
    }

}
