using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutation {

    GeneratorRules gr = new GeneratorRules();

    public Room RandomReseting(Room room, EvaluateRoom evaluateRoom)
    {
        int[] output = room.Data;

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

        return new Room(output, evaluateRoom);
    }
}
