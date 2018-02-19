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
    GeneratorRules gr = new GeneratorRules();
    

    // TODO: Cleanup this mess
    private void Start()
    {
        Room[] hRooms = new InitHuristicRooms(huristicMaps, er).Rooms;

        for(int i = 0;  i < hRooms.Length; i++ )
        {
            Debug.Log(i + " " + hRooms[i].Fitness);
        }
    }

    private void GenerationTest()
    {
        // init pop and evaluation
        Room[] population = ip.Generate(huristicMaps, er.GetGroundPercent(), er);
        //Room[] population = new InitHuristicRooms(huristicMaps, er).Rooms;

        Room[] parents = sr.SelectParents(population);
        parents.Archive();
        Debug.Log("Parents: " + parents[0].Fitness + " " + parents[1].Fitness);

        Room[] testRooms = co.UniformCrossover(parents[0], parents[1], 50, er);
        testRooms.Archive();
        Debug.Log("Crossover: " + testRooms[0].Fitness + " " + testRooms[1].Fitness);

        testRooms[0] = mu.RandomReseting(testRooms[0], er);
        testRooms[1] = mu.RandomReseting(testRooms[1], er);
        testRooms.Archive();
        Debug.Log("Mutation: " + testRooms[0].Fitness + " " + testRooms[1].Fitness);
    }

}
