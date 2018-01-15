using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * A script to handle displaying the players Health in the
 * UI element
 */
public class UIHealth : MonoBehaviour {

    private Text mytext;
    public Player player;

    public void Start()
    {
        mytext = GetComponent<Text>();
        mytext.text = 0.ToString();
    }

    private void Update()
    {
        mytext.text = "Health: " + player.GetHealth().ToString();
    }
}
