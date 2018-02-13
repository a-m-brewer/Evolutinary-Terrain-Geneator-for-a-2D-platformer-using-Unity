using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTesting : MonoBehaviour {

    public TextAsset huristicMaps;
    EvaluateRoom er = new EvaluateRoom(0.75f);

    // Use this for initialization
    void Start () {
        Room[] population = new InitHuristicRooms(huristicMaps, er).Rooms;

        Grid grid = new Grid(population[13].Data);
        grid.CreateGrid();

        Pathfinding pf = new Pathfinding(grid);
        pf.FindPath(new Vector2(0, 4), new Vector2(23, 4));
        grid.DrawPath();
        grid.WalkableGrid.ArchiveRoom();
    }
	
}
