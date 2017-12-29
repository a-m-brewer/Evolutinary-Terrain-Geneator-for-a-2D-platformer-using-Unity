using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Player {

	// Update is called once per frame
	void FixedUpdate () {
        MovePlayer();
        isGrounded = isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
    }

    void MovePlayer()
    {
        moveX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("PlayerMove");
            Jump();
        }
        // Player direction
        DirectionCheck();
        // Physics
        GetRigidBody().velocity = new Vector2(moveX * movementSpeed, GetRigidBody().velocity.y);
    }



}

