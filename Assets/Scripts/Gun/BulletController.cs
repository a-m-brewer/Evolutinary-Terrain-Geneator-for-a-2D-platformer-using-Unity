using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private float damage = 0f;
    private string tagOfParent;
    private string badGuyTag;
    private Player player;

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == badGuyTag)
        {
            if (badGuyTag == "Enemy")
            {
                collider.GetComponent<Enemy>().Hurt(damage);
                if (collider.GetComponent<Enemy>().GetHealth() <= 0)
                {
                    player.IncreaseScore(5);   
                }
            } else if (badGuyTag == "Player")
            {
                collider.GetComponent<PlayerMove>().Hurt(damage);
            } else
            {
                Debug.Log("Wrong tag " + badGuyTag);
            }

            collider.GetComponent<MonoBehaviour>().StartCoroutine(collider.GetComponent<Person>().TriggerHurtAnimation());
            
            Destroy(gameObject);
        }

        
        if (collider.tag != tagOfParent && collider.tag != badGuyTag && collider.tag != "Bullet")
        {
            Destroy(gameObject);
        }


        Destroy(gameObject, 2f);
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
}
