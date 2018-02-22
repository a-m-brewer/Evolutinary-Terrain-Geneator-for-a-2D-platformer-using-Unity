using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeSortRoom {

    // TODO: Make a merge sort based on fitness score
    public List<Room> MergeSort(List<Room> roomList)
    {
        if(roomList.Count == 1)
        {
            return roomList;
        }
        List<Room> listOne = roomList.Take(roomList.Count / 2).ToList();
        List<Room> listTwo = roomList.Skip(roomList.Count / 2).ToList();

        listOne = MergeSort(listOne);
        listTwo = MergeSort(listTwo);

        return Merge(listOne, listTwo);
    }

    public List<Room> Merge(List<Room> listOne, List<Room> listTwo)
    {
        List<Room> listC = new List<Room>();

        while(listOne.Count > 0 && listTwo.Count > 0)
        {
            if(listOne[0].Fitness > listTwo[0].Fitness)
            {
                listC.Add(listTwo[0]);
                listTwo.Remove(listTwo[0]);
            } else
            {
                listC.Add(listOne[0]);
                listOne.Remove(listOne[0]);
            }
        }

        while(listOne.Count > 0)
        {
            listC.Add(listOne[0]);
            listOne.Remove(listOne[0]);
        }

        while(listTwo.Count > 0)
        {
            listC.Add(listTwo[0]);
            listTwo.Remove(listTwo[0]);
        }

        return listC;
    }
}
