using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour{
    public int movementSpeed;
    public int jumpPower;
    public int attackDamage;
    public int health;
    // is the player facing right
    private bool facingRight = false;
    public float moveX;
    //
    private Rigidbody2D personRB;
    // jump parameters
    public bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    private void Start()
    {
        personRB = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        Debug.Log(isGrounded);
        if (isGrounded)
        {
            Debug.Log("Jump");
            personRB.AddForce(new Vector2(0, jumpPower));
        }
    }

    void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void DirectionCheck()
    {
        if (moveX < 0.0f && !facingRight)
        {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight)
        {
            FlipPlayer();
        }
    }

    public Rigidbody2D GetRigidBody()
    {
        return personRB;
    }

}
