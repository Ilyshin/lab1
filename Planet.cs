using System;
namespace Task2SolarSystem
{
    public class Planet
    {
        public double X, Y, Vx, Vy, M, VminX, VminY, T, VmaxX, VmaxY, N, X0, Y0, E;
        public int Number;

        public Planet(double x, double y, double vx, double vy, int number, double m, double vxmin = 0, double vymin = 0, double n = 0, double vmaxX =0, double vmanY=0)
        {
            X = x;
            Y = y;
            X0 = x;
            Y0 = Y;
            Vx = vx;
            Vy = vy;
            Number = number;
            M = m;
            N = n;
            
        }
        public double r => Math.Sqrt(X * X + Y * Y);
        public double V => Math.Sqrt(Vx * Vx + Vy * Vy);
        public double L =>r* M * V;


        private void FindVxVy(Planet planet1, Planet planet2, double delta)
        {
            double GM = 4 * Math.PI * Math.PI;
            double r0 = this.r;
            double r01 = Math.Sqrt((this.X - planet1.X) * (this.X - planet1.X) + (this.Y - planet1.Y) * (this.Y - planet1.Y));
            double r02 = Math.Sqrt((this.X - planet2.X) * (this.X - planet2.X) + (this.Y - planet2.Y) * (this.Y - planet2.Y));
            this.Vx = this.Vx + (-this.X / (r0 * r0 * r0) +Math.Pow(-1, (this.N*3/2))* planet1.M * (planet1.X - this.X) / (r01 * r01 * r01) + Math.Pow(-1, this.N/2)* planet2.M * (planet2.X - this.X) / (r02 * r02 * r02)) * GM * delta;
            this.Vy = this.Vy + (-this.Y / (r0 * r0 * r0) - planet1.M * (planet1.Y - this.Y) / (r01 * r01 * r01) + planet2.M * (planet2.Y - this.Y) / (r02 * r02 * r02)) * GM * delta;
        }

        private double FindX(double delta)
        {

            return this.X + this.Vx * delta;
        }

        private double FindY( double delta)
        {
            return this.Y + this.Vy * delta;

        }

        public void FindPhase(Planet planet1, Planet planet2, double delta, double startTime, double time)
        {
            while (startTime < time)
            {
                FindVxVy(planet1, planet2, delta);
                double tempX = FindX(delta);
                this.Y = FindY(delta);
                this.X = tempX;
                startTime += delta;
            }
        }
        public void FindT (Planet planet1, Planet planet2, double delta)
        {
            for (int i = 0; i<1000; i++)
            {
                FindPhase(planet1, planet2, delta, T, T + delta);
                planet1.FindPhase(this, planet2,delta, T, T + delta);
                planet2.FindPhase(this, planet1, delta, T, T + delta);
                this.T += delta;
            }
            while (!(Math.Abs(this.X0 - this.X) < 1e-4  && Math.Abs(this.Y0 - this.Y) < 0.1))
            {
                FindPhase(planet1, planet2, delta, T, T + delta);
                this.T += delta;
            }
            this.T = Math.Round(this.T, 5);
        }

        public void VMaxMinCoords (Planet planet1, Planet planet2, double delta)
        {
            if(T==0)
                this.FindT(planet1, planet2, delta);
            double Vmin = double.MaxValue;
            double Vmax = double.MinValue;
            double time = 0;
            while (time<this.T)
            {
                FindPhase(planet1, planet2, delta, T, T + delta);
                planet1.FindPhase(this, planet2, delta, T, T + delta);
                planet2.FindPhase(this, planet1, delta, T, T + delta);
                if (this.V < Vmin)
                {
                    this.VminX = Math.Round(this.X, 4);
                    this.VminY = Math.Round(this.Y, 4);
                    Vmin = this.V;
                }
                if (this.V > Vmax)
                {
                    this.VmaxX = Math.Round(this.X, 4);
                    this.VmaxY = Math.Round(this.Y, 4);
                    Vmax = this.V;
                }
                time += delta;
            }
        }
        public void FindEnergy (Planet planet1, Planet planet2, double delta)
        {
            double GM = 4 * Math.PI * Math.PI;
            double r01 = Math.Sqrt((this.X - planet1.X) * (this.X - planet1.X) + (this.Y - planet1.Y) * (this.Y - planet1.Y)); 
            double r02 = Math.Sqrt((this.X - planet2.X) * (this.X - planet2.X) + (this.Y - planet2.Y) * (this.Y - planet2.Y));
            this.E = this.V * this.V / 2 -  GM/this.r - GM*planet1.M/r01 - GM * planet2.M/ r02;

        }
       
    }
}
