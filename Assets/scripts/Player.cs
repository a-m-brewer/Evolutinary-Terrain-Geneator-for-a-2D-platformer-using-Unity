using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Person {

    private int playerScore = 0;
    private bool isTrapped = false;

    public Player() {
        movementSpeed = 10f;
        jumpPower = 600f;
        health = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickUp"))
        {
            IncreaseScore(10);
            Debug.Log(GetScore());
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Trap"))
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
        collision.gameObject.SetActive(false);
        Debug.Log("stop");
    }
}
