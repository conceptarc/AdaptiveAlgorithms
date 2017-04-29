using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsTabuTSP
{
    class Tabu
    {
        LocationMap map;
        int[] path;
        int[] bestPath;
        int[,] tabuList;
        double bestDistance;
        double currentDistance;
        int tabuListSize;

        public Tabu(LocationMap map)
        {
            this.map = map; // use the provided LocationMap for TSP
            tabuListSize = (int)Math.Sqrt(map.PointCollection.Count) + 1; // heuristic for a good list size

            // assume LocationMap can choose whether to loop the last point in the path back to the first point
            path = new int[map.PointCollection.Count];
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = i;
            }
            bestPath = path.ToArray();

            tabuList = new int[map.PointCollection.Count, map.PointCollection.Count];
            for (int i = 0; i < map.PointCollection.Count; i++)
            {
                for (int j = 0; j < map.PointCollection.Count; j++)
                {
                    tabuList[i, j] = 0;
                }
            }

            currentDistance = map.CalcRouteDistance(path);
            bestDistance = currentDistance;
        }

        public int[] GetPath()
        {
            return path;
        }

        public int[] GetBestPath()
        {
            return bestPath;
        }

        void SwapIndex(int[] array, int a, int b)
        {
            int temp = array[a]; // array is reference type
            array[a] = array[b];
            array[b] = temp;
        }

        public void SearchOneIteration()
        {
            if (path.Length < 2) return; // don't even bother

            // use the first neighbor swap to begin with
            int bestI = -1;
            int bestJ = -1;
            double currentBestDistance = double.MaxValue;

            // first search the neighborhood of potential swaps
            Dictionary<Tuple<int, int>, int[]> neighborhoodList = new Dictionary<Tuple<int, int>, int[]>();
            for (int i = 0; i < path.Length; i++)
            {
                for (int j = i + 1; j < path.Length; j++)
                {
                    int[] newPath = path.ToArray(); // need a deep copy
                    SwapIndex(newPath, i, j);
                    double thisDistance = map.CalcRouteDistance(newPath);
                    if ((tabuList[i, j] <= 0 && thisDistance < currentBestDistance) ||
                        (thisDistance < bestDistance)) // less than global best aspiration criteria
                    {
                        bestI = i;
                        bestJ = j;
                        currentBestDistance = thisDistance;
                        if (thisDistance < bestDistance)
                        {
                            bestDistance = thisDistance;
                            bestPath = newPath.ToArray(); // make a copy of the global best solution
                        }
                    }

                    // decrement the tabu list while we are at it
                    if (tabuList[i, j] > 0)
                        tabuList[i, j]--;

                }
            }
            
            // switch to the selected neighbor and then update the tabu list
            if (bestI != -1 && bestJ != -1)
            {
                SwapIndex(path, bestI, bestJ);
                tabuList[bestI, bestJ] = tabuListSize;

            }
        }
    }
}
