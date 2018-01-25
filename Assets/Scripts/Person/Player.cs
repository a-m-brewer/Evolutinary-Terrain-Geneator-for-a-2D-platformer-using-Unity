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
        jumpPower = 700f;
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
            HandleCoinCollison(collision);
        }
        
        // if the player colides with a trap that is triggerable
        if (collision.gameObject.CompareTag("Trap"))
        {
            HandleTrapCollision(collision);
        }

        // if the player hits the end level flag end the level
        if (collision.gameObject.CompareTag("Flag"))
        {
            HandleFlagCollision();
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
            HandleEnemyCollision();
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
        SwitchTiles(collision, "Trap", "Background");
    }

    private void TriggerTrapCoroutines(Collider2D col)
    {
        StartCoroutine(TriggerHurtAnimation());
        StartCoroutine(TriggerTrap(col));
    }

    private void SwitchTiles(Collider2D col, string current, string updated)
    {
        col.gameObject.GetComponent<Triggerable>().ReplaceTile(current, updated);
    }

    private void HandleCoinCollison(Collider2D col)
    {
        if (col.transform.gameObject.GetComponent<Coin>().GetIsTriggerable())
        {
            // add to score and swith the tile for a background tile
            IncreaseScore(10);
            SwitchTiles(col, "Coin", "Background");
            gameObject.GetComponent<PlayerMonitor>().IncreaseCoinsCollected();
        }
    }

    private void HandleTrapCollision(Collider2D col)
    {
        if(col.transform.GetComponent<Trap>().GetIsTriggerable())
        {
            gameObject.GetComponent<PlayerMonitor>().IncreaseTrapsTriggered();
            // hurt the player and trigger the hurt animation
            Hurt(1f);
            TriggerTrapCoroutines(col);
        }
    }

    private void HandleFlagCollision()
    {
        levelEnded = true;
        gameObject.GetComponent<PlayerMonitor>().SetFinalTime();
    }

    private void HandleEnemyCollision()
    {
        gameObject.GetComponent<PlayerMonitor>().IncreaseHitByEnemy();
        Hurt(1f);
        StartCoroutine(TriggerHurtAnimation());
    }

}
