using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to calculate the suitablility of a room
/// </summary>
public class EvaluateRoom {

    private float targetGroundPercentage;
    private GeneratorRules gr = new GeneratorRules();

    /// <summary>
    /// Init method for the class that will evaluate a rooms fitness
    /// </summary>
    /// <param name="groundPercent">amount of the ground that should be floor</param>
    public EvaluateRoom(float groundPercent)
    {
        this.targetGroundPercentage = groundPercent;
    }

    /// <summary>
    /// Get the score of an individual room
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public float Evaluate(Room room)
    {
        float[] roomScoreRaw = gr.MainChecker(room);
        float groundScore = EvaluationDistrobution(roomScoreRaw[0]);
        float score = groundScore * roomScoreRaw[1] * roomScoreRaw[3] * roomScoreRaw[4];
        // give maps that do not score a stake in the pool
        // this is mainly used for initialisation where random generated levels often score 0
        return (score == 0f) ? 0.01f : score;
    }
    
    /// <summary>
    /// Get the gausian disribution like the one in MATLAB gaussmf
    /// </summary>
    /// <param name="X">The number you want to find the membership of</param>
    /// <param name="len">length from the mean to the point at 0.5</param>
    /// <param name="mean">The mid point that will equal 1</param>
    /// <returns></returns>
    private float Gauss(float X, float len, float mean)
    {
        float top = -(Mathf.Pow((X - mean), 2f));
        float bot = 2 * (Mathf.Pow(len, 2f));
        float toPower = top / bot;

        float result = Mathf.Pow((float)System.Math.E, toPower);
        return result;
    }

    /// <summary>
    /// Pre setup disribution that will be used for evaluation
    /// </summary>
    /// <param name="X">Point you want to find the membership value for</param>
    /// <returns></returns>
    private float EvaluationDistrobution(float X)
    {
        return Gauss(X, 0.35f, targetGroundPercentage);
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
