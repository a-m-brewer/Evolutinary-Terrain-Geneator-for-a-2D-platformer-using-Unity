using System.IO;
using UnityEngine;

public class TextFileWriter {

    // TODO: Write map data to a text file to see wtf is going on

    string path = System.IO.Path.GetFullPath(".");
    string filename = "default.txt";
    StreamWriter sr;

    public TextFileWriter(string filename)
    {
        filename = filename + ".txt";
    }

    public void OpenStream()
    {
        sr = File.CreateText(path + filename);
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
