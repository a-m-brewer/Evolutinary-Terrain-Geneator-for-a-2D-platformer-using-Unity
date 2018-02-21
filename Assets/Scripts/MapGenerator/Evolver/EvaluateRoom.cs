﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to calculate the suitablility of a room
/// </summary>
public class EvaluateRoom {

    private float targetGroundPercentage;
    private GeneratorRules gr = new GeneratorRules(DefaultRuleArguments.mutationRate,
                                                   DefaultRuleArguments.targetEnemies,
                                                   DefaultRuleArguments.maxCoins,
                                                   DefaultRuleArguments.maxTraps);

    /// <summary>
    /// Init method for the class that will evaluate a rooms fitness
    /// </summary>
    /// <param name="groundPercent">amount of the ground that should be floor</param>
    public EvaluateRoom(float groundPercent)
    {
        this.targetGroundPercentage = groundPercent;
    }

    public float Evaluate(Room room)
    {
        float[] fromMainChecker = gr.MainChecker(room);
        float score = 0f;
        // if path can't be found return how close
        if(fromMainChecker[0] < 1f)
        {
            Debug.Log("NO PATH " + fromMainChecker[0]);
            score = fromMainChecker[0];
        } else if(fromMainChecker[1] < 1f)
        {
            Debug.Log("ENEMIES NOT CLOSE TO LIMIT: " + fromMainChecker[1]);
            score = fromMainChecker[0] + fromMainChecker[1];

        } else if(fromMainChecker[4] < 1f)
        {
            Debug.Log("ENEMIES NOT ON FLOOR " + fromMainChecker[4]);
            score = fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[4];
        }

        // TODO: Setup other rules

        return score;
    }

    public float[] EvaluatePopulation(Room[] pop)
    {
        float[] result = new float[pop.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Evaluate(pop[i]);
            Debug.Log(i + " " + result[i]);
        }
        return result;
    }

    public float GetGroundPercent()
    {
        return this.targetGroundPercentage;
    }
}
