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

    private bool fliped;

    public GameObject bulletPrefab;

    // Use this for initialization
    void Awake () {
        firePoint = transform.Find("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("Could not find firePoint Gun.cs");
        }

        gameObject.tag = gameObject.transform.parent.tag;
    }
	
	// Update is called once per frame
	void Update () {

        HandleShootingInput();

        fliped = transform.parent.gameObject.GetComponent<Person>().GetFacingRight();
	}

    // handle the shooting input from the player
    void HandleShootingInput()
    {
        //bool isGrounded = transform.parent.gameObject.GetComponent<Person>().isGrounded;
        bool isPlayer = (transform.parent.gameObject.GetComponent<PlayerMove>() != null);
        bool playerIsFreeToMove = true;
        if (isPlayer)
        {
            playerIsFreeToMove = !transform.parent.gameObject.GetComponent<PlayerMove>().GetIsTrapped();
        }
        
        if (Input.GetButton("Fire1") && playerIsFreeToMove) // && isGrounded)
        {
            Shoot();
        }
    }

    // fire the gun
    public void Shoot()
    {        
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        Vector2 forwardPoint = firePointPosition + gameObject.transform.right.ToVector2();
        
        if (fliped)
        {
            forwardPoint = firePointPosition - gameObject.transform.right.ToVector2();
        }

        if (Time.time > timeToFire)
        {
            SpawnBullet(forwardPoint, firePointPosition);
            timeToFire = Time.time + 1 / fireRate;
        }
    }

    // spawn the bullet
    void SpawnBullet(Vector2 forward, Vector2 firePos)
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<BulletController>().SetDamage(damage);
        bullet.GetComponent<BulletController>().SetTagOfParent(gameObject.tag);
        bullet.GetComponent<BulletController>().SetBadGuyTag(transform.parent.gameObject.GetComponent<Person>().GetBadGuyTag());
        if (transform.parent.tag == "Player")
        {
            bullet.GetComponent<BulletController>().SetPlayer(transform.parent.GetComponent<Player>());
        }
        bullet.tag = "Bullet";
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2((forward - firePos).x * bulletSpeed, bullet.GetComponent<Rigidbody2D>().velocity.y);
    }
}
