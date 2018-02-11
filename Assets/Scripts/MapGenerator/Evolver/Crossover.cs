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
    public int[][] UniformCrossover(int[] roomOne, int[] roomTwo, int bias)
    {
        int[][] output = new int[2][];
        for(int i = 0; i < roomOne.Length; i++)
        {
            if(UsefulMethods.RandomChance(bias))
            {
                output[0][i] = roomOne[i];
                output[1][i] = roomTwo[i];
            } else
            {
                output[0][i] = roomTwo[i];
                output[1][i] = roomOne[i];
            }
        }
        return output;
    }

}
