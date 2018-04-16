using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_InitPopulation {

	[Test]
	public void TEST_InitPopulationSimplePasses() {
        // Use the Assert class to test conditions.
        TextAsset textAsset = Resources.Load("20maps") as TextAsset;

        EvaluateRoom evaluateRoom = new EvaluateRoom();

        InitPopulation initPopulation = new InitPopulation();

        Room[] testRooms = initPopulation.Generate(textAsset);

        int[,] endRoom = new int[10, 24]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,4,0,0,1,1,1,1,1,1},
            {1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
            {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        Assert.AreEqual(endRoom, testRooms[testRooms.Length - 1].Data);
        
	}

}
