using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Demo_Battle_Field
{
    class Player
    {
        private Panel map;
        bool goup;
        bool godown;
        bool goleft;
        bool goright;
        public string facing = "godown";
        public int speed = 10;
        int interval = 70;
        public int level = 1;
        //public int ammo = 10;
        public PictureBox player = new PictureBox();
        public Timer timer = new Timer();
        public Timer timerFlicker = new Timer();
        public Label lblAmmo = new Label();
        public SoundPlayer bulletMusic = new SoundPlayer("../../Resources/shoot.wav");

        public Player(Panel panel)
        {
            //this.lblAmmo = lblAmmo;
            this.map = panel;
            timer.Interval = interval;
            timer.Tick += new EventHandler(Timer_Tick);
            timerFlicker.Interval = 100;
            timerFlicker.Tick += new EventHandler(TimerFlicker_Tick);
        }

        int count = 0;
        private void TimerFlicker_Tick(object sender, EventArgs e)
        {
            player.Visible = !player.Visible;
            count++;
            if (count == 10)
            {
                player.Visible = true;
                count = 0;
                timerFlicker.Enabled = false;
            }

        }

        int num = 1;
        string dir = @"../../Resources/";
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (num > 4)
            {
                num = 1;
            }

            Bitmap image = null;
            if (goleft)
            {
                image = new Bitmap(dir + "WalkLeft" + num + ".png");
                if (player.Left > 34)
                    player.Left -= speed;
            }
            if (goright)
            {
                image = new Bitmap(dir + "WalkRight" + num + ".png");
                if (player.Left + player.Width + 30 < map.Width)
                    player.Left += speed;
            }
            if (goup)
            {
                image = new Bitmap(dir + "WalkUp" + num + ".png");
                if (player.Top > 75)
                    player.Top -= speed;
            }
            if (godown)
            {
                image = new Bitmap(dir + "WalkDown" + num + ".png");
                if (player.Top + player.Height < map.Height - 30)
                    player.Top += speed;
            }

            player.Image = image;
            player.BackgroundImageLayout = ImageLayout.Stretch;
            player.BackColor = Color.Transparent;
            num++;
        }

        internal void MakePlayer(Point p)
        {
            player.Size = new Size(40, 40);
            player.Location = p;
            player.BackgroundImage = Properties.Resources.StandDown;
            player.SizeMode = PictureBoxSizeMode.Zoom;
            player.BackgroundImageLayout = ImageLayout.Stretch;
            player.BackColor = Color.Transparent;
            player.BringToFront();
            map.Controls.Add(player);
        }

        internal void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
                facing = "left";
                timer.Enabled = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
                facing = "right";
                timer.Enabled = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goup = true;
                facing = "up";
                timer.Enabled = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = true;
                facing = "down";
                timer.Enabled = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                bulletMusic.Play();
                shootBullet(facing);
                
            }
        }

        private void shootBullet(string facing)
        {
            Bullet bullet = new Bullet(map);
            bullet.direction = facing;
            bullet.levelBullet = level;

            bullet.makeBullet(player, "bullet");
            //ammo--;
            //lblAmmo.Text = ammo.ToString();
        }

        internal void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goup = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = false;
            }

            timer.Enabled = false;
            setFacingPlayer();
        }

        private void setFacingPlayer()
        {
            Bitmap image = null;
            if (facing == "up")
            {
                image = new Bitmap(Properties.Resources.StandUp);
            }
            if (facing == "down")
            {
                image = new Bitmap(Properties.Resources.StandDown);
            }
            if (facing == "left")
            {
                image = new Bitmap(Properties.Resources.StandLeft);
            }
            if (facing == "right")
            {
                image = new Bitmap(Properties.Resources.StandRight);
            }

            player.Image = image;
            player.BackgroundImageLayout = ImageLayout.Stretch;
            player.BackColor = Color.Transparent;
            player.BringToFront();
        }
    }
}
