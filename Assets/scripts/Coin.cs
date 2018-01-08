using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Triggerable{

    // DIFFICUTLY SCORE MUST ALWAYS BE IN AWAKE 
    public void Awake()
    {
        DifficultyScore = 1;
    }
    
}
