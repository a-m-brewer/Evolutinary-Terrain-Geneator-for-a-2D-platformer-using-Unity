using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_Crossover {

	[Test]
	public void TEST_UniformCrossover() {

        int[,] room1 = new int[10, 24]
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

        int[,] room2 = new int[10, 24]
        {
            {1,1,1,1,6,6,1,1,1,1,1,1,1,6,6,6,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,1,1,1,0,1,1,1,0,0,0,1,1,1,0,0,0,0,1},
            {0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        var evaluateRoom = new EvaluateRoom();
        var roomOne = new Room(room1, evaluateRoom);
        var roomTwo = new Room(room2, evaluateRoom);

        Crossover co = new Crossover();
        Room[] crossover = co.UniformCrossover(roomOne, roomTwo, 50, evaluateRoom);
        bool result = false;
        for(int y = 0; y < roomOne.Data.GetLength(0); y++)
        {
            for (int x = 0; x < roomOne.Data.GetLength(1); x++)
            {
                bool testOne = crossover[0].Data[y, x] == roomOne.Data[y, x]
                    && crossover[1].Data[y, x] == roomTwo.Data[y, x];
                bool testTwo = crossover[1].Data[y, x] == roomOne.Data[y, x]
                    && crossover[0].Data[y, x] == roomTwo.Data[y, x];
                result = testOne || testTwo;
            }
        }
        Assert.True(result);
	}

    [Test]
    public void TEST_MultipointCrossover()
    {
        int[,] room1 = new int[10, 24]
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

        int[,] room2 = new int[10, 24]
        {
            {1,1,1,1,6,6,1,1,1,1,1,1,1,6,6,6,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,1,1,1,0,1,1,1,0,0,0,1,1,1,0,0,0,0,1},
            {0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        var evaluateRoom = new EvaluateRoom();
        var roomOne = new Room(room1, evaluateRoom);
        var roomTwo = new Room(room2, evaluateRoom);

        Crossover co = new Crossover();
        Room[] crossover = co.MultiPointCrossover(roomOne, roomTwo, 50, evaluateRoom);

        int blockSizeX = 6;
        int blockSizeY = 5;

        bool result = false;

        for (int y = 0; y < roomOne.Data.GetLength(0); y += blockSizeY)
        {
            for (int x = 0; x < roomOne.Data.GetLength(1); x += blockSizeX)
            {
                bool which = UsefulMethods.RandomChance(50);
                for (int row = y; row < y + blockSizeY; row++)
                {
                    for (int col = x; col < x + blockSizeX; col++)
                    {
                        result = (which) ? room1[row, col] == crossover[0].Data[row, col] : room2[row, col] == crossover[0].Data[row, col];
                        result = (which) ? room2[row, col] == crossover[1].Data[row, col] : room1[row, col] == crossover[1].Data[row, col];
                    }
                }
            }
        }
        Assert.True(result);
    }
}
