using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.EventSystems;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using UnityEngine.SceneManagement;
using UtilityLibrary.Classes;
namespace UtilityLibrary
{
    public static class UtilityFunctions
    {
        #region Math
        public static float ClampMax(float value, float maxValue)
        {

            if (value > maxValue)
                value = maxValue;
            return value;
        }
        public static float ClampMin(float value, float minValue)
        {
            if (value < minValue)
                value = minValue;
            return value;
        }
        public static int ClampMax(int value, int maxValue)
        {
            if (value > maxValue)
                value = maxValue;
            return value;
        }
        public static int ClampMin(int value, int minValue)
        {
            if (value < minValue)
                value = minValue;
            return value;
        }

        public static float ClampAngle(float angle)
        {
            if (angle > 360)
            {

                while (angle > 360)
                    angle -= 360;

            }
            if (angle < 0)
            {

                while (angle < 0)
                    angle += 360;

            }

            return angle;
        }
        public static float ReversePercentage(float finalValue, float percentAdded)
        {
            return (finalValue * 100) / (percentAdded + 100);
        }
        /// <summary>
        /// Returns the angle of the vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float GetAngle(this Vector2 v)
        {
            // normalize the vector: this makes the x and y components numerically
            // equal to the sine and cosine of the angle:
            v.Normalize();
            // get the basic angle:
            var ang = Mathf.Asin(v.y) * Mathf.Rad2Deg;
            // fix the angle for 2nd and 3rd quadrants:
            if (v.x < 0)
            {
                ang = 180 - ang;
            }
            else // fix the angle for 4th quadrant:
            if (v.y < 0)
            {
                ang = 360 + ang;
            }
            return ang;

        }

        /// <summary>
        /// Returns the distance between two angles.
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static float SmallerAngleDifference(float alpha, float beta)
        {

            float phi = Mathf.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            float distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, float angleInDegrees)
        {

            float angleInRadians = Mathf.Abs(angleInDegrees) * (Mathf.PI / 180);

            float cosTheta = Mathf.Cos(angleInRadians);
            float sinTheta = Mathf.Sin(angleInRadians);

            float originX = pointToRotate.x - centerPoint.x;
            float originY = pointToRotate.y - centerPoint.y;

            float newX = 0, newY = 0;
            if (angleInDegrees >= 0)
            {
                newX = originX * cosTheta - originY * sinTheta;
                newY = originX * sinTheta + originY * cosTheta;
            }
            else
            {
                newX = originX * cosTheta + originY * sinTheta;
                newY = -originX * sinTheta + originY * cosTheta;
            }


            return new Vector2(newX + centerPoint.x, newY + centerPoint.y);
        }


        /// <summary>
        /// Calculates the points of a circle.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="diameter"></param>
        /// <param name="positionReference"></param>
        /// <returns></returns>
        public static List<Vector2> CalculateCirclePoints(int points, Vector2? positionReference = null, float diameter = 1f)
        {
            return CalculateEllipsePoints(points, positionReference, diameter, diameter);
        }

        /// <summary>
        /// Returns the closest point in a circle to a given point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="circleCenter"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector2 ShortestPointInCircle(Vector2 point, Vector2 circleCenter, float radius)
        {
            float vx = point.x - circleCenter.x;
            float vy = point.y - circleCenter.y;

            float magV = Mathf.Sqrt(vx * vx + vy * vy);

            float closestPointX = circleCenter.x + vx / magV * radius;
            float closestPointY = circleCenter.y + vy / magV * radius;

            return new Vector2(closestPointX, closestPointY);
        }

        /// <summary>
        /// Calculates the points of a ellipse.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="positionReference"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<Vector2> CalculateEllipsePoints(int points, Vector2? positionReference = null, float width = 1f, float height = 1f)
        {
            float x;
            float y;
            points = ClampMin(points, 3);
            float angle = 0;
            float angleStep = (360f / points);

            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < (points); i++)
            {
                y = Mathf.Sin(Mathf.Deg2Rad * angle) * height / 2f;
                x = Mathf.Cos(Mathf.Deg2Rad * angle) * width / 2f;

                var pos = new Vector2(x, y);

                if (positionReference != null)
                    pos += positionReference ?? Vector2.zero;

                positions.Add(pos);

                angle += angleStep;
            }

            return positions;
        }

        /// <summary>
        /// Checks if a point is whitin a circle.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="circleCenter"></param>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public static bool PointWhitinCircle(Vector2 point, Vector2 circleCenter, float diameter = 1f)
        {
            return PointWhitinEllipse(point, circleCenter, diameter, diameter);
        }
        /// <summary>
        /// Checks if a point is whitin a ellipse.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="ellipseCenter"></param>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public static bool PointWhitinEllipse(Vector2 point, Vector2 ellipseCenter, float width = 1f, float height = 1f)
        {
            return PointWhitinEllipse(point, ellipseCenter, width, height, 0f);
        }
        /// <summary>
        /// Checks if a point is whitin a ellipse.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="ellipseCenter"></param>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public static bool PointWhitinEllipse(Vector2 point, Vector2 ellipseCenter, float width = 1f, float height = 1f, float angle = 0f)
        {
            angle = ClampAngle(angle) * Mathf.Deg2Rad;

            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);

