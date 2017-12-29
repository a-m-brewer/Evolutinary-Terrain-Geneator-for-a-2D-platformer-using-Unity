using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
