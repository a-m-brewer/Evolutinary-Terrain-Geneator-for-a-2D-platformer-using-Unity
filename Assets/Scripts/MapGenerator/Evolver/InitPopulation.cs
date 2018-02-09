using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class InitPopulation {

    CreateRoom cr = new CreateRoom();
    GeneratorRules gr = new GeneratorRules();

    /// <summary>
    /// Method that generates the initial set of rooms to be evolved
    /// </summary>
    /// <returns>the set of rooms</returns>
    public int[][] Generate(float percentGround)
    {
        int[][] initMap = new int[gr.GetPopulationSize()][];

        TextFileWriter tfw = new TextFileWriter(InitFilename());
        tfw.OpenStream();

        for (int i = 0; i < initMap.Length; i++)
        {
            initMap[i] = cr.Generate();
            tfw.WriteLine(cr.roomString);
        }

        tfw.CloseStream();
        return initMap;
    }

    /// <summary>
    /// Creates filename that is used to store old maps for debug purposes
    /// </summary>
    /// <returns>the file name for the data to be stored</returns>
    private string InitFilename()
    {
        string fileName = "mapdata-" + GetDateTime();

        string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + " ";

        foreach (char c in invalid)
        {
            fileName = fileName.Replace(c.ToString(), "-");
        }

        return fileName;
    }

    /// <summary>
    /// the date and time in en-GB right now
    /// </summary>
    /// <returns>The time and date</returns>
    private string GetDateTime()
    {
        DateTime dt = DateTime.Now;
        CultureInfo ci = new CultureInfo("en-GB");
        return dt.ToString(ci);
    }
}
