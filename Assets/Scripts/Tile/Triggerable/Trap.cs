using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Triggerable, IDifficulty
{
    // set the difficulty score of a trap from tileinformation
    private void Awake()
    {
        DifficultyScore = TileInformation.difficultyScores[3];    
    }
}
