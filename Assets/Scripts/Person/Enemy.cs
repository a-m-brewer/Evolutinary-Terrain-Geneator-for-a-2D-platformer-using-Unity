using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Person, IDifficulty {

    public LayerMask enemyMask;

    private const int pointsForKilling = 5;

    private bool hasGun = false;

    private int difficulty;
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
        SetupDifficulty();
    }

    // Use this for initialization
    private void Start()
    {
        movementSpeed = 4;

        SetBadGuyTag("Player");

        EnemyShoot();
    }

    private new void FixedUpdate()
    {
        HandleEnemyRotation();

        DirectionCheck();

        EnemyMove();
    }

    private void EnemyMove()
    {
        Vector2 enemyVelocity = GetRigidBody().velocity;
        enemyVelocity.x = transform.right.x * movementSpeed;
        GetRigidBody().velocity = enemyVelocity;
    }

    private bool EnemyIsGrounded(Vector2 foe)
    {
        Debug.DrawLine(foe, foe - Vector2.up, Color.white);
        return Physics2D.Linecast(foe, foe - Vector2.up * 1.10f, enemyMask);
    }

    private bool EnemyIsBlocked(Vector2 foe)
    {
        Debug.DrawLine(foe, foe + transform.right.ToVector2() * 0.3f, Color.white);
        return Physics2D.Linecast(foe, foe + transform.right.ToVector2() * 0.03f, enemyMask);
    }

    private void RotateEnemy()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += 180f;
        transform.eulerAngles = currentRotation;
    }

    private void EnemyShootGun()
    {
        gameObject.transform.GetChild(0).GetComponent<Gun>().Shoot();
    }

    public int GetPointsForKilling()
    {
        return pointsForKilling;
    }

    private void SetupDifficulty()
    {
        hasGun = (gameObject.transform.childCount > 0);
        // For some reason this needs to be in awake
        if (hasGun)
        {
            DifficultyScore = TileInformation.difficultyScores[5];
        }
        else
        {
            DifficultyScore = TileInformation.difficultyScores[4];
        }
    }

    private void HandleEnemyRotation()
    {
        if (NeedsRotation())
        {
            RotateEnemy();
        }
    }

    private bool NeedsRotation()
    {
        Vector2 frontOfEnemyBot = RayCastFrontPosition(0.75f);
        Vector2 frontOfEnemyTop = RayCastFrontPosition(1.5f);

        isGrounded = EnemyIsGrounded(frontOfEnemyBot);
        bool isBlockedBot = EnemyIsBlocked(frontOfEnemyBot);
        bool isBlockedTop = EnemyIsBlocked(frontOfEnemyTop);

        return !isGrounded || (isBlockedBot || isBlockedTop);
    }

    private Vector2 RayCastFrontPosition(float positionOnBody)
    {
        Vector2 size = GetWidthAndHeight();
        return (transform.position.ToVector2() - Vector2.up) + transform.right.ToVector2() * size.x + Vector2.up * (size.y * positionOnBody);
    }

    private Vector2 GetWidthAndHeight()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        return new Vector2(sr.bounds.extents.x, sr.bounds.extents.y);
    }

    // delay 3 secs at start and then shoot as much as possible
    private void EnemyShoot()
    {
        if (hasGun)
        {
            // So that the enemy can't shoot right away and kill you
            InvokeRepeating("EnemyShootGun", 3f, 1f);
        }
    }

}
