using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class BulletController : MonoBehaviour {

    private float damage = 0f;
    private float timeToDie = 1.7f;
    private string tagOfParent;
    private string badGuyTag;
    private Player player;

    private void Start()
    {
        DestroyGameBulletAfterTime(timeToDie);

        if(player != null)
            player.gameObject.GetComponent<PlayerMonitor>().IncreaseBulletsFired();
    }

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == badGuyTag && IsValidBadGuyTag(collider))
        {
            HandleCollision(collider, damage);

            HandlePlayerScore(collider, player);

            collider.GetComponent<MonoBehaviour>().StartCoroutine(collider.GetComponent<Person>().TriggerHurtAnimation());

            if (player != null)
            {
                player.gameObject.GetComponent<PlayerMonitor>().IncreaseBulletsHit();
                if (collider.GetComponent<Person>().IsDead())
                {
                    player.gameObject.GetComponent<PlayerMonitor>().IncreaseEnemiesKilled();
                }

            }

            if (collider.tag == "Player")
                collider.gameObject.GetComponent<PlayerMonitor>().IncreaseShotByEnemy();

            Destroy(gameObject);
        }

        DestroyBulletIfNotTarget(collider, GetBadGuyTag(), GetTagOfParent());

    }

    private void HandleCollision(Collider2D col, float d)
    {
        col.GetComponent<Person>().Hurt(d);
    }

    private void HandlePlayerScore(Collider2D col, Player p)
    {

        Person target = col.GetComponent<Person>();

        if (ColidedWithEnemy(col))
        {
            if (target.IsDead())
            {
                p.IncreaseScore(col.GetComponent<Enemy>().GetPointsForKilling());
            }
        }
    }

    private bool ColidedWithEnemy(Collider2D col)
    {
        return (col.GetComponent<Enemy>() != null);
    }

    private void DestroyBulletIfNotTarget(Collider2D col, string bgTag, string pTag)
    {
        if (!(IsValidTarget(col, bgTag, pTag)))
        {
            Destroy(gameObject);
        }
    }

    private bool IsValidBadGuyTag(Collider2D col)
    {
        Regex reg = new Regex("(^Player$|^Enemy$)");

        return reg.IsMatch(col.tag);
    }

    private bool IsValidTarget(Collider2D col, string bgTag, string pTag)
    {
        Regex reg = new Regex("(^Bullet$|^" + pTag + "$|^" + bgTag + "$)");

        return reg.IsMatch(col.tag);
    }
    

    public void SetDamage(float d)
    {
        damage = d;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetTagOfParent(string s)
    {
        tagOfParent = s;
    }

    public string GetTagOfParent()
    {
        return tagOfParent;
    }

    public void SetBadGuyTag(string t)
    {
        badGuyTag = t;
    }

    public string GetBadGuyTag()
    {
        return badGuyTag;
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public Player GetPlayer()
    {
        return player;
    }

    private void DestroyGameBulletAfterTime(float time)
    {
        Destroy(gameObject, time);
    }
}
