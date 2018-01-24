using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Methods that apply to both the player and the enemy
 */
public class Person : MonoBehaviour{
    public float movementSpeed;
    public float jumpPower;
    public float attackDamage;
    public float health;
    private float originalHealth;
    // is the player facing right
    private bool facingRight = false;
    public float moveX;
    //
    public Rigidbody2D personRB;
    public Animator anim;
    public float damageAnimationTime = 2f;
    // jump parameters
    public bool isGrounded = false;
    public bool levelEnded = false;
    private string badGuyTag;

    //public MapGeneratorMain map;

    private void Start()
    {
        // somthing wrong with this for enemy
        personRB = GetComponent<Rigidbody2D>();
        if (personRB == null || anim == null)
        {
            Debug.Log("whoops");
        }
    }

    public void FixedUpdate()
    {
        if (PesonIsBellowMap())
        {
            Hurt(health);
        }
    }

    public void Jump()
    {
        if(isGrounded)
        {
            if (gameObject.GetComponent<PlayerMonitor>() != null)
                gameObject.GetComponent<PlayerMonitor>().IncreaseJumps();

            personRB.AddForce(new Vector2(0, jumpPower));
        }
    }

    // flip the sprite of the person
    public void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Flip if the player changes direction
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

    // hurt the person for a given damage
    public void Hurt(float damageToGive)
    {
        health -= damageToGive;
        if (health <= 0)
        {
            if (gameObject.tag == "Player")
            {
                RestartMap();
            }
            if(gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }
    }

    // set the animation bool to trigger hurt animation for
    // N seconds
    public IEnumerator TriggerHurtAnimation()
    {
        // Start Animation
        anim.SetBool("Damaged", true);
        // Wait For Invisiblity to end
        yield return new WaitForSeconds(damageAnimationTime);
        // Stop blinking reenable collison
        anim.SetBool("Damaged", false);
    }

    // returns true if the player is bellow the map
    public bool PesonIsBellowMap()
    {
        return transform.position.y < (-1.5f);
    }

    // if level ended is true you are taken to the Difficulty seclection screen
    // This also handles the reset so to restart the map levelEnded only needs
    // to be true
    private void RestartMap()
    {
        if (gameObject.GetComponent<PlayerMonitor>() != null)
            gameObject.GetComponent<PlayerMonitor>().IncreaseDeaths();

        levelEnded = true;
    }

    // various setters and getters for variables
    public void GiveHealth(float healthToGive)
    {
        health += healthToGive;
    }

    public float GetHealth()
    {
        return health;
    }

    public void ResetHealth()
    {
        health = originalHealth;
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

    public void SetOriginalHealth(float ihealth)
    {
        originalHealth = ihealth;
    }

    public float GetOriginalHealth()
    {
        return originalHealth;
    }

    public bool IsDead()
    {
        return GetHealth() <= 0f;
    }
}
