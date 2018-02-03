using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPopulation {

    InitMap im = new InitMap();
    GeneratorRules gr = new GeneratorRules();

    public int[][][] Generate()
    {
        int[][][] population = new int[gr.GetPopulationSize()][][];
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = im.Generate();
        }
        return population;
    }
}
