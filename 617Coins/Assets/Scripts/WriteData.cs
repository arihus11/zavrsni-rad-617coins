using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteData : MonoBehaviour
{
    private double xCor;
    private double zCor;    
    private bool enableOutput = false;
    void Update(){
        if(!enableOutput){
            enableOutput = true;
            makeOneOutput();
            Invoke("disableOutput",2f);
        }
    }

    public void disableOutput(){
        enableOutput = false;
    }

    public void makeOneOutput(){
        xCor = this.gameObject.transform.localPosition.x;
        zCor = GameObject.Find("Player").gameObject.transform.localPosition.z;
        WriteString(xCor.ToString("F3") + " " + zCor.ToString("F3"));
    }

    public static void WriteString(string toWrite)
   {
       string path = "Assets/Resources/Data.txt";
       //Write some text to the test.txt file
       StreamWriter writer = new StreamWriter(path, true);
       writer.WriteLine(toWrite);
        writer.Close();
       StreamReader reader = new StreamReader(path);
       //Print the text from the file
       Debug.Log(reader.ReadToEnd());
       reader.Close();
    }

     public static void ReadString()
   {
       string path = "Assets/Resources/Data.txt";
       //Read the text from directly from the test.txt file
       StreamReader reader = new StreamReader(path);
       Debug.Log(reader.ReadToEnd());
       reader.Close();
   }
}
