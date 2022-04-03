using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Agario
{
    public partial class Form1 : Form
    {
        public struct Food
        {
            public float x, y, r;
            public Color color;

            public Food(float x, float y, float r, Color color)
            {
                this.x = x;
                this.y = y;
                this.r = r;
                this.color = color;
            }
        }

        public struct Player
        {
            public int score;
            public float spd, r, x, y;
            public Color color, bColor;

            public Player(float x, float y, float r, float spd, Color color)
            {
                this.x = x;
                this.y = y;
                this.r = r;
                this.spd = spd;
                this.color = color;
                this.score = (int)this.r;

                bColor = DimColor(color, 60);
            }
        }

        private struct Buttons
        {
            public bool Up, Down, Right, Left;

            public Buttons(bool c)
            {
                Up = c;
                Down = c;
                Right = c;
                Left = c;
            }
        }

        public void ChangeX(int i, float x)
        {
            Player temp = players[i];
            temp.x += x;
            players[i] = temp;
        }

        public void ChangeXTo(int i, float x)
        {
            Player temp = players[i];
            temp.x = x;
            players[i] = temp;
        }

        public void ChangeY(int i, float y)
        {
            Player temp = players[i];
            temp.y += y;
            players[i] = temp;
        }

        public void ChangeYTo(int i, float y)
        {
            Player temp = players[i];
            temp.y = y;
            players[i] = temp;
        }

        public void ChangeR(int i, float r)
        {
            Player temp = players[i];
            temp.r += r;
            temp.score = (int)temp.r;
            temp.spd = 2666.67f / temp.r;
            players[i] = temp;
        }

        public void ChangeScore(int i, int s)
        {
            Player temp = players[i];
            temp.score += s;
            temp.r += s;
            temp.spd = temp.score / 13.33f;
            players[i] = temp;
        }

        public Form1()
        {
            InitializeComponent();
        }

        List<Player> players = new List<Player>();
        List<Food> food = new List<Food>();
        Buttons btns = new Buttons(false);
        Random rnd = new Random();

        float baseSpeed = 15f;
        int foodPerSecond = 15;
        int foodVal = 4;
        int foodR = 10;

        private void Form1_Load(object sender, EventArgs e)
        {
            FTimer.Interval = 1000 / foodPerSecond;
            label1.Location = new Point(10, 10);

            players.Add(new Player(this.Width / 2, this.Height / 2, 200, baseSpeed, RndColor()));
            players.Add(new Player(this.Width / 2 + 500, this.Height / 2 + 500, 200, baseSpeed, RndColor()));
            players.Add(new Player(this.Width / 2 + 500, this.Height / 2 + 800, 300, baseSpeed, RndColor()));
            players.Add(new Player(this.Width / 2 + 500, this.Height / 2 + 1100, 400, baseSpeed, RndColor()));

            for (int i = 0; i < 750; i++)
                NewFood();

            NewFood();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                btns.Right = true;
            if (e.KeyCode == Keys.Left)
                btns.Left = true;
            if (e.KeyCode == Keys.Up)
                btns.Up = true;
            if (e.KeyCode == Keys.Down)
                btns.Down = true;

            if (e.KeyCode == Keys.Enter)
                NewFood();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                btns.Right = false;
            if (e.KeyCode == Keys.Left)
                btns.Left = false;
            if (e.KeyCode == Keys.Up)
                btns.Up = false;
            if (e.KeyCode == Keys.Down)
                btns.Down = false;
        }

        float dX = 0, dY = 0;
        void GMove()
        {
            display.Location = new Point((int)(display.Location.X + dX), (int)(display.Location.Y + dY));

            if ((int)dX > 0)
                dX -= (int)dX;
            else
                dX = 0;
            if ((int)dY > 0)
                dY -= (int)dY;
            else
                dY = 0;

            if (display.Location.X < 0)
                ChangeXTo(0, Math.Abs(display.Location.X) + this.Width / 2);
            else
                ChangeXTo(0, -display.Location.X + this.Width / 2);
            if (display.Location.Y < 0)
                ChangeYTo(0, Math.Abs(display.Location.Y) + this.Height / 2);
            else
                ChangeYTo(0, -display.Location.Y + this.Height / 2);

            if (btns.Right)
            {
                ChangeX(0, players[0].spd);
                dX -= players[0].spd;
            }
            if (btns.Left)
            {
                ChangeX(0, -players[0].spd);
                dX += players[0].spd;
            }
            if (btns.Up)
            {
                ChangeY(0, -players[0].spd);
                dY += players[0].spd;
            }
            if (btns.Down)
            {
                ChangeY(0, players[0].spd);
                dY -= players[0].spd;
            }

            if (players[0].x < players[0].r / 2)
            {
                ChangeXTo(0, players[0].r / 2);
                dX = 0;
            }
            if (players[0].x > display.Width - players[0].r / 2)
            {
                ChangeXTo(0, display.Width - players[0].r / 2);
                dX = 0;
            }
            if (players[0].y < players[0].r / 2)
            {
                ChangeYTo(0, players[0].r / 2);
                dY = 0;
            }
            if (players[0].y > display.Height - players[0].r / 2)
            {
                ChangeYTo(0, display.Height - players[0].r / 2);
                dY = 0;
            }
        }

        private static Color DimColor(Color inputColor, int dimCof)
        {
            int r = 0, g = 0, b = 0;

            if (inputColor.R > dimCof)
                r = inputColor.R - dimCof;
            if (inputColor.G > dimCof)
                g = inputColor.G - dimCof;
            if (inputColor.B > dimCof)
                b = inputColor.B - dimCof;

            return Color.FromArgb(r, g, b);
        }

        private Color RndColor()
        {
            switch(rnd.Next(0, 9))
            {
                case 0:
                    return Color.Blue;
                case 1:
                    return Color.LightGreen;
                case 2:
                    return Color.Yellow;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Orange;
                case 5:
                    return Color.Cyan;
                case 6:
                    return Color.Magenta;
                case 7:
                    return Color.Gold;
                case 8:
                    return Color.Firebrick;
                case 9:
                    return Color.LightBlue;
                default:
                    return Color.White;
            }
        }

        private void NewFood()
        {
            food.Add(new Food(rnd.Next(0, display.Width), rnd.Next(0, display.Height), foodR, RndColor()));
        }

        private void LoseMass()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].r > 50)
                    ChangeR(i, -players[i].r / 360);
            }
        }

        private void DrawPlayers(ref Graphics g)
        {
            List<Player> oPlayers = players.OrderBy(o => o.score).ToList();

            for (int i = 0; i < oPlayers.Count; i++)
            {
                g.DrawEllipse(new Pen(oPlayers[i].bColor, oPlayers[i].r / 25), oPlayers[i].x - oPlayers[i].r / 2, oPlayers[i].y - oPlayers[i].r / 2, oPlayers[i].r, oPlayers[i].r);
                g.FillEllipse(new SolidBrush(oPlayers[i].color), oPlayers[i].x - oPlayers[i].r / 2, oPlayers[i].y - oPlayers[i].r / 2, oPlayers[i].r, oPlayers[i].r);
            }
        }

        private void DrawFood(ref Graphics g)
        {
            for (int i = 0; i < food.Count; i++)
                g.FillEllipse(new SolidBrush(food[i].color), food[i].x, food[i].y, food[i].r, food[i].r);
        }

        private void DrawText(ref Graphics g)
        {
            label1.Text = "Score : " + players[0].score;

            Font font = new Font("Microsoft Sans Serif", players[0].r / 14f);
            g.DrawString("Score : " + players[0].score, font, new SolidBrush(Color.Black), players[0].x - g.MeasureString("Score : " + players[0].score, font).Width / 2, players[0].y - font.Size / 2);
        }

        private void Draw()
        {
            Bitmap b = new Bitmap(display.Width, display.Height);
            Graphics g = Graphics.FromImage(b);

            DrawFood(ref g);
            DrawPlayers(ref g);
            DrawText(ref g);

            display.Image = b;
            display.Refresh();

            b.Dispose();
            g.Dispose();
        }

        private void Logic()
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < food.Count; j++)
                {
                    if (((players[i].x - food[j].x) * (players[i].x - food[j].x) + (players[i].y - food[j].y) * (players[i].y - food[j].y)) < players[i].r / 2 * players[i].r / 2)
                    {
                        food.RemoveAt(j);
                        ChangeR(i, foodVal);
                    }
                }

                for (int j = 0; j < players.Count; j++)
                {
                    if (players[i].score >= players[j].score * 1.1)
                    {
                        if (((players[i].x - players[j].x) * (players[i].x - players[j].x) + (players[i].y - players[j].y) * (players[i].y - players[j].y)) < players[i].r / 2 * players[i].r / 2)
                        {
                            ChangeR(i, players[j].score * 0.75f);
                            players.RemoveAt(j);
                        }
                    }
                    else if (players[j].score >= players[i].score * 1.1)
                    {
                        if (((players[i].x - players[j].x) * (players[i].x - players[j].x) + (players[i].y - players[j].y) * (players[i].y - players[j].y)) < players[j].r / 2 * players[j].r / 2)
                        {
                            ChangeR(j, players[i].score * 0.75f);
                            players.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void GTimer_Tick(object sender, EventArgs e)
        {
            GMove();
            Logic();
            Draw();
        }

        private void FTimer_Tick(object sender, EventArgs e)
        {
            NewFood();
            LoseMass();
        }
    }
}