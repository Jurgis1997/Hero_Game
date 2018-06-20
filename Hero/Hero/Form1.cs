using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hero
{
    public partial class Form1 : Form
    {
        Random random;
        enum Board
        {
            Space,
            Hero,
            Fruit
        };
        enum Movement
        {
            Up,
            Down,
            Left,
            Right
        };
        struct HeroCoord
        {
            public int x;
            public int y;
        }

        Board[,] gameboard;
        HeroCoord[] HeroXY;
        int Herosize;
        Movement move;
        Graphics D;

        public Form1()
        {
            InitializeComponent();
            gameboard = new Board[11, 11];
            HeroXY = new HeroCoord[100];
            random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(420, 420);
            D = Graphics.FromImage(pictureBox1.Image);
            D.Clear(Color.White);

            for (int i = 1; i <= 10; i++)
            {
                D.DrawImage(imageList1.Images[3], i * 35, 0);
                D.DrawImage(imageList1.Images[3], i * 35, 385);
            }
            for (int i = 0; i <= 11; i++)
            {
                D.DrawImage(imageList1.Images[3], 0, i * 35);
                D.DrawImage(imageList1.Images[3], 385, i * 35);
            }

            HeroXY[0].x = 5;
            HeroXY[0].y = 5;
            HeroXY[1].x = 5;
            HeroXY[1].y = 6;
            HeroXY[2].x = 5;
            HeroXY[2].y = 7;

            D.DrawImage(imageList1.Images[1], 5 * 35, 5 * 35);
            D.DrawImage(imageList1.Images[0], 5 * 35, 6 * 35);
            D.DrawImage(imageList1.Images[0], 5 * 35, 7 * 35);

            gameboard[5, 5] = Board.Hero;
            gameboard[5, 6] = Board.Hero;
            gameboard[5, 7] = Board.Hero;

            move = Movement.Up;
            Herosize = 3;

            for (int i = 0; i < 5; i++)
            {
                Fruit();
            }
        }
        private void Fruit()
        {
            int x, y;
            do
            {
            x = random.Next(1, 10);
            y = random.Next(1, 10);
            }
            while (gameboard[x, y] != Board.Space);

            gameboard[x, y] = Board.Fruit;
            D.DrawImage(imageList1.Images[2], x * 35, y * 35);

        
        }

        private void Hero_Keydown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    move = Movement.Up;
                    break;
                case Keys.Down:
                    move = Movement.Down;
                    break;
                case Keys.Left:
                    move = Movement.Left;
                    break;
                case Keys.Right:
                    move = Movement.Right;
                    break;
            }
        }
        private void GameOver()
        {
            timer1.Enabled = false;
            MessageBox.Show("Better luck next time.");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            D.FillRectangle(Brushes.White, HeroXY[Herosize - 1].x * 35, HeroXY[Herosize - 1].y * 35,35,35);
            gameboard[HeroXY[Herosize - 1].x, HeroXY[Herosize - 1].y] = Board.Space;
            pictureBox1.Refresh();

            for ( int i = Herosize; i >= 1; i--)
            {
                HeroXY[i].x = HeroXY[i - 1].x;
                HeroXY[i].y = HeroXY[i - 1].y;
            }
            D.DrawImage(imageList1.Images[0], HeroXY[0].x * 35, HeroXY[0].y * 35);

            switch (move)
            {
                case Movement.Up:
                    HeroXY[0].y = HeroXY[0].y - 1;
                    break;
                case Movement.Down:
                    HeroXY[0].y = HeroXY[0].y + 1;
                    break;
                case Movement.Left:
                    HeroXY[0].x = HeroXY[0].x - 1;
                    break;
                case Movement.Right:
                    HeroXY[0].x = HeroXY[0].x + 1;
                    break;
            }

            if (HeroXY[0].x < 1 || HeroXY[0].x > 10 || HeroXY[0].y < 1 || HeroXY[0].x > 10)
            {
                GameOver();
                pictureBox1.Refresh();
                return;
            }

            if (gameboard[HeroXY[0].x,HeroXY[0].y] == Board.Hero)
            {
                GameOver();
                pictureBox1.Refresh();
                return;
            }
            if (gameboard[HeroXY[0].x, HeroXY[0].y] == Board.Fruit)
            {
                D.DrawImage(imageList1.Images[0], HeroXY[Herosize].x * 35, HeroXY[Herosize].y * 35);
                gameboard[HeroXY[Herosize].x, HeroXY[Herosize].y] = Board.Hero;
                Herosize++;

                if (Herosize<99)
                {
                    Fruit();
                }
                this.Text = "Hero - score:" + Herosize;
            }
            D.DrawImage(imageList1.Images[1], HeroXY[0].x * 35, HeroXY[0].y * 35);
            gameboard[HeroXY[0].x, HeroXY[0].y] = Board.Hero;
            pictureBox1.Refresh();
        }
    }
}
