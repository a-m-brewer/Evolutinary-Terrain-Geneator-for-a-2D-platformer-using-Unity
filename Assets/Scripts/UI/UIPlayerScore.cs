using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * A script to handle displaying the players score in the
 * UI element
 */ 
public class UIPlayerScore : MonoBehaviour {

    private Text mytext;
    public Player player;

    public void Start()
    {
        mytext = GetComponent<Text>();
        mytext.text = 0.ToString();
    }

    private void Update()
    {
        mytext.text = "Score: " + player.GetScore().ToString();
    }
}
