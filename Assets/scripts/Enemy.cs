using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Person, IDifficulty {

    public LayerMask enemyMask;
    private int difficulty;
    private Transform enemyTransform;
    private float enemyWidth;
    private float enemyHeight;

    private bool isBlocked = false;


    public int DifficultyScore
    {
        get
        {
            return this.difficulty;
        }

        set
        {
            this.difficulty = value;
        }
    }

    // Use this for initialization
    private void Start()
    {
        DifficultyScore = 10;
        movementSpeed = 4;
        enemyTransform = this.transform;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        enemyWidth = sr.bounds.extents.x;
        enemyHeight = sr.bounds.extents.y;

        SetBadGuyTag("Player");
    }

    private void FixedUpdate()
    {
        Vector2 frontOfEnemy = (enemyTransform.position.ToVector2() - Vector2.up) + enemyTransform.right.ToVector2() * enemyWidth + Vector2.up * enemyHeight;

        isGrounded = EnemyIsGrounded(frontOfEnemy);

        isBlocked = EnemyIsBlocked(frontOfEnemy);

        if (!isGrounded || isBlocked)
        {
            RotateEnemy();
        }

        DirectionCheck();

        EnemyMove();
        if (gameObject.transform.childCount > 0)
        {
            gameObject.transform.GetChild(0).GetComponent<Gun>().Shoot();
        }        
    }

    private void EnemyMove()
    {
        Vector2 enemyVelocity = GetRigidBody().velocity;
        enemyVelocity.x = enemyTransform.right.x * movementSpeed;
        GetRigidBody().velocity = enemyVelocity;
    }

    private bool EnemyIsGrounded(Vector2 foe)
    {
        Debug.DrawLine(foe, foe - Vector2.up, Color.white);
        return Physics2D.Linecast(foe, foe - Vector2.up * 1.10f, enemyMask);
    }

    private bool EnemyIsBlocked(Vector2 foe)
    {
        Debug.DrawLine(foe, foe + enemyTransform.right.ToVector2() * 0.2f, Color.white);
        return Physics2D.Linecast(foe, foe + enemyTransform.right.ToVector2() * 0.02f, enemyMask);
    }

    private void RotateEnemy()
    {
        Vector3 currentRotation = enemyTransform.eulerAngles;
        currentRotation.y += 180;
        enemyTransform.eulerAngles = currentRotation;
    }
}
