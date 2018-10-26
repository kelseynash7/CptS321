/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 13
Due: 4/21/2017 at 11:59 pm
Sources: Sam Chen with the math assist.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nash_HW13
{
    public partial class Form1 : Form
    {
        //List of points of center of gravity
        private List<CenterOfGravity> COG = new List<CenterOfGravity>();
        //List of points of planets
        private List<Planet> Planets = new List<Planet>();

        SolidBrush grayBrush = new SolidBrush(Color.LightGray);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
            // set default radius for center of gravity to 50
            numericUpDown1.Value = 50;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            MouseEventArgs click = e as MouseEventArgs;
            Point newPoint = new Point(click.X, click.Y);



            if (centerOfGravityButton.Checked)
            {
                int radius = (int)numericUpDown1.Value;

                //Add a new CenterOfGravity to the list
                CenterOfGravity newCOG = new CenterOfGravity(newPoint, radius);
                COG.Add(newCOG);

                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                Graphics circle = Graphics.FromImage(bmp);
                int index = 0;
                //display all centers of gravity
                foreach (CenterOfGravity cog in COG)
                {
                    int diameter = cog.Radius * 2;
                    circle.FillEllipse(grayBrush, cog.Location.X - cog.Radius, cog.Location.Y - cog.Radius, diameter, diameter);
                    circle.FillEllipse(blackBrush, cog.Location.X - 3, cog.Location.Y - 3, 6, 6);
                    index++;
                }
                //display any planets
                foreach (Planet planet in Planets)
                {
                    circle.FillEllipse(redBrush, planet.Location.X - 5, planet.Location.Y - 5, 10, 10);
                }

                pictureBox1.Image = bmp;


            }
            else if (planetButton.Checked)
            {

                bool inPull = false;
                CenterOfGravity gravity = new CenterOfGravity();
                double closestDist = 100000.00;

                //only add the point if it is within the area of the center of gravity
                //Determine the distance between the point of gravity and the planet
                foreach (CenterOfGravity cog in COG)
                {
                    var d = Math.Sqrt(Math.Pow(newPoint.X - cog.Location.X, 2) + Math.Pow(newPoint.Y - cog.Location.Y, 2));
                    var dist = Math.Abs(d);

                    if (dist <= cog.Radius)
                    {
                        inPull = true;
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            gravity.Location = cog.Location;
                            gravity.Radius = cog.Radius;
                        }
                        break;
                    }
                    else
                    {
                        inPull = false;
                    }
                }

                if (inPull == true)
                {
                    //add the planet to our list
                    Planet newPlanet = new Planet(newPoint);
                    newPlanet.DetermineCOG(gravity);
                    Planets.Add(newPlanet);

                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics planet = Graphics.FromImage(bmp);

                    //first redraw any centers of gravity
                    int index = 0;
                    foreach (CenterOfGravity cog in COG)
                    {
                        int diameter = cog.Radius * 2;
                        planet.FillEllipse(grayBrush, cog.Location.X - cog.Radius, cog.Location.Y - cog.Radius, diameter, diameter);
                        planet.FillEllipse(blackBrush, cog.Location.X - 3, cog.Location.Y - 3, 6, 6);
                        index++;
                    }
                    //then draw planets on top.
                    foreach (Planet pt in Planets)
                    {
                        planet.FillEllipse(redBrush, pt.Location.X - 5, pt.Location.Y - 5, 10, 10);
                    }

                    pictureBox1.Image = bmp;
                }
                else
                {
                    MessageBox.Show("Please place the planet in a center of gravity");
                }
            }
            else // neither is checked and that's an issue
            {
                MessageBox.Show("Please select an option: create a center of gravity or a planet.");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {           
            //rotate each planet to it's new position
            foreach (Planet pt in Planets)
            {
                pt.RotatePlanet();
            }

            //Get ready to draw
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics animate = Graphics.FromImage(bmp);

            //First fill the picture box with a solid color
            animate.FillRectangle(whiteBrush, 0, 0, pictureBox1.Width, pictureBox1.Height);

            //then redraw any centers of gravity
            int index = 0;
            foreach (CenterOfGravity cog in COG)
            {
                int diameter = cog.Radius * 2;
                animate.FillEllipse(grayBrush, cog.Location.X - cog.Radius, cog.Location.Y - cog.Radius, diameter, diameter);
                animate.FillEllipse(blackBrush, cog.Location.X - 3, cog.Location.Y - 3, 6, 6);
                index++;
            }

            //Render each planet.
            foreach (Planet pt in Planets)
            {
                animate.FillEllipse(redBrush, pt.Location.X - 5, pt.Location.Y - 5, 10, 10);
            }

            pictureBox1.Image = bmp;
        }


    }
}
