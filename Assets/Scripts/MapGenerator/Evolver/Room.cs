using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room  {
    private float fitness;
    private int[] data;
    private string roomDataString;
    public float Fitness { get { return this.fitness; } set { this.fitness = value; } }
    public int[] Data { get { return this.data; } set { this.data = value; } }
    public string RString { get { return this.roomDataString; } set { this.roomDataString = value; } }


    public Room(int[] dataToHold, string roomString, EvaluateRoom er)
    {
        this.Data = dataToHold;
        this.RString = roomString;
        this.Fitness = er.Evaluate(this);
    }

    public Room(int[] dataToHold, EvaluateRoom er)
    {
        this.Data = dataToHold;
        this.Fitness = er.Evaluate(this);
    }

    
}
