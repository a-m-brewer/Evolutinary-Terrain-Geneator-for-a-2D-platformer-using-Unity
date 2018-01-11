using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelector {

    public int CalculateRoomDifficulty(int[] roomData, Transform[] tileTypes)
    {
        int difficulty = 0;

        for (int i = 0; i < roomData.Length; i++)
        {
            Transform cur = tileTypes[roomData[i]];
            if(cur.gameObject.GetComponent<IDifficulty>() != null)
            {
                difficulty += TileInformation.difficultyScores[roomData[i]];
            }
        }
        return difficulty;
    }

    public int[] CalculateDifficultyOfEachRoom(int[][] allRoomsData, Transform[] tileTypes)
    {
        int[] diffScores = new int[allRoomsData.Length];

        for (int i = 0; i < allRoomsData.Length; i++)
        {
            diffScores[i] = CalculateRoomDifficulty(allRoomsData[i], tileTypes);
        }
        return diffScores;
    }

    public IEnumerable<int[]> MyCombinations(IList<int> argList, int argSetSize)
    {
        if (argList == null) throw new ArgumentNullException("argList");
        if (argSetSize <= 0) throw new ArgumentException("argSetSize Must be greater than 0", "argSetSize");
        return MyCombinationsImpl(argList, 0, argSetSize - 1);
    }
    
    private IEnumerable<int[]> MyCombinationsImpl(IList<int> argList, int argStart, int argIteration, List<int> argIndicies = null) {
        argIndicies = argIndicies ?? new List<int>();
        for (int i = argStart; i < argList.Count; i++)
        {
            argIndicies.Add(i);
            if (argIteration > 0)
            {
                foreach (var array in MyCombinationsImpl(argList, i + 1, argIteration - 1, argIndicies))
                {
                    yield return array;
                }
            }
            else
            {
                var array = new int[argIndicies.Count];
                for (int j = 0; j < argIndicies.Count; j++)
                {
                    array[j] = argList[argIndicies[j]];
                }

                yield return array;
            }
            argIndicies.RemoveAt(argIndicies.Count - 1);
        }
    }

    public int CountNumberOfElements(IEnumerable<int[]> ListOfCombinations)
    {
        int count = 0;
        foreach (int[] i in ListOfCombinations)
        {
            count++;
        }
        return count;
    }

    public int[][] GetArrayOfPosiblileMapCombinations(IEnumerable<int[]> combinations, int countOfElementsInCombintation)
    {

        int[][] doubleArr = new int[countOfElementsInCombintation][];

        int iterator = 0;
        foreach (int[] i in combinations)
        {
            doubleArr[iterator] = new int[i.Length];
            doubleArr[iterator] = i;
            iterator++;
        }

        return doubleArr;
    }

    public int[][] GetMapClosestToScore(int target, int[][] allCombinationIndexs, int[][] allRoomData, Transform[] tileTypes)
    {
        System.Random r = new System.Random();

        int[][] clossestMap = GetMapFromCombination(allCombinationIndexs[0],allRoomData);
        int clossestMapScore = DifficultyScoreOfMap(clossestMap, tileTypes);
        
        for (int i = 0; i < allCombinationIndexs.Length - 1; i++)
        {
            int[][] contender = GetMapFromCombination(allCombinationIndexs[i + 1], allRoomData);
            int contenderScore = DifficultyScoreOfMap(contender, tileTypes);

            if (Math.Abs(target - clossestMapScore) > Math.Abs(target - contenderScore))
            {
                // contender map is closer than clossest;
                clossestMap = contender;
                clossestMapScore = contenderScore;
            }
            else if (Math.Abs(target - clossestMapScore) < Math.Abs(target - contenderScore))
            {
                // clossest still rains supreme
                //Debug.Log("closest wins");
            }
            else
            {
                // They are the same score so chance of changing
                if (r.Next(0,2) % 2 == 0)
                {
                    clossestMap = contender;
                    clossestMapScore = contenderScore;
                }
            }
        }
        
        return clossestMap;
    }

    public int[][] GetMapFromCombination(int[] listOfIndexes, int[][] allRoomData)
    {
        int[][] newMap = new int[listOfIndexes.Length][];
        for (int i = 0; i < newMap.Length; i++)
        {
            newMap[i] = allRoomData[listOfIndexes[i]];
        }
        return newMap;
    }

    public int DifficultyScoreOfMap(int[][] mapData, Transform[] tileTypes)
    {
        int[] diffScores = CalculateDifficultyOfEachRoom(mapData, tileTypes);
        int diff = 0;
        for (int j = 0; j < diffScores.Length; j++)
        {
            diff += diffScores[j];
        }
        return diff;
    }


    public int[][] SelectMap(int[][] allRoomData, int numberOfRooms, int targetDifficulty, Transform room)
    {
        int[] indexes = Enumerable.Range(0, allRoomData.Length).ToArray();

        IEnumerable<int[]> possibleRoomCombinations = MyCombinations(indexes, numberOfRooms);

        int possibleRoomCombinationsCount = CountNumberOfElements(possibleRoomCombinations);

        int[][] indexesOfThePossibleCombinations = GetArrayOfPosiblileMapCombinations(possibleRoomCombinations, possibleRoomCombinationsCount);

        int[][] closest = GetMapClosestToScore(targetDifficulty, indexesOfThePossibleCombinations, allRoomData, room.GetComponent<RoomGenerator>().GetRoomTiles());

        return closest;
    }

    
}