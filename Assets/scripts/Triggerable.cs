using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour, IDifficulty {

    public int difficulty;
    private bool triggerable = true;

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

    public void ReplaceTile(string prefabTag, string replacementPrefabTag)
    {
        if (triggerable)
        {
            transform.Find(prefabTag).transform.gameObject.SetActive(false);
            transform.Find(replacementPrefabTag).transform.gameObject.SetActive(true);
            triggerable = false;
        }
    }

    public bool GetIsTriggerable()
    {
        return triggerable;
    }

}
