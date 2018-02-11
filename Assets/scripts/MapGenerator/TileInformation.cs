using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  A list of difficulty of tiles that is accessible everywhere
 */
public static class TileInformation {

    public static int roomSizeY = 10;
    public static int roomSizeX = 24;
    public static int numRooms = 4;

    // a list of the difficulty scores of each tileType
    public static int[] difficultyScores = new int[7]
    {
        0,0,0,5,10,20,2
    };

    public static int numTiles = difficultyScores.Length - 1;

}

