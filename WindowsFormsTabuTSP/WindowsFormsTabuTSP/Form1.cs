using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsTabuTSP
{
    public partial class Form1 : Form
    {
        int circleRadiusPixels = 10;
        Pen pen = new Pen(Color.Black);
        Pen penBold;

        List<Tuple<double, double>> pointsForTSP;
        bool beginTabuSearch;
        Tabu tabuSearch;
        long delayCounter = 0;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            
            beginTabuSearch = false;
            pointsForTSP = new List<Tuple<double, double>>();

            penBold = new Pen(Color.FromArgb((int)(0.3*255), 0, 0, 0), (float)(circleRadiusPixels / 1.5));
        }

        void DrawTSPPath(int[] path, Pen thisPen, PaintEventArgs e)
        {
            for (int i = 1; i < path.Length; i++)
            {
                Tuple<double, double> point1 = pointsForTSP[path[i]];
                Tuple<double, double> point2 = pointsForTSP[path[i - 1]];

                e.Graphics.DrawLine(thisPen, (float)point1.Item1, (float)point1.Item2, (float)point2.Item1, (float)point2.Item2);
            }
            // connect the end to the beginning
            Tuple<double, double> start = pointsForTSP[path[0]];
            Tuple<double, double> end = pointsForTSP[path[path.Length - 1]];
            e.Graphics.DrawLine(thisPen, (float)start.Item1, (float)start.Item2, (float)end.Item1, (float)end.Item2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            for (int i = 0; i < pointsForTSP.Count; i++)
            {
                Tuple<double, double> point = pointsForTSP[i];
                //e.Graphics.DrawLine(pen, 0, 0, lastMouseX + counter, lastMouseY);
                float x = (float)(point.Item1 - circleRadiusPixels / 2.0);
                float y = (float)(point.Item2 - circleRadiusPixels / 2.0);
                e.Graphics.DrawEllipse(pen, x, y, circleRadiusPixels, circleRadiusPixels);
            }

            if (beginTabuSearch && tabuSearch.GetPath().Length > 0)
            {
                if (delayCounter++ % 500 == 0) // delay
                {
                    tabuSearch.SearchOneIteration();
                }
                
                // tabuSearch should not be null anymore at this point

                int[] path = tabuSearch.GetPath();
                DrawTSPPath(path, pen, e); // draw the current path
                int[] bestPath = tabuSearch.GetBestPath();
                DrawTSPPath(bestPath, penBold, e); // draw the best path
            }

            Invalidate();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (beginTabuSearch) return;

            MouseEventArgs mEvent = (MouseEventArgs)e;

            Tuple<double, double> point = new Tuple<double, double>(mEvent.X, mEvent.Y);
            pointsForTSP.Add(point);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!beginTabuSearch)
            {
                beginTabuSearch = true;
                LocationMap map = new LocationMap();
                map.PointCollection = pointsForTSP;
                tabuSearch = new Tabu(map);
            }
        }
    }
}
