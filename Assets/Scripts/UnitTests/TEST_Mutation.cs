using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_Mutation {

	[Test]
	public void TEST_RandomReseting() {
        Mutation mutation = new Mutation();
        EvaluateRoom evaluateRoom = new EvaluateRoom();
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
        Room room = new Room(roomData, evaluateRoom);

        //mutation.mutationRate = 100f;

        Room mutatedRoom = mutation.RandomReseting(room, evaluateRoom);

        bool result = room.Data[0, 0] != mutatedRoom.Data[0, 0];
        Debug.Log(roomData[0, 0]  + " " + mutatedRoom.Data[0, 0]);

        Assert.True(result);

    }

}
