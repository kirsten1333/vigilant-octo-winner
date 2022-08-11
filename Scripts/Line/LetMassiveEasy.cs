using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LetMassiveEasy : MonoBehaviour
{
    //public LineRenderer line;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DATAHOLDER.WriteCoords();
        }
    }
    public List<Vector3> ToEqualCount(List<Vector3> vectors)
    {
        while(vectors.Count != 10)
        {
            double maxmag = 0;
            int indexMaxMag = 0;
            for (int i = 0; i < vectors.Count-1; i++)
            {
                var mag = (vectors[i] - vectors[i + 1]).sqrMagnitude;
                if(mag > maxmag) { maxmag = mag; indexMaxMag = i; };
            }
            Vector3  v = vectors[indexMaxMag] + vectors[indexMaxMag + 1];
            v /= 2;
            vectors.Insert(indexMaxMag+1, v);
        }
        return vectors;
    }
    public List<Vector3> Normilize(List<Vector3> vectors)
    {
        Vector3 xymin;
        Vector3 xymax;
        (xymin, xymax)  = Square(vectors);
        var delta = xymax - xymin;
        for (int i = 0; i < vectors.Count; i++)
        {
            Vector3 vector = vectors[i];
            vector -= xymin;
            vector.x /= delta.x;
            vector.y /= delta.y;
            vectors[i] = vector;
        }


        return vectors;
    }

    public (Vector3,Vector3) Square(List <Vector3> coords) //Рисует квадрат вокруг заклинания
    {
        float xmax = float.MinValue;
        float ymax = float.MinValue;
        float xmin = float.MaxValue;
        float ymin = float.MaxValue;
        float deltax;
        float deltay;
        Vector3 xymax;
        Vector3 xymin;
        xymax.z = 0;
        xymin.z = 0;
        //line.positionCount = 0;
        //line.positionCount = 4;
        //line.loop = true;

        for (int i = 0; i < coords.Count; i++)
        {
            if (coords[i].x > xmax) { xmax = coords[i].x; }
            if (coords[i].y > ymax) { ymax = coords[i].y; }
            if (coords[i].x < xmin) { xmin = coords[i].x; }
            if (coords[i].y < ymin) { ymin = coords[i].y; }
        }

        deltax = xmax - xmin;
        deltay = ymax - ymin;
        xymax.x = xmax;
        xymax.y = ymax;
        if(deltax > deltay) { xymin.x = xymax.x - deltax; xymin.y = xymax.y - deltax;}
        else { xymin.x = xymax.x - deltay; xymin.y = xymax.y - deltay; }
        //line.SetPosition(0, xymin);
        //line.SetPosition(1,  new Vector3 ( xymin.x, xymax.y, 0 ));
        //line.SetPosition(2, xymax);
        //line.SetPosition(3, new Vector3(xymax.x, xymin.y, 0));
        return (xymin, xymax);
    }
}
