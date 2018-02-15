using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTesting : MonoBehaviour {

    public TextAsset huristicMaps;
    EvaluateRoom er = new EvaluateRoom(0.75f);

    // Use this for initialization
    void Start () {
        Room[] population = new InitHuristicRooms(huristicMaps, er).Rooms;

        //population[poptotest].Data.ArchiveRoom();
        for (int i = 0; i < population.Length; i++)
        {
            Grid grid = new Grid(population[i].Data);
            grid.CreateGrid();

            Pathfinding pf = new Pathfinding(grid);

            pf.FindPath(new Vector2(0, FindPosY(grid, 0)), new Vector2(23, FindPosY(grid, 23)));
            grid.DrawPath();
            //grid.WalkableGrid.ArchiveRoom();
            Debug.Log(pf.foundpath);
        }
    }

    int FindPosY(Grid grid, int x)
    {
        int y = 0;
        while(grid.WalkableGrid[y, x] != 1 && y < TileInformation.roomSizeY)
        {
            y++;
        }
        return y;
    }
	
}
