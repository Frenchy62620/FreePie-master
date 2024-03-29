﻿using System;

namespace FreePIE.Plugin_Wiimote.Wiimote
{
    public class Nunchuck
    {
        public Acceleration Acceleration { get; set; }

        public AnalogStick Stick { get; set; }

        public NunchuckButtons Buttons { get; set; }
    }

    public class Acceleration
    {
        public Acceleration(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }
        public override string ToString()
        {
            return String.Format("x: {0}, y: {1}, z: {2}", x, y, z);
        }
    }
}
