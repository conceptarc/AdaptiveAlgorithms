﻿using System;
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
        Pen pen;
        Pen penLine;
        Pen penBold;

        List<Tuple<double, double>> pointsForTSP;
        bool beginTabuSearch;
        Tabu tabuSearch;
        long delayCounter = 0;

        bool refreshControlsOnLoad = true;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            
            beginTabuSearch = false;
            pointsForTSP = new List<Tuple<double, double>>();

            pen = new Pen(Color.FromArgb((int)(0.7 * 255), 0, 0, 0));
            penLine = new Pen(Color.FromArgb((int)(0.4 * 255), 0, 0, 0));
            penLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            penBold = new Pen(Color.FromArgb((int)(0.4*255), 70, 200, 200), (float)(circleRadiusPixels / 1.5));
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
                if (delayCounter++ % 25 == 0) // delay
                {
                    tabuSearch.SearchOneIteration();
                }
                else
                {
                    System.Threading.Thread.Sleep(5);
                }

                // tabuSearch should not be null anymore at this point

                int[] path = tabuSearch.GetPath();
                DrawTSPPath(path, penLine, e); // draw the current path
                int[] bestPath = tabuSearch.GetBestPath();
                DrawTSPPath(bestPath, penBold, e); // draw the best path
            }
            else
            {
                //buttonRestart.Enabled = false;
                System.Threading.Thread.Sleep(1);
            }

            Invalidate();
            if (refreshControlsOnLoad)
            {
                label1.Refresh();
                label2.Refresh();
                label3.Refresh();
                buttonRestart.Refresh();
                buttonStart.Refresh();
                refreshControlsOnLoad = false;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (beginTabuSearch) return;

            MouseEventArgs mEvent = (MouseEventArgs)e;

            Tuple<double, double> point = new Tuple<double, double>(mEvent.X, mEvent.Y);
            pointsForTSP.Add(point);
            
            Focus();
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            beginTabuSearch = false;
            pointsForTSP = new List<Tuple<double, double>>();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!beginTabuSearch && pointsForTSP.Count > 3)
            {
                beginTabuSearch = true;
                LocationMap map = new LocationMap();
                map.PointCollection = pointsForTSP;
                tabuSearch = new Tabu(map);
                //buttonRestart.Enabled = true;
            }
        }
    }
}
