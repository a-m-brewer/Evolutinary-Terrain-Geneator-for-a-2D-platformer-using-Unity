using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour{
    public float movementSpeed;
    public float jumpPower;
    public float attackDamage;
    public float health;
    // is the player facing right
    private bool facingRight = false;
    public float moveX;
    //
    private Rigidbody2D personRB;
    // jump parameters
    public bool isGrounded = false;

    private void Start()
    {
        personRB = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        if(isGrounded)
        {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
