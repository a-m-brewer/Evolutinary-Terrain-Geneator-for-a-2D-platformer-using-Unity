using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * A script to handle displaying the players difficulty score in the
 * UI element
 */
public class UIDifficultyScore : MonoBehaviour {

    private Text mytext;
    public MapGeneratorMain map;

	// Use this for initialization
	void Start () {
        mytext = GetComponent<Text>();
        mytext.text = "Difficutly Score Here!";
	}
	
	// Update is called once per frame
	void Update () {
        //mytext.text = map.DifficultyScore.ToString();
        mytext.text = "Target Difficulty: " + map.mapTargetDifficulty + " Actual Difficulty: " + map.actualDifficultyScore;
	}
}
