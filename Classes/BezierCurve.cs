using System.Collections.Generic;
using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public class BezierCurve
    {

        [Tooltip("List of the curve control points.")]
        public List<Vector3> lineControlPoints = new List<Vector3>();


        protected List<Vector3> PointsPos = new List<Vector3>();
        protected List<float> DistanceProgress = new List<float>();

        [Tooltip("Number of points that form the curve.")]
        public int curvePointsCount = 10;

        public BezierCurve(List<Vector3> lineControlPoints)
        {
            this.lineControlPoints = lineControlPoints;
        }

        public BezierCurve()
        {
        }
        public BezierCurve(BezierCurve b)
        {
            if (b != null)
            {
                lineControlPoints = new List<Vector3>(b.lineControlPoints);
                CalculatePoints();
                curvePointsCount = b.curvePointsCount;
            }
        }

        public BezierCurve(List<Vector3> lineControlPoints, int curvePointsCount) : this(lineControlPoints)
        {
            this.curvePointsCount = curvePointsCount;
        }

        public List<Vector3> GetPathPoints()
        {
            return CalculatePoints();
        }

        public List<float> GetDistanceProgress()
        {
            if (PointsPos != null ? PointsPos.Count == 0 : true)
                CalculatePoints();
            return DistanceProgress;
        }

        private List<Vector3> CalculatePoints()
        {

            PointsPos = new List<Vector3>();
            if (lineControlPoints != null)
            {
                if (lineControlPoints.Count > 1)
                {
                    PointsPos.Add(lineControlPoints[0]);

                    for (int i = 0; i < curvePointsCount; i++)
                    {

                        float t = (1f / (curvePointsCount + 1f) * (i + 1f));
                        PointsPos.Add(LerpPoints(lineControlPoints, t));
                    }

                    PointsPos.Add(lineControlPoints[lineControlPoints.Count - 1]);
                }
            }
            CalculateDistances();
            return new List<Vector3>(PointsPos);
        }

        protected void CalculateDistances()
        {
            if (PointsPos == null)
                return;

            DistanceProgress = new List<float>();
            DistanceProgress.Add(0);
            float distance = 0;
            for (int i = 1; i < PointsPos.Count; i++)
            {
                distance += UtilityFunctions.Distance(PointsPos[i - 1], PointsPos[i]);
                DistanceProgress.Add(distance);
            }
        }

        private Vector3 LerpPoints(List<Vector3> points, Float01 t)
        {

            if (points.Count > 1)
            {
                List<Vector3> pointsTemp = new List<Vector3>();

                foreach (Vector3 point in points)
                    pointsTemp.Add(point);

                while (pointsTemp.Count > 1)
                {
                    List<Vector3> temp = new List<Vector3>();
                    for (int i = 0; i < pointsTemp.Count - 1; i++)
                    {
                        temp.Add(Vector3.Lerp(pointsTemp[i], pointsTemp[i + 1], t));
                    }
                    pointsTemp.Clear();
                    foreach (Vector3 point in temp)
                        pointsTemp.Add(point);
                }
                return pointsTemp[0];
            }

            return Vector3.zero;
        }

        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    if (lineControlPoints != null)
        //    {
        //        CalculatePoints();

        //        for (int i = 0; i < PointsPos.Count - 1; i++)
        //        {
        //            Gizmos.DrawLine(PointsPos[i], PointsPos[i + 1]);

        //        }
        //        Gizmos.color = Color.blue;
        //        for (int i = 0; i < lineControlPoints.Count; i++)
        //        {
        //            Gizmos.DrawLine(lineControlPoints[i] - Vector3.up, lineControlPoints[i] + Vector3.up);
        //            Gizmos.DrawLine(lineControlPoints[i] - Vector3.left, lineControlPoints[i] + Vector3.left);
        //        }
        //    }
        //}



    }

}