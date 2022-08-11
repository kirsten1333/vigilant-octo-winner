using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{

    public LineRenderer lineDraw;
    
    void Start()
    {
        lineDraw.startWidth = 1f;
        lineDraw.endWidth = 1f;
        lineDraw.positionCount = 0;
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    lineDraw.positionCount = 0;
        //    lineDraw.positionCount = DATAHOLDER.coords.Count;
        //    lineDraw.SetPositions(coordstomass());
        //    DATAHOLDER.coords.Clear();
        //}

    }
    private Vector3[] coordstomass()
    {
        Vector3[] coordsMass = new Vector3[DATAHOLDER.coords.Count];
        DATAHOLDER.coords.CopyTo(coordsMass);
        return coordsMass;
    }
    public void TestLine() //Выводит линию, сохраненную в датахолдере
    {
        lineDraw.positionCount = 0;
        lineDraw.positionCount = DATAHOLDER.coords.Count;
        lineDraw.SetPositions(coordstomass());
        DATAHOLDER.coords.Clear();
    }
}
