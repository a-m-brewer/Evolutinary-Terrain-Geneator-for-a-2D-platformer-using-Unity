using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_InitRoom {

	[Test]
	public void TEST_GenerateARandomRoom() {
        CreateRoom cr = new CreateRoom();
        EvaluateRoom evaluateRoom = new EvaluateRoom(0.75f);
        Room ranRoom = cr.Generate(evaluateRoom);

        bool inRange = true;

        for (int i = 0; i < ranRoom.Data.Length; i++)
        {
            if (!(0 <= ranRoom.Data[i] && ranRoom.Data[i] <= 6))
            {
                inRange = false;
                break;
            }
        }

        Assert.True(inRange);

	}

    [Test]
    public void TEST_CorrectLength()
    {
        CreateRoom cr = new CreateRoom();
        EvaluateRoom evaluateRoom = new EvaluateRoom(0.75f);
        Room ranRoom = cr.Generate(evaluateRoom);

        Assert.True((ranRoom.Data.Length == (TileInformation.roomSizeX * TileInformation.roomSizeY)));

    }
}
