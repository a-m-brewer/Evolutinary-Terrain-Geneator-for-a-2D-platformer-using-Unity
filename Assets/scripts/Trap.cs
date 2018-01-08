using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Triggerable, IDifficulty
{

    private void Start()
    {
        DifficultyScore = 5;
    }

}
