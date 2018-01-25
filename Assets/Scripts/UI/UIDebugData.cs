using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugData : MonoBehaviour {

    private Text mytext;
    public Canvas c;
    public PlayerMonitor player;
    private bool on = false;

    public void Start()
    {
        c.gameObject.SetActive(on);
        mytext = GetComponent<Text>();
        mytext.text = 0.ToString();
    }

    private void Update()
    {

        mytext.text = "Amount of Jumps: " + player.amountOfJumps.ToString() + "\n" +
                      "Amount of Deaths: " + player.amountOfDeaths.ToString() + "\n" +
                      "Amount of Right Key Presses: " + player.amountPressedRight.ToString() + "\n" +
                      "Amount of Left Key Presses: " + player.amountPressedLeft.ToString() + "\n" +
                      "Timer: " + player.timer.ToString("f0") + "\n" +
                      "Time spent going left: " + player.timeSpentGoingLeft.ToString("f0") + "\n" +
                      "Time spent going right: " + player.timeSpentGoingRight.ToString("f0") + "\n" +
                      "Time spent jumping: " + player.timeSpentJumping.ToString("f0") + "\n" +
                      "Bullets Fired: " + player.bulletsFired.ToString() + "\n" +
                      "Bullets Hit: " + player.bulletsHit.ToString() + "\n" +
                      "Coins Collected: " + player.coinsCollected.ToString() + "\n" +
                      "Traps Triggered: " + player.trapsTriggered.ToString() + "\n" +
                      "Enemies Killed: " + player.enemiesKilled.ToString() + "\n" +
                      "Gaps Fallen into: " + player.gapsFallenInto.ToString() + "\n" +
                      "Times shot by enemy: " + player.timesShotByEnemy.ToString() + "\n" +
                      "Times hit by Enemy: " + player.timesHitByEnemy.ToString() + "\n" +
                      "Finish Time: " + player.timeToFinishLevel.ToString("f0");

 
    }

    public void ShowDebug()
    {
        c.gameObject.SetActive(!on);
        on = !on;
    }
}
