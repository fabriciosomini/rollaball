using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DrawLine : MultiplayerBehaviour
{
    private LineRenderer line;
    public List<Vector3> pointsList;
    private Vector3 mainGameObjectPos;
    private bool isColliding;

    public bool IsColliding { get { return isColliding; } }

    // Structure for line points
    private struct Line
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
    };

    //    -----------------------------------    
    private void Awake()
    {
        // Create line renderer component and set its property
        line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.positionCount = 0;
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.startColor = Color.green;
        line.endColor = Color.green;
        line.useWorldSpace = true;
       
        pointsList = new List<Vector3>();

    }

    //    -----------------------------------    
    private void Update()
    {
        mainGameObjectPos = gameObject.transform.position;
        mainGameObjectPos.y = 0.5f;
        NetworkServer.Spawn(line.gameObject);
        if (!pointsList.Contains(mainGameObjectPos))
        {
            pointsList.Add(mainGameObjectPos);
            line.positionCount = pointsList.Count;
            line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
            if (isLineCollide())
            {
                line.startColor = Color.red;
                line.endColor = Color.red;
                isColliding = true;
            }
        }
    }

    //    -----------------------------------    
    //  Following method checks is currentLine(line drawn by last two points) collided with line 
    //    -----------------------------------    
    private bool isLineCollide()
    {
        if (pointsList.Count < 2)
        {
            return false;
        }

        int TotalLines = pointsList.Count - 1;
        Line[] lines = new Line[TotalLines];
        if (TotalLines > 1)
        {
            for (int i = 0; i < TotalLines; i++)
            {
                lines[i].StartPoint = pointsList[i];
                lines[i].EndPoint = pointsList[i + 1];
            }
        }
        for (int i = 0; i < TotalLines - 1; i++)
        {
            Line currentLine;
            currentLine.StartPoint = pointsList[pointsList.Count - 2];
            currentLine.EndPoint = pointsList[pointsList.Count - 1];
            if (isLinesIntersect(lines[i], currentLine))
            {
                return true;
            }
        }
        return false;
    }

    //    -----------------------------------    
    //    Following method checks whether given two points are same or not
    //    -----------------------------------    
    private bool checkPoints(Vector3 pointA, Vector3 pointB)
    {
        return (pointA.x == pointB.x && pointA.z == pointB.z);
    }

    //    -----------------------------------    
    //    Following method checks whether given two line intersect or not
    //    -----------------------------------    
    private bool isLinesIntersect(Line L1, Line L2)
    {
        if (checkPoints(L1.StartPoint, L2.StartPoint) ||
            checkPoints(L1.StartPoint, L2.EndPoint) ||
            checkPoints(L1.EndPoint, L2.StartPoint) ||
            checkPoints(L1.EndPoint, L2.EndPoint))
        {
            return false;
        }

        return (Mathf.Max(L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min(L2.StartPoint.x, L2.EndPoint.x)) &&
            (Mathf.Max(L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min(L1.StartPoint.x, L1.EndPoint.x)) &&
            (Mathf.Max(L1.StartPoint.z, L1.EndPoint.z) >= Mathf.Min(L2.StartPoint.z, L2.EndPoint.z)) &&
            (Mathf.Max(L2.StartPoint.z, L2.EndPoint.z) >= Mathf.Min(L1.StartPoint.z, L1.EndPoint.z));
    }
}