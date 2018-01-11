using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour, IDifficulty {

    public int difficulty;
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
        DifficultyScore = TileInformation.difficultyScores[6];
    }
}
