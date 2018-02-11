using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using System.IO;

public class RulesTesting : MonoBehaviour {

    const int i = 24 * 10;

    public TextAsset huristicMaps;

    int[] testingMap = new int[i] 
        {
            1,1,1,1,1,1,1,1,1,6,6,6,6,1,1,1,1,1,6,6,6,6,6,6,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        };

    InitPopulation ip = new InitPopulation();
    EvaluateRoom er = new EvaluateRoom(0.75f);
    Crossover co = new Crossover();
    Mutation mu = new Mutation();
    SelectRoom sr = new SelectRoom();

    // TODO: Cleanup this mess
    private void Start()
    {

        Room[] population = ip.Generate(huristicMaps, er.GetGroundPercent(), er);

        float demoSelectFitness = sr.RouletteWheelSelectionOfRoom(population).Fitness;

        Debug.Log(demoSelectFitness);
   
    }

    // TODO: add top scores getter, if it can't reach target amount get as many as possible
    // TODO: if all scores are 0 pick random sample
    // TODO: Then mutate and copy

}
