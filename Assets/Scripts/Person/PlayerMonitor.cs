using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMonitor : MonoBehaviour {

    public int amountOfJumps = 0;
    public int amountOfDeaths = 0;
    public int amountPressedRight = 0;
    public int amountPressedLeft = 0;

    public float startTime = 0f;
    public float timer = 0f;
    public float timeToFinishLevel = 0f;

    public float timeSpentGoingRight = 0f;
    public float timeSpentGoingLeft = 0f;
    public float timeSpentJumping = 0f;

    public int bulletsFired = 0;
    public int bulletsHit = 0;

    public int coinsCollected = 0;
    public int trapsTriggered = 0;
    public int enemiesKilled = 0;

    public int gapsFallenInto = 0;

    public int timesShotByEnemy = 0;
    public int timesHitByEnemy = 0;

    private void Update()
    {
        UpdateTimer();
    }

    public void SetFinalTime()
    {
        timeToFinishLevel = timer;
    }

    public void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void IncreaseJumps()
    {
        amountOfJumps++;
    }

    public void IncreaseDeaths()
    {
        amountOfDeaths++;
    }

    public void IncreasePressedLeft()
    {
        amountPressedLeft++;
    }

    public void IncreasePressedRight()
    {
        amountPressedRight++;
    }

    public void IncreaseBulletsFired()
    {
        bulletsFired++;
    }

    public void IncreaseBulletsHit()
    {
        bulletsHit++;
    }

    public void IncreaseCoinsCollected()
    {
        coinsCollected++;
    }

    public void IncreaseTrapsTriggered()
    {
        trapsTriggered++;
    }

    public void IncreaseEnemiesKilled()
    {
        enemiesKilled++;
    }

    public void IncreaseGapsFallenInto()
    {
        gapsFallenInto++;
    }

    public void IncreaseShotByEnemy()
    {
        timesShotByEnemy++;
    }

    public void IncreaseHitByEnemy()
    {
        timesHitByEnemy++;
    }

    public void IncreaseTimeGoingRight()
    {
        timeSpentGoingRight += Time.deltaTime;
    }

    public void IncreaseTimeGoingLeft()
    {
        timeSpentGoingLeft += Time.deltaTime;
    }

    public void IncreaseTimeJumping()
    {
        timeSpentJumping += Time.deltaTime;
    }

    public void PlayerMonitorReset()
    {
        amountOfJumps = 0;
        amountOfDeaths = 0;
        amountPressedRight = 0;
        amountPressedLeft = 0;

        startTime = 0f;
        timer = 0f;
        timeToFinishLevel = 0f;

        timeSpentGoingRight = 0f;
        timeSpentGoingLeft = 0f;
        timeSpentJumping = 0f;

        bulletsFired = 0;
        bulletsHit = 0;

        coinsCollected = 0;
        trapsTriggered = 0;
        enemiesKilled = 0;

        gapsFallenInto = 0;

        timesShotByEnemy = 0;
        timesHitByEnemy = 0;
    }
}
