/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 13
Due: 4/21/2017 at 11:59 pm
Sources: Sam Chen with the math assist.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nash_HW13
{
    public class Planet
    {
        private Point m_location;
        private CenterOfGravity m_centerOfGravity;
        private double m_distanceFromCOG;

        public Point Location
        {
            get
            {
                return m_location;
            }
            set
            {
                if ( m_location != value)
                {
                    m_location = value;
                }
            }
        }

        public CenterOfGravity CenterOfGravity
        {
            get
            {
                return m_centerOfGravity;
            }
            set
            {
                if (m_centerOfGravity != value)
                {
                    m_centerOfGravity = value;
                }
            }
        }
        public double DistanceFromCOG
        {
            get
            {
                return m_distanceFromCOG;
            }
            set
            {
                if (m_distanceFromCOG != value)
                {
                    m_distanceFromCOG = value;
                }
            }
        }


        public Planet(Point planetLocation)
        {
            m_location = planetLocation;
            m_distanceFromCOG = 10000000;
        }

        // Determines and sets the center of gravity for a planet. If no center of gravity is in range, the planet will not move.
        public void DetermineCOG(CenterOfGravity possibleCOG)
        {
            //Determine the distance between the point of gravity and the planet
            var d = Math.Sqrt(Math.Pow(m_location.X - possibleCOG.Location.X, 2) + Math.Pow(m_location.Y - possibleCOG.Location.Y, 2));
            var dist = Math.Abs(d);

            // Verify that the planet is within the radius of the center of gravity
            if (dist <= possibleCOG.Radius)
            {
                if (dist < m_distanceFromCOG)
                {
                    m_distanceFromCOG = dist;

                    //Set the center of gravity
                    m_centerOfGravity = possibleCOG;
                }
            }
        }

        // Rotate function - moves the planet around the center of gravity
        public void RotatePlanet()
        {
            int distanceX = m_location.X - m_centerOfGravity.Location.X;
            int distanceY = m_location.Y - m_centerOfGravity.Location.Y;

            m_location.X = (int)(m_centerOfGravity.Location.X + (distanceX * (Math.Cos(5 * Math.PI / 180))) - distanceY * (Math.Sin(5 * Math.PI / 180)));
            m_location.Y = (int)(m_centerOfGravity.Location.Y + (distanceY * (Math.Cos(5 * Math.PI / 180))) + distanceX * (Math.Sin(5 * Math.PI / 180)));
        }

    }
}
