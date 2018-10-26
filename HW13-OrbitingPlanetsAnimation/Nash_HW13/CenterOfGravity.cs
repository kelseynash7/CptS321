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
    public class CenterOfGravity
    {
        private Point m_location;

        private int m_radius;

        public Point Location
        {
            get
            {
                return m_location;
            }
            set
            {
                if (m_location != value)
                {
                    m_location = value;
                }
            }
        }

        public int Radius
        {
            get
            {
                return m_radius;
            }
            set
            {
                if (m_radius != value)
                {
                    m_radius = value;
                }
            }
        }

        public CenterOfGravity(Point location, int radius)
        {
            m_location = location;
            m_radius = radius;
        }
        public CenterOfGravity()
        {

        }

    }
}
