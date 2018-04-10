using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_InitRandomPopulation {

	[Test]
	public void TEST_Generate() {
        // Use the Assert class to test conditions.
        EvaluateRoom evaluateRoom = new EvaluateRoom();

        InitRandomPopulation initRandomPopulation = new InitRandomPopulation(evaluateRoom);

        Room[] testRooms = initRandomPopulation.Generate(evaluateRoom);

        bool length = testRooms[0].Data.GetLength(0) == 10 && testRooms[0].Data.GetLength(1) == 24;

        bool correctNumbers = false;

        foreach(int i in testRooms[0].Data)
        {
            correctNumbers = 0 <= i && i <= 6;
            if(!(0 <= i && i <= 6))
            {
                break;
            }
        }

        Assert.True(length && correctNumbers);

	}

}
