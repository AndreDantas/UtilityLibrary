using System;
using System.Collections.Generic;
using UtilityLibrary.Classes;

namespace UtilityLibrary.Pathfinding
{
    public static class BreadthFirstSearch
    {
        /// <summary>
        /// Returns all positions that can be reached from starting point. 
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="validPosition">Function to check if position is valid.</param>
        /// <param name="getNeighbors">Function to get the position's neighbors.</param>
        /// <returns></returns>
        public static List<Position> FindRange(
            Position start,
            Func<Position, bool> validPosition,
            Func<Position, List<Position>> getNeighbors)
        {

            var result = new List<Position>();

            var open = new Queue<Position>();
            open.Enqueue(start);

            while (open.Count > 0)
            {
                var pos = open.Dequeue();

                if ((validPosition?.Invoke(pos) ?? false) && !result.Contains(pos))
                {
                    result.Add(pos);
                    foreach (var item in getNeighbors?.Invoke(pos))
                    {
                        if (!result.Contains(item) && !open.Contains(item))
                            open.Enqueue(item);
                    }

                }
            }
            return result;
        }

    }
}