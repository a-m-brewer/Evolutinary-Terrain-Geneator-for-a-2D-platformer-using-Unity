using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesTesting : MonoBehaviour {

    const int i = 24 * 10;

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

    GeneratorRules gr = new GeneratorRules();
    EvaluateRoom er = new EvaluateRoom(0.5f);
    CreateRoom cr = new CreateRoom();
    InitPopulation ip = new InitPopulation();

    private void Start()
    {
        int[][] rooms = new int[gr.GetPopulationSize()][];
        float[] scores = new float[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i] = cr.Generate();
            scores[i] = er.Evaluate(rooms[i]);
            
            Debug.Log(i + " " + scores[i]);
        }
       
    }

    // TODO: add top scores getter, if it can't reach target amount get as many as possible
    // TODO: if all scores are 0 pick random sample
    // TODO: Then mutate and copy

    private float EvaluateMap(int[][] map)
    {
        float[] scores = new float[map.Length];
        float output = 1f;
        for(int i = 0; i < map.Length; i++)
        {
            scores[i] = er.Evaluate(map[i]);
            output *= scores[i];
        }
        return output;
    }



}
