using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_Player {

    [Test]
    public void TEST_IncreaseScoreOfThePlayerByOne()
    {    
        var player = new GameObject().AddComponent<Player>();
        player.tag = "Player";

        int playerInitScore = 0;
        int pointsToAdd = 1;
        int endResult = playerInitScore + pointsToAdd;

        player.IncreaseScore(pointsToAdd);

        Assert.AreEqual(player.GetScore(), endResult);
        
    }

    [Test]
    public void TEST_ResetPlayerScore()
    {
        var player = new GameObject().AddComponent<Player>();
        player.tag = "Player";

        player.IncreaseScore(1);
        int scoreBeforeReset = player.GetScore();

        bool scoreBeforeResetGreaterThanZero = scoreBeforeReset > 0;

        player.ResetScore();
        bool isReset = player.GetScore() == 0;

        Assert.True(scoreBeforeResetGreaterThanZero && isReset);
    }

    [TearDown]
    public void AfterTest()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("TestPerson"))
        {
            Object.Destroy(go);
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            Object.Destroy(go);
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Object.Destroy(go);
        }
    }
}
