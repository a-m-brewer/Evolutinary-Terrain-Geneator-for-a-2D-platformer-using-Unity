using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestInitMap {

    [Test]
    public void TestIfMapsInit()
    {
        InitMap im = new InitMap();

        int[][] testMap = im.Generate();

        bool result = true;

        for(int rows = 0; rows < testMap.Length; rows++)
        {
            for (int elements = 0; elements < testMap[rows].Length; elements++)
            {
                if (!(0 <= testMap[rows][elements] && testMap[rows][elements] <= 6))
                {
                    result = false;
                    break;
                }
            }
            if (!result)
                break;
        }

        Assert.True(result);
    }


    [Test]
    public void TestMapCorrectSize()
    {
        InitMap im = new InitMap();

        int[][] testMap = im.Generate();

        bool result = testMap.Length == TileInformation.numRooms;

        Assert.True(result);
    }
}
