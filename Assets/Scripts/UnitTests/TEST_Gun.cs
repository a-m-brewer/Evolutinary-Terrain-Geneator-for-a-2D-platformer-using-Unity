using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_Gun {

    [Test]
    public void TEST_Shoot()
    {
        var player = new GameObject();

        player.tag = "Player";
        player.AddComponent<Rigidbody2D>();

        var gun = new GameObject();
        var shootingPoint = new GameObject();
        var bullet = new GameObject();

        gun.transform.parent = player.transform;
        shootingPoint.transform.parent = gun.transform;
        shootingPoint.name = "FirePoint";

        player.AddComponent<Player>();
        gun.AddComponent<Gun>();
        bullet.AddComponent<BulletController>();
        bullet.AddComponent<Rigidbody2D>();

        gun.GetComponent<Gun>().bulletPrefab = bullet;

        gun.GetComponent<Gun>().Shoot();

        bool bulletInWorld = GameObject.FindGameObjectWithTag("Bullet");

        Assert.True(bulletInWorld);
    }


    [TearDown]
    public void AfterTest()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Object.Destroy(go);
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            Object.Destroy(go);
        }
    }
}
