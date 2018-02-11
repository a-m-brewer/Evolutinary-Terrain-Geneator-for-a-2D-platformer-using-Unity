using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class InitRandomPopulation {

    CreateRoom cr = new CreateRoom();
    GeneratorRules gr = new GeneratorRules();

    public Room[] population;

    public InitRandomPopulation(float percentGround, EvaluateRoom evaluateRoom)
    {
        population = Generate(percentGround, evaluateRoom);
    }

    /// <summary>
    /// Method that generates the initial set of rooms to be evolved
    /// </summary>
    /// <returns>the set of rooms</returns>
    public Room[] Generate(float percentGround, EvaluateRoom evaluateRoom)
    {
        Room[] initMap = new Room[gr.GetInitRandomPopulationSize()];

        for (int i = 0; i < initMap.Length; i++)
        {
            initMap[i] = cr.Generate(evaluateRoom);
        }
       
        return initMap;
    }

}
