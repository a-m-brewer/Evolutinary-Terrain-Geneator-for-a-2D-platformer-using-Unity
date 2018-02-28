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
        // N < 1
        if (fromMainChecker[fromMainChecker.Count - 1] < 1f)
        {
            score = fromMainChecker[fromMainChecker.Count - 1];
        }
        // 1 <= N < 4 
        else if (fromMainChecker[0] < 1f || fromMainChecker[1] < 1f || fromMainChecker[2] < 1f)
        {
            float prevStage = fromMainChecker[fromMainChecker.Count - 1];
            score = prevStage + fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[2];
        }
        // 4 <= N
        else
        {
            float prevStage = fromMainChecker[fromMainChecker.Count - 1] + fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[2];
            score = prevStage + fromMainChecker[3] + fromMainChecker[4] + fromMainChecker[5];
        }

        return score;
    }

    //public float Evaluate(Room room)
    //{
    //    float[] fromMainChecker = gr.MainChecker(room);
    //    float score = 0f;

    //    for(int i = 0; i < fromMainChecker.Length; i++)
    //    {
    //        score += fromMainChecker[i];
    //    }

    //    return score;
    //}

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
