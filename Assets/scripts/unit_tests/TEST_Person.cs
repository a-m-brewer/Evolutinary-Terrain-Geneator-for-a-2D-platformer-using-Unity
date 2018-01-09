using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_Person {

	[Test]
	public void TEST_GiveOneHealthToPlayer() {

        var person = new GameObject().AddComponent<Person>();
        person.tag = "TestPerson";

        var intialPlayerHealth = person.health;

        var healthToAdd = 1f;

        var correctResult = intialPlayerHealth + healthToAdd;

        person.GiveHealth(healthToAdd);

        Assert.AreEqual(correctResult, person.health);
	}

    [Test]
    public void TEST_PlayerFlipDirection()
    {
        var person = new GameObject().AddComponent<Person>();
        person.tag = "TestPerson";

        var positionShouldBe = (person.transform.localScale.x * -1);

        person.FlipPlayer();

        var isFliped = (person.transform.localScale.x == positionShouldBe);

        Assert.True(person.GetFacingRight() && isFliped);
    }

    [Test]
    public void TEST_SetBadGuyTagToEnemy()
    {
        var person = new GameObject().AddComponent<Person>();
        person.tag = "TestPerson";

        var tag = "Enemy";

        person.SetBadGuyTag(tag);

        Assert.AreEqual(tag, person.GetBadGuyTag());
    }
    
    [Test]
    public void TEST_HurtingEnemyWhoHasThreeHealth()
    {
        var person = new GameObject().AddComponent<Person>();
        person.tag = "Enemy";

        var startingHealth = 3f;

        person.health = startingHealth;

        var damageToGive = 1f;

        var desiredResult = startingHealth - damageToGive;

        person.Hurt(damageToGive);

        Assert.AreEqual(desiredResult, person.health);

    }

    [Test]
    public void TEST_HurtingEnemyWhoWillDie()
    {
        var person = new GameObject().AddComponent<Person>();
        person.tag = "Enemy";

        var startingHealth = 1f;
        var damageToGive = 1f;

        person.health = startingHealth;

        person.Hurt(damageToGive);

        Debug.Log(person.enabled);

        Assert.True(!person.enabled && person.health == 0);
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
