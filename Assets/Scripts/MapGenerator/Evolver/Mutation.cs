using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutation {

    GeneratorRules gr = new GeneratorRules(DefaultRuleArguments.mutationRate,
                                           DefaultRuleArguments.targetEnemies,
                                           DefaultRuleArguments.maxCoins,
                                           DefaultRuleArguments.maxTraps,
                                           DefaultRuleArguments.checkpoints);

    public float mutationRate = 0f;

    public Mutation()
    {
        mutationRate = gr.GetMutationRate();
    }

    public Room RandomReseting(Room room, EvaluateRoom evaluateRoom)
    {
        int[,] output = room.Data;

        for (int y = 0; y < output.GetLength(0); y++)
        {
            for (int x = 0; x < output.GetLength(1); x++)
            {
                if (UsefulMethods.RandomChance(mutationRate))
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

    public Room SwapMutation(Room room, EvaluateRoom evaluateRoom)
    {
        int[,] output = room.Data;

        for (int y = 0; y < output.GetLength(0); y++)
        {
            for (int x = 0; x < output.GetLength(1); x++)
            {
                if (UsefulMethods.RandomChance(mutationRate))
                {
                    int toSwapX = Random.Range(0, TileInformation.roomSizeX - 1);
                    int toSwapY = Random.Range(0, TileInformation.roomSizeY - 1);

                    int toSwapTile = output[toSwapY, toSwapX];

                    output[toSwapY, toSwapX] = output[y, x];

                    output[y, x] = toSwapTile;
                }
            }
        }

        return new Room(output, evaluateRoom);
    }
}
