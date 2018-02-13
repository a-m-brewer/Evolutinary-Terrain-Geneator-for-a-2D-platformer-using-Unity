using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHuristicRooms {

    private Room[] huristicRooms;
    public Room[] Rooms { get { return this.huristicRooms; } set { huristicRooms = value; } }

    public InitHuristicRooms(TextAsset rooms, EvaluateRoom evaluateRoom)
    {
        string[] loadRooms = LoadRooms(rooms, evaluateRoom);
        this.Rooms = LoadData(loadRooms, evaluateRoom);
    }

    public string[] LoadRooms(TextAsset inFile, EvaluateRoom er)
    {
        string[] wholeLevels = new string[1];

        if (inFile != null)
        {
            wholeLevels = new string[(inFile.text.Split('.')).Length];
            wholeLevels = (inFile.text.Split('.'));
        }
        return wholeLevels; 
    }

    // TODO: Somthing about this is fucked
    public Room[] LoadData(string[] rooms, EvaluateRoom er)
    {

        string[][] levels = new string[rooms.Length][];
        string[][][] final = new string[rooms.Length][][];
        Room[] r = new Room[rooms.Length];
        int[,] toAdd = new int[TileInformation.roomSizeY, TileInformation.roomSizeX]; 

        for (int z = 0; z < levels.Length; z++)
        {

            toAdd = new int[TileInformation.roomSizeY, TileInformation.roomSizeX];

            levels[z] = rooms[z].Split('!');
            final[z] = new string[levels[z].Length][];
            
            for (int y = 0; y < TileInformation.roomSizeY; y++)
            {
                final[z][y] = levels[z][y].Split(',');

                for (int x = 0; x < TileInformation.roomSizeX; x++)
                {
                    //Debug.Log("1: " + final.Length + " 2: " + final[z].Length + " 3: " + final[z][y].Length + " 4: " + final[z][y][x].Length);
                    int.TryParse(final[z][y][x], out toAdd[y, x]);
                }
            }
            r[z] = new Room(toAdd, er);
        }
        return r;

    }

}
