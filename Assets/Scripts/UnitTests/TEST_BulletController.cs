using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_BulletController {

    [Test]
    public void TEST_SetDamageOfBullet()
    {
        var bullet = new GameObject().AddComponent<BulletController>();
        bullet.tag = "Bullet";

        float damage = 1f;

        bullet.SetDamage(damage);

        Assert.AreEqual(damage, bullet.GetDamage());
    }

    [Test]
    public void TEST_SetParentTag()
    {
        var bullet = new GameObject().AddComponent<BulletController>();
        bullet.tag = "Bullet";

        string parent = "Player";

        bullet.SetTagOfParent(parent);

        Assert.AreEqual(parent, bullet.GetTagOfParent());
    }

    [Test]
    public void TEST_SetBadGuyTag()
    {
        var bullet = new GameObject().AddComponent<BulletController>();
        bullet.tag = "Bullet";

        string badGuyTag = "Enemy";

        bullet.SetBadGuyTag(badGuyTag);

        Assert.AreEqual(badGuyTag, bullet.GetBadGuyTag());
    }

    [Test]
    public void TEST_SetPlayer()
    {
        var bullet = new GameObject().AddComponent<BulletController>();
        bullet.tag = "Bullet";

        Player p = new Player();

        bullet.SetPlayer(p);

        Assert.AreEqual(p, bullet.GetPlayer());
    }

    [TearDown]
    public void AfterTest()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Object.Destroy(go);
        }
    }
}
