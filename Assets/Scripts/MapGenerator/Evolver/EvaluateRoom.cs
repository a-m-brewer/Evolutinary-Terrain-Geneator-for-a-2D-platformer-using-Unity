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
                                                   DefaultRuleArguments.maxTraps,
                                                   DefaultRuleArguments.checkpoints);

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
        List<float> fromMainChecker = gr.MainChecker(room);
        float score = 0f;
        // if path can't be found return how close

        if (fromMainChecker[fromMainChecker.Count - 1] < 1f)
        {
            score = fromMainChecker[fromMainChecker.Count - 1];
        }
        else if (fromMainChecker[0] < 1f || fromMainChecker[1] < 1f || fromMainChecker[2] < 1f)
        {
            float prevStage = fromMainChecker[fromMainChecker.Count - 1];
            score = prevStage + fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[2];
        }
        else
        {
            float prevStage = fromMainChecker[fromMainChecker.Count - 1] + fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[2];
            score = prevStage + fromMainChecker[3] + fromMainChecker[4] + fromMainChecker[5] + fromMainChecker[6];
        }

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
