﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room  {
    private float fitness;
    private int[,] data;
    private string roomDataString;
    public float Fitness { get { return this.fitness; } set { this.fitness = value; } }
    public int[,] Data { get { return this.data; } set { this.data = value; } }
    public string RString { get { return this.roomDataString; } set { this.roomDataString = value; } }

    private Vector2 roomSize;
    public Vector2 RoomSize { get { return this.roomSize; } }

    public int[,] walkableGrid;

    public Room(int[,] dataToHold, string roomString)
    {
        this.roomSize = new Vector2(dataToHold.GetLength(1), dataToHold.GetLength(0));
        this.Data = dataToHold;
        this.RString = roomString;
    }

    public Room(int[,] dataToHold)
    {
        this.roomSize = new Vector2(dataToHold.GetLength(1), dataToHold.GetLength(0));
        this.Data = dataToHold;
    }

    
}
