using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Methods that are unique to the player
 */ 
public class Player : Person {

    private int playerScore = 0;
    private bool isTrapped = false;

    private void Start()
    {
        movementSpeed = 10f;
        //jumpPower = 16f;
        jumpPower = 14.5f;
        health = 3;
        SetBadGuyTag("Enemy");
        SetOriginalHealth(health);
        // start the level ended so that the level difficulty
        // Selector comes up
        levelEnded = true;
    }

    // handles the player collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the player hits a coin
        if (collision.gameObject.CompareTag("PickUp"))
        {
            // if the coin has been triggerd before don't add to score
            if (collision.transform.gameObject.GetComponent<Coin>().GetIsTriggerable())
            {
                // add to score and swith the tile for a background tile
                IncreaseScore(10);
                collision.transform.gameObject.GetComponent<Coin>().ReplaceTile("Coin", "Background");
                gameObject.GetComponent<PlayerMonitor>().IncreaseCoinsCollected();
            }
        }
        
        // if the player colides with a trap that is triggerable
        if (collision.gameObject.CompareTag("Trap") && collision.transform.GetComponent<Trap>().GetIsTriggerable())
        {
            gameObject.GetComponent<PlayerMonitor>().IncreaseTrapsTriggered();
            // hurt the player and trigger the hurt animation
            Hurt(1f);
            StartCoroutine(TriggerHurtAnimation());
            // trap the player
            IEnumerator coroutine = TriggerTrap(collision);
            StartCoroutine(coroutine);
        }
        // if the player hits the end level flag end the level
        if (collision.gameObject.CompareTag("Flag"))
        {
            levelEnded = true;
            gameObject.GetComponent<PlayerMonitor>().SetFinalTime();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            gameObject.GetComponent<PlayerMonitor>().IncreaseHitByEnemy();
            Hurt(1f);
            StartCoroutine(TriggerHurtAnimation());
        }
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void IncreaseScore(int PointsToAdd)
    {
        playerScore += PointsToAdd; 
    }

    public void ResetScore()
    {
        playerScore = 0;
    }

    public bool GetIsTrapped()
    {
        return isTrapped;
    }

    // Stop the player for a given period of time when trapped
    IEnumerator TriggerTrap(Collider2D collision)
    {
        GetRigidBody().velocity = new Vector2(0f, 0f);
        isTrapped = true;
        yield return new WaitForSeconds(2f);
        isTrapped = false;
        collision.gameObject.GetComponent<Triggerable>().ReplaceTile("Trap", "Background");
    }

}
