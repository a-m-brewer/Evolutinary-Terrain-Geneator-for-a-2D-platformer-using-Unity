using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public MapGeneratorMain map;

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
        if (personRB.position.y < (-1f))
        {
            Hurt(health);
        }
    }

    public void Jump()
    {
        if(isGrounded)
        {
            personRB.AddForce(new Vector2(0, jumpPower));
        }
    }

    // test done
    public void FlipPlayer()
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
                //string currSceneName = SceneManager.GetActiveScene().name;
                //SceneManager.LoadScene(currSceneName);
                RestartMap();
            }
            if(gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }
        StartCoroutine(TriggerHurtAnimation());
        Debug.Log(health);
    }

    public IEnumerator TriggerHurtAnimation()
    {
        Debug.Log("actually happens " + gameObject.tag);
        // Start Animation
        anim.SetBool("Damaged", true);
        // Wait For Invisiblity to end
        yield return new WaitForSeconds(damageAnimationTime);
        // Stop blinking reenable collison
        anim.SetBool("Damaged", false);
    }

    private void RestartMap()
    {
        levelEnded = true;
    }

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
}
