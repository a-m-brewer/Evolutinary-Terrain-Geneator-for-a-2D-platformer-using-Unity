using System.Collections;
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
        for(int i = 0; i < fromMainChecker.Length; i++)
        {
            if (i == 1)
            {
                Debug.Log(i + ": " + fromMainChecker[i]);
                continue;
            }
            score += fromMainChecker[i];
            Debug.Log(i + ": " + fromMainChecker[i]);
        }

        score /= fromMainChecker.Length;

        return (score == 0f) ? 0.01f : score;
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
