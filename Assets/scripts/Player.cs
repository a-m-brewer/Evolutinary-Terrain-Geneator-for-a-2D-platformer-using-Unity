using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Person {

    private int playerScore = 0;
    private float invisibleTimeAfterDamage = 2f;
    private bool isTrapped = false;

    public Animator anim;

    private void Start()
    {
        movementSpeed = 10f;
        jumpPower = 600f;
        health = 3;
        anim = GetComponent<Animator>();

        SetBadGuyTag("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickUp"))
        {

            if (collision.transform.gameObject.GetComponent<Coin>().GetIsTriggerable())
            {
                IncreaseScore(10);
                collision.transform.gameObject.GetComponent<Coin>().ReplaceTile("Coin", "Background");
                Debug.Log(GetScore());
            }
        }
        
        if (collision.gameObject.CompareTag("Trap") && collision.transform.GetComponent<Trap>().GetIsTriggerable())
        {
            IEnumerator coroutine = TriggerTrap(collision);
            StartCoroutine(coroutine);
        }
        
    }

    public int GetScore()
    {
        return playerScore;
    }

    private void IncreaseScore(int PointsToAdd)
    {
        playerScore += PointsToAdd; 
    }

    public bool GetIsTrapped()
    {
        return isTrapped;
    }

    IEnumerator TriggerTrap(Collider2D collision)
    {
        Debug.Log("start");
        GetRigidBody().velocity = new Vector2(0f, 0f);
        isTrapped = true;
        yield return new WaitForSeconds(2f);
        isTrapped = false;
        collision.gameObject.GetComponent<Triggerable>().ReplaceTile("Trap", "Background");
        Debug.Log("stop");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            StartCoroutine(TriggerHurtAnimation(collision.collider));

            Hurt(1f);
        }
    }

    IEnumerator TriggerHurtAnimation(Collider2D collider)
    {
        // Start Animation
        anim.SetBool("Damaged", true);
        // Wait For Invisiblity to end
        yield return new WaitForSeconds(invisibleTimeAfterDamage);
        // Stop blinking reenable collison
        anim.SetBool("Damaged", false);
    }
}
