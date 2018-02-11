using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHuristicRooms {

    private Room[] huristicRooms;
    public Room[] Rooms { get { return this.huristicRooms; } set { huristicRooms = value; } }

    public InitHuristicRooms(TextAsset rooms, EvaluateRoom evaluateRoom)
    {
        this.Rooms = LoadMaps(rooms, evaluateRoom);
    }

    public Room[] LoadMaps(TextAsset inFile, EvaluateRoom er)
    {
        string[][] levels = new string[1][];
        int[][] levelsInt = new int[1][];
        Room[] rooms = new Room[1];

        if (inFile != null)
        {
            string[] wholeLevels = (inFile.text.Split('.'));
            levels = new string[wholeLevels.Length][];
            levelsInt = new int[wholeLevels.Length][];
            rooms = new Room[wholeLevels.Length];

            for (int i = 0; i < wholeLevels.Length; i++)
            {
                levels[i] = wholeLevels[i].Split(',');
                levelsInt[i] = new int[levels[i].Length];
                for (int j = 0; j < levels[i].Length; j++)
                {
                    int.TryParse(levels[i][j], out levelsInt[i][j]);
                }

                rooms[i] = new Room(levelsInt[i], er);
            }

        }
        return rooms;
    }


}
