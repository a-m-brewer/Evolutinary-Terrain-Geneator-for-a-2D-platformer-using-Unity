using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_InitRoom {

	[Test]
	public void TEST_GenerateARandomRoom() {
        CreateRoom cr = new CreateRoom();
        int[] ranRoom = cr.Generate();

        bool inRange = true;

        for (int i = 0; i < ranRoom.Length; i++)
        {
            if (!(0 <= ranRoom[i] && ranRoom[i] <= 6))
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
        int[] ranRoom = cr.Generate();

        Assert.True((ranRoom.Length == (TileInformation.roomSizeX * TileInformation.roomSizeY)));

    }
}
