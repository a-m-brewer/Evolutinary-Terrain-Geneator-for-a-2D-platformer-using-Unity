using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        mytext.text = map.DifficultyScore.ToString();
	}
}
