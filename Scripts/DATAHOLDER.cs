using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DATAHOLDER 
{
    public static List<List<Vector3>> ListOfCoords = new List<List<Vector3>>();
    public static List<Vector3> coords = new List<Vector3>();

    public static void coordsToList() //Переводит лист координат сохранённый в Датахолдере в лист листов, для последующего выведения, нормализуя его
    {
        List<Vector3> coordsN = new List<Vector3>(coords);
        GameObject.FindObjectOfType<LineTest>().TestLine();
        coords.Clear();
        if (coordsN.Count != 10)
        {
            return;
        }
        else
        {
            GameObject.FindObjectOfType<LetMassiveEasy>().Normilize(coordsN);
            ListOfCoords.Add(coordsN);
        }
    }


    public static void WriteCoords()
    {
        if (!File.Exists("coords.txt"))
        {
            File.Create("coords.txt");
        }
        StreamWriter sw = new StreamWriter("coords.txt");
        sw.WriteLine(ListOfCoords.Count);
        foreach (List<Vector3> coords in ListOfCoords)
        {
            //sw.Write("{0:D3} ", ListOfCoords.IndexOf(coords));
            foreach (Vector3 vector in coords)
            {
                if(coords.IndexOf(vector)==coords.Count-1)
                {
                    sw.Write("{0:N6} {1:N6}", vector.x, vector.y);
                }
                else
                {
                    sw.Write("{0:N6} {1:N6} ", vector.x, vector.y);
                }
                
            }
            sw.WriteLine();
        }
        sw.Close();
    }

    public static void coordsToLog()
    {
        Debug.Log(coords.Count);
    }

    private static void coordsToLogF()
    {
        for (int i = 0; i < coords.Count; i++)
        {
            Debug.Log(coords[i]);
            Debug.Log(i);
        }
    }
}
