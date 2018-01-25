using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public float fireRate = 1f;
    public float damage = 1f;
    public float bulletSpeed = 15f;
    public LayerMask canHit;
    private float timeToFire = 0f;
    public Transform firePoint;

    private bool wielderIsPlayer = false;

    public GameObject bulletPrefab;

    // Use this for initialization
    void Awake () {
        firePoint = transform.Find("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("Could not find firePoint Gun.cs");
        }

        gameObject.tag = gameObject.transform.parent.tag;

        wielderIsPlayer = transform.parent.GetComponent<Player>() != null;
    }
	
	// Update is called once per frame
	void Update () {
        HandleShootingInput();
	}

    // handle the shooting input from the player
    void HandleShootingInput()
    {       
        if (Input.GetButton("Fire1") && PlayerCanMove())
        {
            Shoot();
        }
    }

    private bool PlayerCanMove()
    {
        bool playerIsFreeToMove = true;

        if (wielderIsPlayer)
        {
            playerIsFreeToMove = !transform.parent.gameObject.GetComponent<PlayerMove>().GetIsTrapped();
        }

        return playerIsFreeToMove;
    }

    // fire the gun
    public void Shoot()
    {        
        if (Time.time > timeToFire)
        {
            SpawnBullet(GetForwardPointPosition(), GetFirePointPosition());
            timeToFire = Time.time + 1 / fireRate;
        }
    }

    private Vector2 GetForwardPointPosition()
    {
        Vector2 forwardPoint = GetFirePointPosition() + gameObject.transform.right.ToVector2();

        if (GetIsFlipped())
        {
            forwardPoint = GetFirePointPosition() - gameObject.transform.right.ToVector2();
        }

        return forwardPoint;
    }

    private Vector2 GetFirePointPosition()
    {
        return new Vector2(firePoint.position.x, firePoint.position.y);
    }

    // spawn the bullet
    void SpawnBullet(Vector2 forward, Vector2 firePos)
    {
        GameObject bullet = SetupBullet();

        PropelBullet(bullet, forward, firePos);    
    }

    private void PropelBullet(GameObject b, Vector2 forward, Vector2 firePos)
    {
        b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, -1f);
        b.GetComponent<Rigidbody2D>().velocity = new Vector2((forward - firePos).x * bulletSpeed, b.GetComponent<Rigidbody2D>().velocity.y);
    }

    private GameObject SetupBullet()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<BulletController>().SetDamage(damage);
        bullet.GetComponent<BulletController>().SetTagOfParent(gameObject.tag);
        bullet.GetComponent<BulletController>().SetBadGuyTag(transform.parent.gameObject.GetComponent<Person>().GetBadGuyTag());
        if (wielderIsPlayer)
        {
            bullet.GetComponent<BulletController>().SetPlayer(transform.parent.GetComponent<Player>());
        }
        bullet.tag = "Bullet";

        return bullet;
    }

    private bool GetIsFlipped()
    {
        return transform.parent.gameObject.GetComponent<Person>().GetFacingRight();
    }
}
