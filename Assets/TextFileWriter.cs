using System.IO;
using UnityEngine;

public class TextFileWriter {

    // TODO: Write map data to a text file to see wtf is going on

    string path;
    string filename = "default.txt";
    StreamWriter sr;

    public TextFileWriter(string fn)
    {
        this.filename = "/" + fn + ".txt";
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

}
