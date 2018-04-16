using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_InitRoom {

	[Test]
	public void TEST_Generate() {
        // Use the Assert class to test conditions.
        EvaluateRoom evaluateRoom = new EvaluateRoom();

        CreateRoom createRoom = new CreateRoom();

        Room room = createRoom.Generate();

        bool length = room.Data.GetLength(0) == 10 && room.Data.GetLength(1) == 24;

        bool correctNumbers = false;

        foreach (int i in room.Data)
        {
            correctNumbers = 0 <= i && i <= 6;
            if (!(0 <= i && i <= 6))
            {
                break;
            }
        }

        Assert.True(length && correctNumbers);

    }

}