            var dx = (point.x - ellipseCenter.x);
            var dy = (point.y - ellipseCenter.y);

            var rx = (width / 2f);
            var ry = (height / 2f);

            var tdx = cos * dx + sin * dy;
            var tdy = sin * dx - cos * dy;

            return (tdx * tdx) / (rx * rx) + (tdy * tdy) / (ry * ry) < 1;

        }

        public static bool PointWhitinEllipse(Vector2 point, Ellipse e)
        {
            return PointWhitinEllipse(point, e.Center, e.Width, e.Height, e.Rotation);
        }

        private static float isLeft(Vector2 P0, Vector2 P1, Vector2 P2)
        {
            return ((P1.x - P0.x) * (P2.y - P0.y) - (P2.x - P0.x) * (P1.y - P0.y));
        }
        public static bool PointInRectangle(Vector2 X, Vector2 Y, Vector2 Z, Vector2 W, Vector2 P)
        {
            return (isLeft(X, Y, P) > 0 && isLeft(Y, Z, P) > 0 && isLeft(Z, W, P) > 0 && isLeft(W, X, P) > 0);
        }

        public static bool PointInRectangle(Rectangle R, Vector2 P)
        {
            return PointInRectangle(R.TopLeft, R.TopRight, R.BottomRight, R.BottomLeft, P);
        }

        public static bool CircleIntersectsTriangle(Vector2 circleCenter, float radius, Triangle t)
        {

            //TEST 1: Vertex within circle

            float c1x = circleCenter.x - t.V1.x;
            float c1y = circleCenter.y - t.V1.y;

            float radiusSqr = radius * radius;
            float c1sqr = c1x * c1x + c1y * c1y - radiusSqr;

            if (c1sqr <= 0)
                return true;

            float c2x = circleCenter.x - t.V2.x;
            float c2y = circleCenter.y - t.V2.y;

            float c2sqr = c2x * c2x + c2y * c2y - radiusSqr;

            if (c2sqr <= 0)
                return true;

            float c3x = circleCenter.x - t.V3.x;
            float c3y = circleCenter.y - t.V3.y;

            float c3sqr = radiusSqr;
            c3sqr = c3x * c3x + c3y * c3y - radiusSqr;

            if (c3sqr <= 0)
                return true;

            //TEST 2: Circle centre within triangle

            float e1x = t.V2.x - t.V1.x;
            float e1y = t.V2.y - t.V1.y;

            float e2x = t.V3.x - t.V2.x;
            float e2y = t.V3.y - t.V2.y;

            float e3x = t.V1.x - t.V3.x;
            float e3y = t.V1.y - t.V3.y;

            if (e1y * c1x >= e1x * c1y &&
                e2y * c2x >= e2x * c2y &&
                e3y * c3x >= e3x * c3y)
                return true;

            //TEST 3: Circle intersects edge

            var k = c1x * e1x + c1y * e1y;
            float len;
            if (k > 0)
            {
                len = e1x * e1x + e1y * e1y;

                if (k < len)
                {
                    if (c1sqr * len <= k * k)
                        return true;
                }
            }

            // Second edge
            k = c2x * e2x + c2y * e2y;

            if (k > 0)
            {
                len = e2x * e2x + e2y * e2y;

                if (k < len)
                {
                    if (c2sqr * len <= k * k)
                        return true;
                }
            }

            // Third edge
            k = c3x * e3x + c3y * e3y;

            if (k > 0)
            {
                len = e3x * e3x + e3y * e3y;

                if (k < len)
                {
                    if (c3sqr * len <= k * k)
                        return true;
                }
            }

            return false;
        }

        public static bool EllipseIntersectsRectangle(Ellipse E, Rectangle R)
        {
            //Too far apart
            if (Vector2.Distance(E.Center, R.Center) > (Vector2.Distance(R.Center, R.TopLeft) + Mathf.Max(E.Width, E.Height) / 2f))
                return false;

            //Ellipse center whitin rectangle
            if (PointInRectangle(R, E.Center))
                return true;

            List<Vector2> l = new List<Vector2> { R.TopLeft, R.TopRight, R.BottomRight, R.BottomLeft };


            for (int i = 0; i < l.Count; i++)
            {

                l[i] -= E.Center;
                l[i] = RotatePoint(l[i], Vector2.zero, -E.Rotation);
                l[i] = new Vector2(l[i].x * (1f / E.Width),
                                   (l[i].y) * (1f / E.Height));
                l[i] = RotatePoint(l[i], Vector2.zero, E.Rotation);
                l[i] += E.Center;
            }

            if (CircleIntersectsTriangle(E.Center, 0.5f, new Triangle(l[0], l[1], l[2])))
                return true;

            if (CircleIntersectsTriangle(E.Center, 0.5f, new Triangle(l[2], l[3], l[0])))
                return true;

            return false;
        }

        public static bool CircleIntersectsCircle(Vector2 center1, float r1, Vector2 center2, float r2)
        {
            var d1 = (center2.x - center1.x);
            var d2 = (center2.y - center1.y);
            var rr = (r1 + r2);
            return (d1 * d1) + (d2 * d2) <= rr * rr;
        }

        public static bool EllipseIntersectsCircle(Ellipse e, Vector2 circleCenter, float circleRadius)
        {
            var test = new EllipseCollisionTest(10);
            return test.EllipseCircleCollide(e, circleCenter, circleRadius);
        }
        public static bool EllipseIntersectsEllipse(Ellipse e1, Ellipse e2)
        {
            var test = new EllipseCollisionTest(10);
            return test.EllipseEllipseCollide(e1, e2);
        }

        /// <summary>
        /// Returns the factorial of the given number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Factorial(this int number)
        {
            int result = 1;
            while (number != 1)
            {
                result = result * number;
                number = number - 1;
            }
            return result;
        }

        public static float ChangeToZero(this float f, float speed = 30f)
        {
            if (Mathf.Approximately(f, 0f))
                return 0;
            float changeValue = ClampMin(speed, 0f) * Time.deltaTime;
            if (f > 0)
            {

                if (f - changeValue < 0f)
                    return 0f;
                return f - changeValue;
            }
            else
            {

                if (f + changeValue > 0f)
                    return 0f;
                return f + changeValue;
            }
        }

        /// <summary>
        /// Returns the sign of the number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Sign(int value)
        {
            if (value >= 0)
                return 1;
            else
                return -1;
        }

        public static int SignZero(int value)
        {
            if (value > 0)
                return 1;
            else if (value < 0)
                return -1;
            else
                return 0;
        }

        /// <summary>
        /// Returns -1 or 1 at random.
        /// </summary>
        /// <returns></returns>
        public static float RandomSign()
        {
            return UnityEngine.Random.Range(0, 2) * 2 - 1;
        }

        public static float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

        private delegate float RoundingFunction(float value);

        private enum RoundingDirection { Up, Down }


        public static float RoundUp(float value, int precision)
        {
            return Round(value, precision, RoundingDirection.Up);
        }

        public static float RoundDown(float value, int precision)
        {
            return Round(value, precision, RoundingDirection.Down);
        }

        private static float Round(float value, int precision,
                    RoundingDirection roundingDirection)
        {
            RoundingFunction roundingFunction;
            if (roundingDirection == RoundingDirection.Up)
                roundingFunction = Mathf.Ceil;
            else
                roundingFunction = Mathf.Floor;
            value *= Mathf.Pow(10, precision);
            value = roundingFunction(value);
            return value * Mathf.Pow(10, -1 * precision);
        }



        #endregion

        #region IEnumerables
        /// <summary>
        /// Returns a random element from a IEnumerable.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        /// <summary>
        /// Returns a IEnumerable with random elements from the source IEnumerable.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="source"></param>
        /// <param name="count">The amount of elements to pick.</param>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }


        #endregion

        #region Lists
        /// <summary>
        /// Converts an array to a list.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="array">The array to convert to a list.</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this T[] array)
        {
            if (array == null)
                return null;

            List<T> result = new List<T>();
            foreach (T t in array)
            {
                result.Add(t);
            }
            return result;
        }

        public static List<T> ToNonNullList<T>(this IEnumerable<T> obj)
        {
            return obj == null ? new List<T>() : obj.ToList();
        }
        /// <summary>
        /// Prints all elements on the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l"></param>
        public static void PrintAll<T>(this List<T> l)
        {
            if (l != null)
                foreach (var item in l)
                {
                    Debug.Log(item);
                }
        }

        /// <summary>
        /// Fills a list with the contents of another list, without increasing or decreasing the size.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="l">The list to fill.</param>
        /// <param name="other">The list that will be used to fill.</param>
        /// <returns></returns>
        public static List<T> FillList<T>(this List<T> l, List<T> other)
        {
            if (l == null || other == null)
                return l;
            for (int i = 0; i < l.Count; i++)
            {
                if (other.Count - 1 < i)
                    break;
                l[i] = other[i];
            }

            return l;
        }
        /// <summary>
        /// Prevents adding null items to the list.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddNotNull<T>(this List<T> l, T item)
        {
            if (item != null && l != null)
                l.Add(item);

        }
        public static void TryAddRange<T>(this List<T> l, List<T> other)
        {
            if (other != null)
                l.AddRange(other);
        }

        /// <summary>
        /// Returns the item after the given index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l"></param>
        /// <param name="currentItemIndex"></param>
        /// <returns></returns>
        public static T NextItem<T>(this List<T> l, int currentItemIndex)
        {
            if (l == null)
                return default(T);

            if (l.ValidIndex(currentItemIndex + 1))
                return l[currentItemIndex + 1];

            return default(T);
        }

        /// <summary>
        /// Returns the item before the given index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l"></param>
        /// <param name="currentItemIndex"></param>
        /// <returns></returns>
        public static T PreviousItem<T>(this List<T> l, int currentItemIndex)
        {
            if (l == null)
                return default(T);

            if (l.ValidIndex(currentItemIndex - 1))
                return l[currentItemIndex - 1];

            return default(T);
        }


        /// <summary>
        /// Is the list empty?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list?.Count == 0;
        }

        public static bool ValidIndex<T>(this List<T> list, int index)
        {
            return (index >= 0 && index < list.Count);
        }

        public static int NextIndex<T>(this List<T> list, int currentIndex)
        {

            if (list.ValidIndex(currentIndex + 1))
                return (++currentIndex);
            else
                return 0;
        }

        #endregion

        #region Dictionary
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }
        #endregion

        #region Stack

        /// <summary>
        /// Checks if the stack is empty before calling the Peek() method. (Prevents "InvalidOperationException")
        /// </summary>
        public static T SafePeek<T>(this Stack<T> t)
        {
            if (t != null && t.Count > 0)
                return t.Peek();
            return default(T);
        }

        #endregion

        #region Arrays
        /// <summary>
        /// Prints all elements on the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void PrintAll<T>(this T[] array)
        {
            if (array != null)
                foreach (var item in array)
                {
                    Debug.Log(item);
                }
        }
        #endregion

        #region 2D Arrays
        /// <summary>
        /// Returns a list with all the items of the two-dimensional array.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="twoDimensionalArray"></param>
        /// <returns></returns>
        public static List<T> GetItems<T>(this T[,] twoDimensionalArray)
        {
            if (twoDimensionalArray == null)
                return null;
            List<T> result = new List<T>();

            for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
            {
                for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
                {
                    result.Add(twoDimensionalArray[i, j]);
                }
            }
            return result;
        }



        /// <summary>
        /// Checks if the (x,y) coordinates is valid in the two-dimensional array.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="twoDimensionalArray"></param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        public static bool ValidCoordinates<T>(this T[,] twoDimensionalArray, int x, int y)
        {
            if (twoDimensionalArray == null)
                return false;

            return (x >= 0 && x < twoDimensionalArray.GetLength(0) &&
                    y >= 0 && y < twoDimensionalArray.GetLength(1));
        }
        /// <summary>
        /// Checks if the (x,y) coordinates is valid in the two-dimensional array.
        /// </summary>
        /// <typeparam name="T">The object Type.</typeparam>
        /// <param name="twoDimensionalArray"></param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        public static bool ValidCoordinates<T>(this T[,] twoDimensionalArray, Position pos)
        {
            return twoDimensionalArray.ValidCoordinates(pos.x, pos.y);
        }

        /// <summary>
        /// Rotates a two-dimensional array by +90.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T[,] RotateMatrix90R<T>(this T[,] matrix)
        {
            T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    ret[i, j] = matrix[matrix.GetLength(0) - j - 1, i];
                }
            }

            return ret;
        }

        /// <summary>
        /// Rotates a two-dimensional array by -90.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T[,] RotateMatrix90L<T>(this T[,] matrix)
        {

            T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    ret[i, j] = matrix[j, matrix.GetLength(1) - i - 1];
                }
            }

            return ret;
        }
        /// <summary>
        /// Rotates a two-dimensional array by 180.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T[,] Reverse<T>(this T[,] matrix)
        {

            return matrix.RotateMatrix90R().RotateMatrix90R();
        }
        public static bool Check2DArray<T>(this T[,] data, T[,] find) where T : class
        {
            if (data == null || find == null)
                return false;
            int dataLen = data.Length; // length of the whole data
            int findLen = find.Length; // length of the whole find

            for (int i = 0; i < dataLen; i++) // iterate through data
            {
                int dataX = i % data.GetLength(0); // get current column index
                int dataY = i / data.GetLength(0); // get current row index

                bool okay = true; // declare result placeholder for that check
                for (int j = 0; j < findLen && okay; j++) // iterate through find
                {
                    int findX = j % find.GetLength(1); // current column in find
                    int findY = j / find.GetLength(1); // current row in find

                    int checkedX = findX + dataX; // column index in data to check
                    int checkedY = findY + dataY; // row index in data to check

                    // check if checked index is not going outside of the data boundries 
                    if (checkedX >= data.GetLength(0) || checkedY >= data.GetLength(1))
                    {
                        // we are outside of the data boundries
                        // set flag to false and break checks for this data row and column
                        okay = false;
                        break;
                    }

                    // we are still inside of the data boundries so check if values matches
                    okay = data[dataY + findY, dataX + findX] == find[findY, findX]; // check if it matches
                }
                if (okay) // if all values from both fragments are equal
                    return true; // return true

            }
            return false;
        }
        #endregion

        #region Vectors

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return Mathf.Sqrt((v2.x - v1.x) * (v2.x - v1.x) +
                                 (v2.y - v1.y) * (v2.y - v1.y) +
                                 (v2.z - v1.z) * (v2.z - v1.z));
        }

        public static Vector3 RoundVector3(Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }

        public static Vector2 RoundVector2(Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }

        /// <summary>
        /// Returns the midpoint between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 MidPoint(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + (b.x - a.x) / 2f,
                               a.y + (b.y - a.y) / 2f,
                               a.z + (b.z - a.z) / 2f);
        }

        public static Vector3 ClampToBounds(this Vector3 v, Bounds b)
        {
            Vector3 pos = new Vector3(v.x, v.y, v.z);

            pos.x = Mathf.Clamp(pos.x, b.min.x, b.max.x);

            pos.y = Mathf.Clamp(pos.y, b.min.y, b.max.y);

            pos.z = Mathf.Clamp(pos.z, b.min.z, b.max.z);

            return pos;
        }

        public static Vector2Int Normalized(this Vector2Int v)
        {
            return new Vector2Int(Sign(v.x), Sign(v.y));
        }
        public static Vector2Int NormalizedZero(this Vector2Int v)
        {
            return new Vector2Int(SignZero(v.x), SignZero(v.y));
        }

        public static Vector2 GetAngleDirection(this float Angle, bool radAngle = false)
        {
            if (!radAngle)
                Angle = Angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
        }
        #endregion

        #region GameObject
        /// <summary>
        /// Tries to get a component in GameObject, adds new one if not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject g) where T : Component
        {
            if (g.GetComponent<T>() == null)
                return g.AddComponent<T>();
            else
                return g.GetComponent<T>();
        }

        /// <summary>
        /// Adds a component to a GameObject if it doesn't have the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static T AddComponentOnce<T>(this GameObject g) where T : Component
        {
            var c = g.GetComponent<T>();
            if (c == null)
                return g.AddComponent<T>();

            return c;
        }

        /// <summary>
        /// Replaces or adds a component to a GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static T ReplaceOrAddComponent<T>(this GameObject g) where T : Component
        {
            var c = g.GetComponents<T>();
            for (int i = 0; i < c.Length; i++)
            {
                UnityEngine.Object.Destroy(c[i]);
            }

            return g.AddComponent<T>();
        }
        /// <summary>
        /// Removes a component attached to a GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static void RemoveComponent<T>(this GameObject g) where T : Component
        {
            var c = g.GetComponents<T>();

            for (int i = 0; i < c.Length; i++)
            {
                UnityEngine.Object.Destroy(c[i]);
            }


        }


        /// <summary>
        /// Checks if a component is present on GameObject.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this GameObject g) where T : Component
        {
            if (!g)
                return false;

            bool hasComponent = false;
            foreach (Component comp in g.GetComponents<Component>())
            {

                if (comp is T)
                {
                    hasComponent = true;
                    break;
                }
            }

            return hasComponent;
        }

        public static void ResetTransformLocal(this GameObject g)
        {
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = Vector3.zero;
            g.transform.localEulerAngles = Vector3.zero;
        }
        public static void ResetTransform(this GameObject g)
        {
            g.transform.localScale = Vector3.one;
            g.transform.position = Vector3.zero;
            g.transform.eulerAngles = Vector3.zero;
        }

        /// <summary>
        /// Sets the GameObject's scale.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="size"></param>
        public static void SetScale(this GameObject g, float size)
        {
            g.transform.localScale = new Vector3(size, size, size);
        }

        public static void DestroySelf(this UnityEngine.Object c)
        {
            UnityEngine.Object.Destroy(c);
        }

        /// <summary>
        /// Deactivates the GameObject.
        /// </summary>
        /// <param name="g"></param>
        public static void Deactivate(this GameObject g)
        {
            if (!g)
                return;
            g.SetActive(false);
        }

        /// <summary>
        /// Activates the GameObject.
        /// </summary>
        /// <param name="g"></param>
        public static void Activate(this GameObject g)
        {
            if (!g)
                return;
            g.SetActive(true);
        }
        /// <summary>
        /// Use this method to get all loaded objects of some type, including inactive objects. 
        /// </summary>
        public static List<T> FindObjectsOfTypeAll<T>()
        {
            List<T> results = new List<T>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.isLoaded)
                {
                    var allGameObjects = s.GetRootGameObjects();
                    for (int j = 0; j < allGameObjects.Length; j++)
                    {
                        var go = allGameObjects[j];
                        results.AddRange(go.GetComponentsInChildren<T>(true));
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Destroys all child objects on transform.
        /// </summary>
        /// <param name="parent"></param>
        public static void DestroyChildren(this Transform parent, bool destroyInInspector = false)
        {
            List<GameObject> destroyList = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                destroyList.Add(child.gameObject);
            }

            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                if (Application.isEditor && destroyInInspector)
                    UnityEngine.Object.DestroyImmediate(destroyList[i]);
                else
                    UnityEngine.Object.Destroy(destroyList[i]);
            }

        }
        /// <summary>
        /// Destroys all child objects on transform that have the component.
        /// </summary>
        /// <param name="parent"></param>
        public static void DestroyChildren<T>(this Transform parent, bool destroyInInspector = false) where T : Component
        {
            List<GameObject> destroyList = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.HasComponent<T>())
                    destroyList.Add(child.gameObject);
            }

            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                if (Application.isEditor && destroyInInspector)
                    UnityEngine.Object.DestroyImmediate(destroyList[i]);
                else
                    UnityEngine.Object.Destroy(destroyList[i]);
            }

        }

        public static void SafeDestroy(GameObject g)
        {
            if (!g)
                return;
            if (Application.isEditor)
                UnityEngine.Object.DestroyImmediate(g);
            else
                UnityEngine.Object.Destroy(g);
        }
        /// <summary>
        /// Changes the GameObject's scale without affecting the children.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scale"></param>
        public static void ChangeParentScale(this Transform parent, Vector3 scale)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in parent)
            {
                child.parent = null;
                children.Add(child);
            }
            parent.localScale = scale;
            foreach (Transform child in children) child.parent = parent;
        }
        #endregion

        #region Audio
        /// <summary>
        /// Adds an AudioSource with the clip to the GameObject.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="clip">The clip to add.</param>
        /// <param name="loop">Loop the clip?</param>
        /// <param name="playAwake">Play clip on awake?</param>
        /// <param name="vol">Clip's volume.</param>
        /// <returns></returns>
        public static AudioSource AddAudio(this GameObject gameObject, AudioClip clip, bool loop = false, bool playAwake = false, float vol = 1f)
        {
            if (clip == null)
                return null;
            // Check if clip already exists in object.
            foreach (AudioSource audio in gameObject.GetComponents<AudioSource>())
            {
                if (audio.clip == clip)
                {
                    // Update values.
                    audio.loop = loop;
                    audio.playOnAwake = playAwake;
                    audio.volume = vol;
                    return audio;
                }
            }
            var newAudio = gameObject.AddComponent<AudioSource>();
            newAudio.clip = clip;
            newAudio.loop = loop;
            newAudio.playOnAwake = playAwake;
            newAudio.volume = vol;
            return newAudio;
        }

        /// <summary>
        /// Plays a clip from an AudioSource in the GameObject. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="clip">The clip to play.</param>
        /// <param name="addIfNotFound">Add and play the clip if not found?.</param>
        public static void PlayAudio(this GameObject gameObject, AudioClip clip, bool addIfNotFound = true)
        {
            if (clip == null)
                return;

            // Check if clip already exists in object.
            foreach (AudioSource audio in gameObject.GetComponents<AudioSource>())
            {
                if (audio.clip == clip)
                {
                    audio.Play();
                    return;
                }
            }
            if (addIfNotFound)
            {
                // Add and play if not found.
                gameObject.AddAudio(clip);
                gameObject.PlayAudio(clip);
            }
        }

        public static void Mute(this GameObject g)
        {
            foreach (AudioSource s in g.GetComponents<AudioSource>())
            {
                s.mute = true;
            }
        }

        public static bool IsMute(this GameObject g)
        {
            bool mute = true;
            foreach (AudioSource s in g?.GetComponents<AudioSource>())
            {
                if (!s.mute)
                    mute = false;
            }
            return mute;
        }

        public static void ToggleMute(this GameObject g)
        {
            foreach (AudioSource s in g.GetComponents<AudioSource>())
            {
                s.mute = !s.mute;
            }
        }

        public static void Unmute(this GameObject g)
        {
            foreach (AudioSource s in g.GetComponents<AudioSource>())
            {
                s.mute = false;
            }
        }

        #endregion

        #region String

        public static string RemoveLineEndings(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty)
                        .Replace("\u200B", "");
        }


        public static bool ContainsIgnoreCase(this List<string> l, string s)
        {
            if (l == null)
                return false;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].ToLower().Trim().RemoveZeroWidthSpace() == s.ToLower().Trim().RemoveZeroWidthSpace())
                    return true;
            }

            return false;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string RemoveZeroWidthSpace(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Replace("\u200B", "");
        }
        #endregion

        #region Events and UI

        /// <summary>
        /// The width of the screen.
        /// </summary>
        public static float ScreenWidth
        {
            get
            {
#if UNITY_EDITOR

            var screenSize = Handles.GetMainGameViewSize();
            return ScreenHeight * screenSize.x / screenSize.y;

#else
                return ScreenHeight * Screen.width / Screen.height;
#endif
            }
        }
        /// <summary>
        /// The height of the screen.
        /// </summary>
        public static float ScreenHeight
        {
            get
            {
                return Camera.main.orthographicSize * 2.0f;
            }
        }

        /// <summary>
        /// Removes and adds a listener.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="listener"></param>
        public static void RemoveAndAddListener(this UnityEvent y, UnityAction listener)
        {
            y.RemoveListener(listener);
            y.AddListener(listener);
        }
        /// <summary>
        /// Removes and adds a listener.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="listener"></param>
        public static void RemoveAndAddListener<T>(this UnityEvent<T> y, UnityAction<T> listener)
        {
            y.RemoveListener(listener);
            y.AddListener(listener);
        }
        /// <summary>
        /// Removes and adds a listener.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="listener"></param>
        public static void RemoveAndAddListener<T1, T2>(this UnityEvent<T1, T2> y, UnityAction<T1, T2> listener)
        {
            y.RemoveListener(listener);
            y.AddListener(listener);
        }/// <summary>
         /// Removes and adds a listener.
         /// </summary>
         /// <param name="y"></param>
         /// <param name="listener"></param>
        public static void RemoveAndAddListener<T1, T2, T3>(this UnityEvent<T1, T2, T3> y, UnityAction<T1, T2, T3> listener)
        {
            y.RemoveListener(listener);
            y.AddListener(listener);
        }
        // /// <summary>
        // /// Checks if the pointer was over a UI element.
        // /// </summary>
        // /// <returns></returns>
        // public static bool IsPointerOverUIObject()
        // {
        //     PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //     eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //     List<RaycastResult> results = new List<RaycastResult>();
        //     EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        //     return results.Count > 0;
        // }
        #endregion

        #region Extras
        /// <summary>
        /// Draws bounds in a DrawGizmos function.
        /// </summary>
        /// <param name="b"></param>
        public static void GizmosDrawBounds(Bounds b, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;
            Gizmos.DrawWireCube(b.center, b.size);
        }

        /// <summary>
        /// Draws a Quadrilateral in a DrawGizmos function.
        /// </summary>
        public static void GizmosDrawQuadrilateral(Vector2 V1, Vector2 V2, Vector2 V3, Vector2 V4, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;

            Gizmos.DrawLine(V1, V2);
            Gizmos.DrawLine(V2, V3);
            Gizmos.DrawLine(V3, V4);
            Gizmos.DrawLine(V4, V1);
        }
        /// <summary>
        /// Draws a Square in a DrawGizmos function.
        /// </summary>
        public static void GizmosDrawSquare(Vector2 center, float scale, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;

            GizmosDrawQuadrilateral(
                new Vector2(center.x - scale / 2f, center.y - scale / 2f),
                new Vector2(center.x - scale / 2f, center.y + scale / 2f),
                new Vector2(center.x + scale / 2f, center.y + scale / 2f),
                new Vector2(center.x + scale / 2f, center.y - scale / 2f)
            );
        }
        /// <summary>
        /// Draws a Rectangle in a DrawGizmos function.
        /// </summary>
        /// <param name="r"></param>
        public static void GizmosDrawRectangle(Rectangle r, Color? gizmosColor = null)
        {
            GizmosDrawQuadrilateral(r.TopLeft, r.TopRight, r.BottomRight, r.BottomLeft, gizmosColor);
        }

        public const int DefaultLayerMask = ~0;


        /// <summary>
        /// Draws a circle in a DrawGizmos function.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="diameter"></param>
        /// <param name="points"></param>
        /// <param name="gizmosColor"></param>
        public static void GizmosDrawCircle(Vector2 pos, float diameter = 1f, int points = 30, Color? gizmosColor = null)
        {
            GizmosDrawEllipse(pos, diameter, diameter, points, gizmosColor);
        }

        /// <summary>
        /// Draws an ellipse in a DrawGizmos function.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="points"></param>
        /// <param name="gizmosColor"></param>
        public static void GizmosDrawEllipse(Vector2 pos, float width, float height, float rotatedAngle, int points = 30, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;
            var circlePoints = CalculateEllipsePoints(points, pos, width, height);

            circlePoints[0] = RotatePoint(circlePoints[0], pos, rotatedAngle);

            var current = circlePoints[0];

            rotatedAngle = ClampAngle(rotatedAngle);

            for (int i = 1; i < circlePoints.Count; i++)
            {
                circlePoints[i] = RotatePoint(circlePoints[i], pos, rotatedAngle);
                Gizmos.DrawLine(current, circlePoints[i]);
                current = circlePoints[i];
            }

            Gizmos.DrawLine(circlePoints[circlePoints.Count - 1], circlePoints[0]);
        }
        /// <summary>
        /// Draws an ellipse in a DrawGizmos function.
        /// </summary>
        public static void GizmosDrawEllipse(Ellipse e, int points = 30, Color? gizmosColor = null)
        {
            GizmosDrawEllipse(e.Center, e.Width, e.Height, e.Rotation, points, gizmosColor);
        }
        /// <summary>
        /// Draws an ellipse in a DrawGizmos function.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="points"></param>
        /// <param name="gizmosColor"></param>
        public static void GizmosDrawEllipse(Vector2 pos, float width, float height, int points = 30, Color? gizmosColor = null)
        {
            GizmosDrawEllipse(pos, width, height, 0f, points, gizmosColor);
        }

        /// <summary>
        /// Draws an arc.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="arcPoints"></param>
        /// <param name="gizmosColor"></param>
        public static void GizmosDrawAngleArc(Vector2 center, float radius, Angle start, Angle end, int arcPoints = 10, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;
            arcPoints = ClampMin(arcPoints, 2);
            var angle = start;
            var arcLength = new Angle(end.Value - start.Value).Value;
            Vector2 point;
            List<Vector2> arcPointsPos = new List<Vector2>();
            for (int i = 0; i < arcPoints; i++)
            {
                point = GetAngleDirection(angle.Value) * radius;

                arcPointsPos.Add(point + center);
                angle.Value += arcLength / arcPoints;
            }
            point = GetAngleDirection(angle.Value) * radius;
            arcPointsPos.Add(point + center);
            Gizmos.DrawLine(center, arcPointsPos[0]);
            Gizmos.DrawLine(center, arcPointsPos[arcPointsPos.Count - 1]);

            for (int i = 0; i < arcPointsPos.Count - 1; i++)
                Gizmos.DrawLine(arcPointsPos[i], arcPointsPos[i + 1]);

        }

        /// <summary>
        /// Draws a cross in a DrawGizmos function.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="gizmosColor"></param>
        public static void GizmosDrawCross(Vector2 pos, float size = 1f, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;
            Gizmos.DrawLine(new Vector3(pos.x, pos.y - size / 2f, 0f), new Vector3(pos.x, pos.y + size / 2f));
            Gizmos.DrawLine(new Vector3(pos.x - size / 2f, pos.y, 0f), new Vector3(pos.x + size / 2f, pos.y));
        }

        /// <summary>
        /// Draws an arrow from one point to the other.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="headArc"></param>
        /// <param name="headLength"></param>
        /// <param name="gizmosColor"></param>
        public static void GizmosDrawArrow(Vector2 start, Vector2 end, float headArc = 90f, float headLength = 0.5f, Color? gizmosColor = null)
        {
            Gizmos.color = gizmosColor ?? Color.green;

            Gizmos.DrawLine(start, end);
            headArc = ClampAngle(headArc);
            var angle = (start - end).normalized.GetAngle() + 180;

            Gizmos.DrawLine(end, end - (angle + headArc / 2f).GetAngleDirection() * headLength);
            Gizmos.DrawLine(end, end - (angle - headArc / 2f).GetAngleDirection() * headLength);

        }

        public static void GizmosDrawBezierCurve(BezierCurve curve, int points = 10, Color? gizmosColorCurve = null, Color? gizmosColorControlPoints = null)
        {
            Gizmos.color = gizmosColorCurve ?? Color.green;


            var b = new BezierCurve(curve);
            if (b == null)
                return;
            if (b.lineControlPoints != null)
            {
                var PointsPos = b.GetPathPoints();

                for (int i = 0; i < PointsPos.Count - 1; i++)
                {
                    Gizmos.DrawLine(PointsPos[i], PointsPos[i + 1]);

                }
                var color = gizmosColorControlPoints ?? Color.red;
                for (int i = 0; i < b.lineControlPoints.Count; i++)
                {
                    GizmosDrawCross(b.lineControlPoints[i], 0.5f, color);
                }
            }
        }

        /// <summary>
        /// Swaps two references.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Returns a random point inside this bounds.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 RandomPoint(this Bounds b)
        {

            Vector3 randomPos = new Vector3(UnityEngine.Random.Range(b.min.x, b.max.x),
                                            UnityEngine.Random.Range(b.min.y, b.max.y),
                                            UnityEngine.Random.Range(b.min.z, b.max.z));

            return randomPos;
        }

        public static T RandomEnumValue<T>()
        {
            try
            {
                var v = Enum.GetValues(typeof(T));
                return (T)v.GetValue(new System.Random().Next(v.Length));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return default(T);
        }

        #endregion

    }
}