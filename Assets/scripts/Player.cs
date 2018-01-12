using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Person {

    private int playerScore = 0;
    private bool isTrapped = false;

    private void Start()
    {
        movementSpeed = 10f;
        jumpPower = 700f;
        health = 3;
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
            Hurt(1f);
            IEnumerator coroutine = TriggerTrap(collision);
            StartCoroutine(coroutine);
        }

        if (collision.gameObject.CompareTag("Flag"))
        {
            Debug.Log("Hit End Flag");
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
            Hurt(1f);
        }
    }
}
