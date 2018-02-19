using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutation {

    GeneratorRules gr = new GeneratorRules(DefaultRuleArguments.mutationRate,
                                           DefaultRuleArguments.maxEnemies,
                                           DefaultRuleArguments.maxCoins,
                                           DefaultRuleArguments.maxTraps);

    public Room RandomReseting(Room room, EvaluateRoom evaluateRoom)
    {
        int[,] output = room.Data;

        for (int y = 0; y < output.GetLength(0); y++)
        {
            for (int x = 0; x < output.GetLength(1); x++)
            {
                if (UsefulMethods.RandomChance(gr.GetMutationRate()))
                {
                    // only allow gap tiles if the array is setting the bottom layer
                    if (x < TileInformation.roomSizeX)
                    {
                        output[y,x] = Random.Range(0, TileInformation.numTiles);
                    }
                    else
                    {
                        output[y,x] = Random.Range(0, TileInformation.numTiles - 1);
                    }
                }
            }
        }

        return new Room(output, evaluateRoom);
    }
}
