using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to handle the movement of the player
public class PlayerMove : Player
{

    PlayerMonitor mon;

    private void Awake()
    {
        mon = gameObject.GetComponent<PlayerMonitor>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        HandlePlayerMonitor();
    }


    void MovePlayer()
    {
        //moveX = 0f;

        moveX = Input.GetAxisRaw("Horizontal");

        if (!GetIsTrapped())
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                isGrounded = false;
            }

            // Player direction
            DirectionCheck();
            // Physics
            GetRigidBody().velocity =  new Vector2(moveX * movementSpeed, GetRigidBody().velocity.y);
        }
    }

    private void HandlePlayerMonitor()
    {
        HandlePlayerTimeInDirection();
        HandleTimeJumping();
        HandleKeyPresses();
    }

    private void HandlePlayerTimeInDirection()
    {
        if (moveX == 1f)
        {
            mon.IncreaseTimeGoingRight();
        }
        if (moveX == -1f)
        {
            mon.IncreaseTimeGoingLeft();
        }
    }

    private void HandleTimeJumping()
    {
        if(!isGrounded)
        {
            mon.IncreaseTimeJumping();
        }
    }

    private void HandleKeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mon.IncreasePressedLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            mon.IncreasePressedRight();
        }
    }
}
