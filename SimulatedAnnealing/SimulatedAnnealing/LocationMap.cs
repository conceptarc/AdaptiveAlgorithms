using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsTabuTSP
{
    class LocationMap
    {
        public List<Tuple<double, double>> PointCollection;

        public LocationMap()
        {
            PointCollection = new List<Tuple<double, double>>();
        }

        ~LocationMap() // in C#, this is actually a finalizer (unlike the C++ destructor)
        {
        }

        public double CalcDistance(int point1, int point2)
        {
            double dX = PointCollection[point1].Item1 - PointCollection[point2].Item1;
            double dY = PointCollection[point1].Item2 - PointCollection[point2].Item2;

            return Math.Sqrt(dX * dX + dY * dY);
        }

        public double CalcRouteDistance(int[] path)
        {
            if (path.Length == 0) return 0;

            double totalDistance = 0;
            for (int i = 1; i < path.Length; i++) // note that we start from 1
            {
                totalDistance += CalcDistance(path[i], path[i - 1]);
            }
            // complete the loop
            totalDistance += CalcDistance(path[path.Length - 1], path[0]);

            return totalDistance;
        }
    }
}
