using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Person, IDifficulty {

    public LayerMask enemyMask;
    private int difficulty;
    private Transform enemyTransform;
    private float enemyWidth;
    private float enemyHeight;

    public bool isBlockedBot = false;
    public bool isBlockedTop = false;

    private const int pointsForKilling = 5;

    private bool hasGun = false;

    public int DifficultyScore
    {
        get
        {
            return difficulty;
        }

        set
        {
            difficulty = value;
        }
    }

    private void Awake()
    {
        hasGun = (gameObject.transform.childCount > 0);
        // For some reason this needs to be in awake
        if (hasGun)
        {
            DifficultyScore = TileInformation.difficultyScores[5];
        } else
        {
            DifficultyScore = TileInformation.difficultyScores[4];
        }
    }

    // Use this for initialization
    private void Start()
    {
        movementSpeed = 4;
        enemyTransform = this.transform;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        enemyWidth = sr.bounds.extents.x;
        enemyHeight = sr.bounds.extents.y;

        SetBadGuyTag("Player");

        if (hasGun)
        {
            // So that the enemy can't shoot right away and kill you
            InvokeRepeating("EnemyShootGun", 3f, 1f);
        }

    }

    private new void FixedUpdate()
    {
        Vector2 frontOfEnemyBot = (enemyTransform.position.ToVector2() - Vector2.up) + enemyTransform.right.ToVector2() * enemyWidth + Vector2.up * (enemyHeight * 0.75f);
        Vector2 frontOfEnemyTop = (enemyTransform.position.ToVector2() - Vector2.up) + enemyTransform.right.ToVector2() * enemyWidth + Vector2.up * (enemyHeight * 1.5f);

        isGrounded = EnemyIsGrounded(frontOfEnemyBot);

        isBlockedBot = EnemyIsBlocked(frontOfEnemyBot);
        isBlockedTop = EnemyIsBlocked(frontOfEnemyTop);

        if (!isGrounded || (isBlockedBot || isBlockedTop))
        {
            RotateEnemy();
        }

        DirectionCheck();

        EnemyMove();
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
        Debug.DrawLine(foe, foe + enemyTransform.right.ToVector2() * 0.3f, Color.white);
        return Physics2D.Linecast(foe, foe + enemyTransform.right.ToVector2() * 0.03f, enemyMask);
    }

    private void RotateEnemy()
    {
        Vector3 currentRotation = enemyTransform.eulerAngles;
        currentRotation.y += 180f;
        enemyTransform.eulerAngles = currentRotation;
    }

    private void EnemyShootGun()
    {
        gameObject.transform.GetChild(0).GetComponent<Gun>().Shoot();
    }

    public int GetPointsForKilling()
    {
        return pointsForKilling;
    }

}
