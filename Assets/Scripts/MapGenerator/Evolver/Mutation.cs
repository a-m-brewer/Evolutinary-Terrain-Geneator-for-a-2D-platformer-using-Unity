using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutation {

    GeneratorRules gr = new GeneratorRules();

    public int[] RandomReseting(int[] room)
    {
        int[] output = new int[room.Length];
        for(int i = 0; i < output.Length; i++)
        {
            if(UsefulMethods.RandomChance(gr.GetMutationRate()))
            {
                // only allow gap tiles if the array is setting the bottom layer
                if (i < TileInformation.roomSizeX)
                {
                    output[i] = Random.Range(0, TileInformation.numTiles);
                } else
                {
                    output[i] = Random.Range(0, TileInformation.numTiles - 1);
                }
            }
        }

        return output;
    }
}
