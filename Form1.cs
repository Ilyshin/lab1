using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task2SolarSystem
{
    public partial class Form1 : Form
    {
        static double x0 = Math.Round(0.903412226237334, 5);
        static double y0 = Math.Round(0.4350603957729373, 5);
        static double velx0 = Math.Round(-2.7053453110739483, 5);
        static double vely0 = Math.Round(5.617707423561698, 5);
        static double x1 = Math.Round(-1.29857914138552, 5);
        static double y1 = Math.Round(-1.009740649103588, 5);
        static double velx1 = Math.Round(2.874083542955071, 5);
        static double vely1 = Math.Round(-3.69622134435637, 5);
        static double x2 = Math.Round(4.960855451693971, 9);
        static double y2 = Math.Round(0.73202266974331077, 9);
        static double velx2 = Math.Round(-0.41644877737309244, 9);
        static double vely2 = Math.Round(2.8222224610481232, 9);
        static double[] m = new double[] { 3.0025138260432 * (1e-6), 3.2126696832579 * (1e-7), 0.9543137254901962 * (1e-3) };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            double delta = 0.00001;
            DrawChart(delta, false, 0);
            DrawChart(delta, true, 0);

            DeviationChart(delta);


            EnergyChart(delta, 0);
            EnergyChart(delta, 1);
            EnergyChart(delta, 2);
            LChart(delta, 0);
            LChart(delta, 1);
            LChart(delta, 2);

            DrawTextboxes(delta, false, 0);
            DrawTextboxes(delta, false, 1);
            DrawTextboxes(delta, false, 2);
            DrawTextboxes(delta, true, 0);
            DrawTextboxes(delta, true, 1);
            DrawTextboxes(delta, true, 2);

        }


        private void DrawTextboxes(double delta, bool interaction, int number)
        {

            int seriesNum = number;
            Planet planet0 = new Planet(x0, y0, velx0, vely0, 0, m[0]);
            Planet planet1 = new Planet(x1, y1, velx1, vely1, 1, m[1]);
            Planet planet2 = new Planet(x2, y2, velx2, vely2, 2, m[2]);
            var planets = new Planet[] { planet0, planet1, planet2 };
            TextBox[] textArray = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
            if (!interaction)
            {
                foreach (var planet in planets)
                    planet.M = 0;
            }
            else seriesNum += 3;
            planets[number].FindT(planets[(number + 1) % 3], planets[(number + 2) % 3], delta);
            planets[number].VMaxMinCoords(planets[(number + 1) % 3], planets[(number + 2) % 3], delta);

            var Vminx = planets[number].VminX.ToString();
            var Vminy = planets[number].VminY.ToString();
            var T = planets[number].T.ToString();
            var Vmaxx = planets[number].VmaxX.ToString();
            var Vmaxy = planets[number].VmaxY.ToString();
            textArray[seriesNum].Text = (seriesNum).ToString() + ")Vmin csords " + Vminx + " | " + Vminy + " |T = " + T + " | Vmax coords " + Vmaxx + " | " + Vmaxy;

        }


        private void DrawChart(double delta, bool interaction, int number)

        {
            int seriesNum = number;
            Planet planet0 = new Planet(x0, y0, velx0, vely0, 0, m[0]);
            Planet planet1 = new Planet(x1, y1, velx1, vely1, 1, m[1]);
            Planet planet2 = new Planet(x2, y2, velx2, vely2, 2, m[2]);
            var planets = new Planet[] { planet0, planet1, planet2 };
            if (!interaction)
            {
                foreach (var planet in planets)
                    planet.M = 0;
            }
            else seriesNum += 3;
            TextBox[] textArray = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
            double n = 50;
            for (double i = 0; i <= n; i += 0.05)
            {
                for (int j = 0; j < 3; j++)
                {
                    planets[j].FindPhase(planets[(j + 1) % 3], planets[(j + 2) % 3], delta, i, i + 0.05);
                    chart.Series[seriesNum+j].Points.AddXY(planets[j].X, planets[j].Y);
                }
            }
        }

        private void DeviationChart(double delta)
        {
            Planet planet0 = new Planet(x0, y0, velx0, vely0, 0, m[0]);
            Planet planet1 = new Planet(x1, y1, velx1, vely1, 1, m[1]);
            Planet planet2 = new Planet(x2, y2, velx2, vely2, 2, m[2]);
            Planet planet3 = new Planet(x0, y0, velx0, vely0, 0, 0);
            Planet planet4 = new Planet(x1, y1, velx1, vely1, 1, 0);
            Planet planet5 = new Planet(x2, y2, velx2, vely2, 2, 0);
            var planets = new Planet[] { planet0, planet1, planet2 };
            var planets1 = new Planet[] { planet3, planet4, planet5 };
            var interactionR = new double[3];
            var noInteractionR = new double[3];
            for (double i = 0; i <=20; i += 0.001)
            {
                for (int j = 0; j < 3; j++)
                {
                    interactionR[j] = planets[j].r;
                    noInteractionR[j] = planets1[j].r;
                    chart.Series[j + 6].Points.AddXY(i, Math.Abs(interactionR[j] - noInteractionR[j]));
                    planets[j].FindPhase(planets[(j + 1) % 3], planets[(j + 2) % 3], delta, i, i + 0.001);
                    planets1[j % 3].FindPhase(planets1[(j + 1) % 3], planets1[(j + 2) % 3], delta, i, i + 0.001);
                }
            }
        }
        private void EnergyChart(double delta, int number)
        {
            Planet planet0 = new Planet(x0, y0, velx0, vely0, 0, m[0]);
            Planet planet1 = new Planet(x1, y1, velx1, vely1, 1, m[1]);
            Planet planet2 = new Planet(x2, y2, velx2, vely2, 2, m[2]);
            var planets = new Planet[] { planet0, planet1, planet2 };
            double n = 20;
            for (double i = 0; i <= n; i += 0.001)
            {
                planets[number % 3].FindPhase(planets[(number + 1) % 3], planets[(number + 2) % 3], delta, i, i + 0.001);
                planets[(number + 2) % 3].FindPhase(planets[(number) % 3], planets[(number + 1) % 3], delta, i, i + 0.001);
                planets[(number + 1) % 3].FindPhase(planets[(number) % 3], planets[(number + 2) % 3], delta, i, i + 0.001);
                planets[number % 3].FindEnergy(planets[(number + 1) % 3], planets[(number + 2) % 3], delta);
                chart.Series[number + 9].Points.AddXY(i, planets[number].E);
            }
        }
        private void LChart(double delta, int number)
        {
            Planet planet0 = new Planet(x0, y0, velx0, vely0, 0, m[0]);
            Planet planet1 = new Planet(x1, y1, velx1, vely1, 1, m[1]);
            Planet planet2 = new Planet(x2, y2, velx2, vely2, 2, m[2]);
            var planets = new Planet[] { planet0, planet1, planet2 };
            double n = 20;
            for (double i = 0; i <= n; i += 0.1)
            {
                planets[number % 3].FindPhase(planets[(number + 1) % 3], planets[(number + 2) % 3], delta, i, i + 0.1);
                planets[(number + 2) % 3].FindPhase(planets[(number) % 3], planets[(number + 1) % 3], delta, i, i + 0.1);
                planets[(number + 1) % 3].FindPhase(planets[(number) % 3], planets[(number + 2) % 3], delta, i, i + 0.1);
                chart.Series[number + 12].Points.AddXY(i, planets[number].L);
            }
        }

    }
}
