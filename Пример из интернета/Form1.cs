using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLive
{
    public partial class Form1 : Form
    {
        private int generation = 0;
        private Random random = new Random();
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;

        private void StartGame()
        {
            if (Timer.Enabled)
                return;

            generation = 0;
            Text = $"Generation {generation}";
            resolution = (int)NUDRes.Value;

            NUDRes.Enabled = false;
            NUDDes.Enabled = false;

            rows = pictureBox2.Height / resolution;
            cols = pictureBox2.Width / resolution;

            field = new bool[cols, rows];

            for(int i = 0; i < cols; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    field[i, j] = random.Next((int)NUDDes.Value) == 0;
                }
            }

            

           
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            graphics = Graphics.FromImage(pictureBox2.Image);

            Timer.Start();

            
        }

        private int Count(int x, int y)
        {
            int num = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols ;
                    int row = (y + j + rows) % rows;
                    bool isSelfChecking = col == x && row == y;
                    bool hasLife = field[col, row];
                    if(hasLife && (!isSelfChecking))
                    {
                        num++;
                    }
                }
            }
            
            return num;
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var NewField = new bool[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int count = Count(i, j);
                    bool HasLife = field[i,j];

                    if(!HasLife && count==3)
                    {
                        NewField[i, j] = true;
                    }
                    else if (HasLife && ((count > 3) || (count < 2)))
                    {
                        NewField[i, j] = false;
                    }
                    else
                    {
                        NewField[i, j] = field[i, j];
                    }

                    if (HasLife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, i * resolution, j * resolution, resolution, resolution);
                    }
                }
            }
            field = NewField;
            pictureBox2.Refresh();
            Text = $"Generation {++generation}";
        }

        private void StopGame()
        {
            if(!Timer.Enabled)
            {
                return;
            }
            Timer.Stop();

            NUDRes.Enabled = true;
            NUDDes.Enabled = true;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void BStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void BStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if(!Timer.Enabled)
            {
                return;
            }
            if(e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                if(CheckMousePosition(x,y))
                    field[x, y] = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                if (CheckMousePosition(x, y))
                    field[x, y] = false;
            }
        }

        private bool CheckMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {generation}";
        }
    }
}
