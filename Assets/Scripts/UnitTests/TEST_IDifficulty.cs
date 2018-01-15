using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_IDifficulty {

    [Test]
    public void TEST_SetDifficulty()
    {
        var trap = new GameObject().AddComponent<Trap>();

        int diffToSetTo = 3;
        trap.DifficultyScore = diffToSetTo;

        Assert.AreEqual(diffToSetTo, trap.DifficultyScore);
    }

    [Test]
    public void TEST_GetDifficulty()
    {
        var trap = new GameObject().AddComponent<Trap>();
        int diffToGet = 3;
        trap.DifficultyScore = 3;
        Assert.AreEqual(diffToGet, trap.DifficultyScore);
    }
}
