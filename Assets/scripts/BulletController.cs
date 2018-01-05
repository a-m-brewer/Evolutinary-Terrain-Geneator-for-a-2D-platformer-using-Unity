using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private float damage = 0f;
    private string tagOfParent;
    private string badGuyTag;
    

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.tag == badGuyTag)
        {
            if (badGuyTag == "Enemy")
            {
                collider.GetComponent<Enemy>().Hurt(damage);
            } else if (badGuyTag == "Player")
            {
                collider.GetComponent<PlayerMove>().Hurt(damage);
            } else
            {
                Debug.Log("Wrong tag " + badGuyTag);
            }
            
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

    public void SetTagOfParent(string s)
    {
        tagOfParent = s;
    }

    public void SetBadGuyTag(string t)
    {
        badGuyTag = t;
    }
}
