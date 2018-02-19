using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
// class to handle the movement of the player
public class PlayerMove : Player
{
    public UIDebugData ui;
    PlayerMonitor mon;

    bool hasJumped = false;
    Stopwatch s = new Stopwatch();

    private void Awake()
    {
        mon = gameObject.GetComponent<PlayerMonitor>();
    }

    // Update is called once per frame
    void Update()
    {
        LimitFallSpeed();
        MovePlayer();
        HandlePlayerMonitor();

        if(isGrounded && hasJumped)
        {
            s.Stop();
            hasJumped = false;
        }
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
                

                hasJumped = true;

                s = new Stopwatch();
                s.Start();

                isGrounded = false;
            }

            // Player direction
            DirectionCheck();
            // Physics    
            GetRigidBody().velocity =  new Vector2(moveX * movementSpeed, GetRigidBody().velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ui.ShowDebug();
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
