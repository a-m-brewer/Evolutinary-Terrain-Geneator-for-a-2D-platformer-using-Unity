using System;
using System.Globalization;
using System.IO;
using UnityEngine;

public class TextFileWriter {

    // TODO: Write map data to a text file to see wtf is going on

    string path;
    string filename = "default.txt";
    StreamWriter sr;

    public TextFileWriter(string fn)
    {
        this.filename = "/" + fn + ".csv";
        path = Path.GetDirectoryName(Application.dataPath + "/MapArchive" + this.filename);
    }

    public void OpenStream()
    {
        sr = File.CreateText(path + filename);
        Debug.Log(path + filename);
    }

    public void CloseStream()
    {
        sr.Close();
    }

    public void WriteLine(string line)
    {
        if(sr == null)
        {
            Debug.Log("File stream not open");
            return;
        }

        sr.WriteLine(line);
    }

    /// <summary>
    /// Creates filename that is used to store old maps for debug purposes
    /// </summary>
    /// <returns>the file name for the data to be stored</returns>
    private string InitFilename()
    {
        string guid = Guid.NewGuid().ToString();
        string fileName = "mapdata-" + GetDateTime() + "-GUID-" + guid;

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

    public void WriteRoomsToFile(int[][] rooms)
    {
        TextFileWriter tfw = new TextFileWriter(InitFilename());
        tfw.OpenStream();

        for(int room = 0; room < rooms.Length; room++ )
        {
            string roomString = "";
            for(int tile = 0; tile < rooms[room].Length; tile++)
            {
                if((tile % TileInformation.roomSizeX) == 0)
                {
                    roomString += "\n";
                }
                roomString += rooms[room][tile] + ",";
            }
            roomString += "\n";

            tfw.WriteLine(roomString);
        }

        tfw.CloseStream();
    }
}
