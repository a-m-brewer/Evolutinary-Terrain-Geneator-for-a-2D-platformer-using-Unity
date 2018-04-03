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

    public float Evaluate(Room room)
    {
        List<float> fromMainChecker = gr.MainChecker(room);
        float score = 0f;
        // if path can't be found return how close

        if (fromMainChecker[fromMainChecker.Count - 1] < 1f || fromMainChecker[fromMainChecker.Count - 2] < 1f)
        {
            score = fromMainChecker[fromMainChecker.Count - 1] + fromMainChecker[fromMainChecker.Count - 2];
        }
        else if (fromMainChecker[0] < 1f || fromMainChecker[1] < 1f || fromMainChecker[2] < 1f)
        {
            float prevStage = fromMainChecker[fromMainChecker.Count - 1] + fromMainChecker[fromMainChecker.Count - 2];
            score = prevStage + fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[2];
        }
        else
        {
            float prevStage = fromMainChecker[fromMainChecker.Count - 1] + fromMainChecker[fromMainChecker.Count - 2] + fromMainChecker[0] + fromMainChecker[1] + fromMainChecker[2];
            score = prevStage + fromMainChecker[3] + fromMainChecker[4] + fromMainChecker[5] + fromMainChecker[6];
            //score = prevStage + fromMainChecker[6];
        }

        return score;
    }

    public float EvaluateAll(Room room)
    {
        List<float> fromMainChecker = gr.MainChecker(room);
        float score = 0f;
        foreach(float f in fromMainChecker)
        {
            score += f;
        }
        return score;
    }
}
