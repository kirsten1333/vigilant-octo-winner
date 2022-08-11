using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    public LetMassiveEasy LMI;
    public LineRenderer lineDraw;
    public LineRenderer line;
    public GameController game;
    public NNControler NN;

    [SerializeField]
    private float k = 1;
    [SerializeField]
    private bool pointerIn = false;
    [SerializeField]
    private bool pointerPressed = false;

    void Start()
    {
        lineDraw.startWidth = 5f;
        lineDraw.endWidth = 5f;
        lineDraw.positionCount = 0;
        LMI = GameObject.FindObjectOfType<LetMassiveEasy>();
        game = GameObject.FindObjectOfType<GameController>();
        NN = GameObject.FindObjectOfType<NNControler>();
    }
    void Update()
    {
        if (pointerIn && pointerPressed)
        {
            SetPoint(); //Можно добавить кроутину, чтобы снизить количество иттераций! -----------------
        }
    }
    private Vector2 GetWorldCoordinate(Vector3 mousePosition)
    {
        Vector2 mousePoint = new Vector3(mousePosition.x, mousePosition.y, 1);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    private void SetPoint()
    {
        Vector2 currentPoint = GetWorldCoordinate(Input.mousePosition);
        lineDraw.positionCount++;
        lineDraw.SetPosition(lineDraw.positionCount - 1, currentPoint);
    }

    private void CoordsToRune()
    {
        List<Vector3> posList;
        LineToList(out posList);
        if (posList.Count > 10)
        {
            game.Rune(0);
            return;
        }
        posList = LMI.ToEqualCount(posList);

        posList = GameObject.FindObjectOfType<LetMassiveEasy>().Normilize(posList);

        game.Rune(NN.PredictRune(ListToARR(posList), NN.neuralNetwork));
    }

    private double[] ListToARR(List<Vector3> vectors)
    {
        double[] arr = new double[vectors.Count*2];
        for(int i = 0, j = 1; i < vectors.Count; i++, j++)
        {
            arr[i * 2] = vectors[i].x;
            arr[j * 2 - 1] = vectors[i].y;
        }
        return arr;
    }

    private void SaveCoordInHolder()
    {
        Vector3[] positionsMass;
        List<Vector3> posList;
        LineToList(out positionsMass, out posList);
        if (posList.Count > 10)
        {
            return;
        }
        positionsMass = LMI.ToEqualCount(posList).ToArray();

        for (int i = 0; i < positionsMass.Length; i++)
        {
            DATAHOLDER.coords.Add(positionsMass[i]);
        }
        DATAHOLDER.coordsToList();
    }

    private void LineToList(out Vector3[] positionsMass, out List<Vector3> posList)
    {
        lineDraw.Simplify(k);

        positionsMass = new Vector3[lineDraw.positionCount];
        posList = new();
        lineDraw.GetPositions(positionsMass);

        for (int i = 0; i < positionsMass.Length; i++)
        {
            posList.Add(positionsMass[i]);
        }
    }
    private void LineToList(out List<Vector3> posList)
    {
        lineDraw.Simplify(k);

        var positionsMass = new Vector3[lineDraw.positionCount];
        posList = new();
        lineDraw.GetPositions(positionsMass);

        for (int i = 0; i < positionsMass.Length; i++)
        {
            posList.Add(positionsMass[i]);
        }
    }

    public void PointerEnter()
    {
        pointerIn = true;
    }
    public void PointerDown()
    {
        pointerPressed = true;
    }
    public void PointerExit()
    {
        pointerIn = false;
        if (lineDraw.positionCount > 0)
        {
            //SaveCoordInHolder();
            CoordsToRune();
            lineDraw.positionCount = 0;
        }

    }
    public void PointerUp()
    {
        pointerPressed = false;
        if (lineDraw.positionCount > 0)
        {
            //SaveCoordInHolder();
            CoordsToRune();
            lineDraw.positionCount = 0;
        }
    }
}

