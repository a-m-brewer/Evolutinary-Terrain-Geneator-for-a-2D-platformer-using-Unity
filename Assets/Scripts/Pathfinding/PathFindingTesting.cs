using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTesting : MonoBehaviour {

    public TextAsset huristicMaps;
    EvaluateRoom er = new EvaluateRoom(0.75f);

    // Use this for initialization
    void Start () {
        Room[] population = new InitHuristicRooms(huristicMaps, er).Rooms;

        TestOne(population, 13);
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
	
    void TestAll(Room[] population)
    {
        for (int i = 0; i < population.Length; i++)
        {
            Grid grid = new Grid(population[i].Data);
            grid.CreateGrid();

            Pathfinding pf = new Pathfinding(grid);

            pf.FindPath(new Vector2(0, FindPosY(grid, 0)), new Vector2(23, FindPosY(grid, 23)));
            grid.DrawPath();
            grid.WalkableGrid.ArchiveRoom();
            Debug.Log(pf.foundpath);
        }
    }

    void TestOne(Room[] population, int roomIndex)
    {
        Grid grid = new Grid(population[roomIndex].Data);
        grid.CreateGrid();

        population[roomIndex].Data.ArchiveRoom();

        Pathfinding pf = new Pathfinding(grid);

        pf.FindPath(new Vector2(0, FindPosY(grid, 0)), new Vector2(23, FindPosY(grid, 23)));
        grid.DrawPath();
        grid.WalkableGrid.ArchiveRoom();
        Debug.Log(pf.foundpath);
    }
}
