﻿using System.Collections;
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
    public Room[] UniformCrossover(Room roomOne, Room roomTwo, int bias, EvaluateRoom er, float crossoverChance)
    {
        int[,] out1 = roomOne.Data;
        int[,] out2 = roomTwo.Data;
        
        for(int y = 0; y < roomOne.Data.GetLength(0); y++)
        {
            for (int x = 0; x < roomOne.Data.GetLength(1); x++)
            {
                if (UsefulMethods.RandomChance(crossoverChance)) {
                    if (UsefulMethods.RandomChance(bias))
                    {
                        out1[y, x] = roomOne.Data[y, x];
                        out2[y, x] = roomTwo.Data[y, x];
                    }
                    else
                    {
                        out1[y, x] = roomTwo.Data[y, x];
                        out2[y, x] = roomOne.Data[y, x];
                    }
                }
            }
        }

        Room[] newRooms = new Room[2]
        {
            new Room(out1,er),
            new Room(out2,er)
        };

        return newRooms;
    }

    public Room[] MultiPointCrossover(Room roomOne, Room roomTwo, int bias, EvaluateRoom er, float crossoverChance)
    {
        int[,] out1 = roomOne.Data;
        int[,] out2 = roomTwo.Data;

        int blockSizeX = 6;
        int blockSizeY = 5;

        for (int y = 0; y < roomOne.Data.GetLength(0); y += blockSizeY)
        {
            for (int x = 0; x < roomOne.Data.GetLength(1); x += blockSizeX)
            {
                bool which = UsefulMethods.RandomChance(bias);
                for (int row = y; row < y + blockSizeY; row++)
                {
                    if (UsefulMethods.RandomChance(crossoverChance))
                    {
                        for (int col = x; col < x + blockSizeX; col++)
                        {
                            out1[row, col] = (which) ? roomOne.Data[row, col] : roomTwo.Data[row, col];
                            out2[row, col] = (!which) ? roomOne.Data[row, col] : roomTwo.Data[row, col];
                        }
                    }
                }
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
