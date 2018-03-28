using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TEST_GeneratorRules {

	[Test]
	public void TEST_MainChecker() {
        // Use the Assert class to test conditions.

        List<int[]> checkpoints = new List<int[]>();

        GeneratorRules gr = new GeneratorRules(DefaultRuleArguments.mutationRate,
            DefaultRuleArguments.targetEnemies, DefaultRuleArguments.maxCoins,
            DefaultRuleArguments.maxTraps, checkpoints);

        int[,] roomData = new int[10, 24]
{
            {1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0},
            {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };

        EvaluateRoom evaluateRoom = new EvaluateRoom();

        Room r = new Room(roomData, evaluateRoom);

        List<float> output = gr.MainChecker(r);

        float[] expectedResults = new float[]
        {
            0.9801987f,
            0.9801987f,
            0.9801987f,
            1,
            1,
            1,
            0.9997753f,
            1
        };

        bool result = false;

        for (int i = 0; i < output.Count; i++)
        {
            result = output[i] == expectedResults[i];
        }

        Assert.True(result);
	}

}
