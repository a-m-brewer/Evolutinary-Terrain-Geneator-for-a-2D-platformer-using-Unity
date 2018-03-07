using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_InitRoom {

	[Test]
	public void TEST_GenerateARandomRoom() {
        CreateRoom cr = new CreateRoom();
        EvaluateRoom evaluateRoom = new EvaluateRoom();
        Room ranRoom = cr.Generate(evaluateRoom);

        bool inRange = true;

        for (int y = 0; y < ranRoom.Data.GetLength(0); y++)
        {
            for (int x = 0; x < ranRoom.Data.GetLength(1); x++)
            {
                if(!(0 <= ranRoom.Data[y,x] && ranRoom.Data[y,x] <= 6))
                {
                    inRange = false;
                    break;
                }
            }
        }

        Assert.True(inRange);

	}

    [Test]
    public void TEST_CorrectLength()
    {
        CreateRoom cr = new CreateRoom();
        EvaluateRoom evaluateRoom = new EvaluateRoom();
        Room ranRoom = cr.Generate(evaluateRoom);

        Assert.True((ranRoom.Data.Length == (TileInformation.roomSizeX * TileInformation.roomSizeY)));

    }
}
