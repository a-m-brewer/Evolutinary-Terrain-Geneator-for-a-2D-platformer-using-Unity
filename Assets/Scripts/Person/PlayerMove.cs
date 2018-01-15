using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to handle the movement of the player
public class PlayerMove : Player
{

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }


    void MovePlayer()
    {
        moveX = Input.GetAxis("Horizontal");

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
            GetRigidBody().velocity = new Vector2(moveX * movementSpeed, GetRigidBody().velocity.y);
        }
    }
}
