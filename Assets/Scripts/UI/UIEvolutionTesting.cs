using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEvolutionTesting : MonoBehaviour {

    private Text mytext;
    public MapGenDisplay map;

    // Use this for initialization
    void Start()
    {
        mytext = GetComponent<Text>();
        mytext.text = "Difficutly Score Here!";
    }

    // Update is called once per frame
    void Update()
    {
        //mytext.text = map.DifficultyScore.ToString();
        mytext.text = "Target Difficulty: " + map.mapTargetDifficulty + " Actual Difficulty: " + map.actualDifficultyScore;
    }

}
