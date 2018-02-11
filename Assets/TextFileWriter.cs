using System;
using System.Globalization;
using System.IO;
using UnityEngine;

public class TextFileWriter {

    string path;
    string filename = "default.txt";
    StreamWriter sr;

    /// <summary>
    /// On creation give a file name mapdata + datetime + a guid
    /// </summary>
    public TextFileWriter()
    {
        this.filename = InitFilename();
        path = Path.GetDirectoryName(Application.dataPath + "/MapArchive/");
    }

    /// <summary>
    /// Open the new file and give the rng file name generated when the object
    /// was created
    /// </summary>
    public void OpenStream()
    {
        string toCreate = path + "/" + filename;
        sr = File.CreateText(toCreate);
        Debug.Log("string " + toCreate);
    }


    /// <summary>
    /// End the file
    /// </summary>
    public void CloseStream()
    {
        sr.Close();
    }

    /// <summary>
    /// Write one line into the open file if the stream is open
    /// </summary>
    /// <param name="line"></param>
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
        string fileName = "mapdata-" + GetDateTime() + "-GUID-" + guid + ".csv";

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

    /// <summary>
    /// Write an array of Room objects Data to a file
    /// </summary>
    /// <param name="rooms">a array of rooms</param>
    public void WriteRoomsToFile(Room[] rooms)
    {
        TextFileWriter tfw = new TextFileWriter();
        tfw.OpenStream();

        for(int room = 0; room < rooms.Length; room++ )
        {
            string roomString = "";
            for(int tile = 0; tile < rooms[room].Data.Length; tile++)
            {
                if((tile % TileInformation.roomSizeX) == 0)
                {
                    roomString += "\n";
                }
                roomString += rooms[room].Data[tile] + ",";
            }
            roomString += "\n";

            tfw.WriteLine(roomString);
        }

        tfw.CloseStream();
    }
}
