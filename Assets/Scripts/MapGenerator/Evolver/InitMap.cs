using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMap {

    CreateRoom cr = new CreateRoom();

    public int[][] Generate()
    {
        int[][] initMap = new int[TileInformation.numRooms][];

        for(int i = 0; i < initMap.Length; i++)
        {
            initMap[i] = cr.Generate();
        }

        Debug.Log(initMap.Length);

        return initMap;
    }
}
