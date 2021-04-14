using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace МЧА_1
{
    public partial class Form1 : Form
    {
        private List<double> full = new List<double>();
        private List<double> inter = new List<double>();
        public Form1()
        {
            InitializeComponent();
        }

        double pogrInter()
        {
            double a = -5, b = -1, step = 0.001, x;
            x = a;
            while (x <= b)
            {
                inter.Add(Math.Abs(F(x) - L1(x, Node)));
                x += step;
            }
            inter.Sort();
            return inter[inter.Count-1];
        }

        private void button1_Click(object sender, EventArgs e)
        {

            double a = -10, b = 3, step = 0.25, x, y;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            x = -5;
            for (int i = 0; i < n; i++)
            {
                Node[i] = x;
                x += step;
            }

            x = a;
            while (x <= b)
            {
                y =F(x);
                chart1.Series[0].Points.AddXY(x, y);
                chart1.Series[1].Points.AddXY(x, L1(x, Node));
                x += step;
            }

            double pInter = pogrInter();
            textBox2.Text = "" + pInter;
        }

        static int n = 21;
        double[] Node = new double[n]; //массив узлов

        double F(double x) //исходная функция
        {
            return Math.Exp(2*x);
        }

        double L1(double x, double[] Node_1) //интерполяционный полином
        {
            double Polinom = 0;
            for (int i = 0; i < n; i++)
            {
                double p = 1;
                for (int j = 0; j < n; j++)
                    if (i != j)
                    {
                        p = p * (x - Node_1[j]) / (Node_1[i] - Node_1[j]);
                    }

                Polinom += F(Node_1[i]) * p; // Прибавлять нужно здесь 
            }
            return Polinom;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
