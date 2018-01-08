using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Person : MonoBehaviour{
    public float movementSpeed;
    public float jumpPower;
    public float attackDamage;
    public float health;
    // is the player facing right
    private bool facingRight = false;
    public float moveX;
    //
    public Rigidbody2D personRB;
    // jump parameters
    public bool isGrounded = false;

    private string badGuyTag;

    private void Start()
    {
        // somthing wrong with this for enemy
        personRB = GetComponent<Rigidbody2D>();

        if (personRB == null)
        {
            Debug.Log("whoops");
        }
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

    public void Hurt(float damageToGive)
    {
        health -= damageToGive;
        if (health <= 0)
        {
            if (gameObject.tag == "Player")
            {
                string currSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currSceneName);
            }
            if(gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }
        Debug.Log(health);
    }

    public void GiveHealth(float healthToGive)
    {
        health += healthToGive;
    }

    public float GetHealth()
    {
        return health;
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }

    public string GetBadGuyTag()
    {
        return badGuyTag;
    }

    public void SetBadGuyTag(string t)
    {
        badGuyTag = t;
    }
}
