using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IDifficulty
{
    private int difficulty;

    public int DifficultyScore
    {
        get
        {
            return this.difficulty;
        }

        set
        {
            this.difficulty = value;
        }
    }

    private void Start()
    {
        DifficultyScore = 5;
    }

}
