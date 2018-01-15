using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for handling the logic for coins and traps
 */ 
public class Triggerable : MonoBehaviour, IDifficulty {

    public int difficulty;
    private bool triggerable = true;
    // implimentation of IDifficulty
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

    // replace the trap or coin tile with a background tile
    public void ReplaceTile(string prefabTag, string replacementPrefabTag)
    {
        if (triggerable)
        {
            transform.Find(prefabTag).transform.gameObject.SetActive(false);
            transform.Find(replacementPrefabTag).transform.gameObject.SetActive(true);
            triggerable = false;
        }
    }

    // if the trap is triggered, triggerable is set to false meaning it's actions won't
    // be calculated
    public bool GetIsTriggerable()
    {
        return triggerable;
    }

}
