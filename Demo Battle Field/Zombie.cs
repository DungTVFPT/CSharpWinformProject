using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo_Battle_Field
{

    public class Zombie
    {
        Panel map;
        public PictureBox zombie;
        public Timer timerZombie = new Timer();
        private Timer timerSpawner = new Timer();
        public Timer timerBullet = new Timer();
        string dir = @"../../Resources/";
        private bool direct;
        public int levelGame;
        PictureBox player;
        public int zombieSpeed = 3;
        bool facing = true;
        public string facing1 = "down";
        public bool Direct
        {
            get { return direct; }
            set { direct = value; }
        }

        public Zombie(Panel map, PictureBox player)
        {
            this.map = map;
            this.player = player;
            zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Parent = map;

        }

        public Zombie(Panel map)
        {
            this.map = map;
            zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Parent = map;
        }

        int count = 0;
       

        internal void MakeZombie(Point point)
        {
            zombie.Size = new Size(45, 45);
            zombie.SizeMode = PictureBoxSizeMode.Zoom;
            zombie.BackgroundImageLayout = ImageLayout.Stretch;
            zombie.BackColor = Color.Transparent;
            zombie.Location = point;
            timerSpawner.Tick += new EventHandler(TimerSpawner_Tick); // assignment the timer with an event
            timerSpawner.Interval = 200;
            timerSpawner.Enabled = true;
            timerBullet.Tick += new EventHandler(TimerBullet_Tick); // assignment the timer with an event
            timerBullet.Interval = 3000;
        }

        private void TimerBullet_Tick(object sender, EventArgs e)
        {
            shootBullet(facing);
        }

        private void shootBullet(bool facing)
        {
            Bullet bullet = new Bullet(map);
            bullet.direction = facing1;
            bullet.speed = 5;
            //bullet.levelBullet = level;

            bullet.makeBullet(zombie, "bulletZb");
        }

        private Image[] list
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources.Spawn1,
                    Properties.Resources.Spawn2,
                    Properties.Resources.Spawn3,
                    Properties.Resources.Spawn4
                };
            }
        }

        internal void Move(int levelGame)
        {
            this.levelGame = levelGame;
            timerZombie.Tick += new EventHandler(TimerZombie_Tick); // assignment the timer with an event
            timerZombie.Interval = 300;
            timerZombie.Start();
        }

        private void TimerZombie_Tick(object sender, EventArgs e)
        {
            switch (levelGame)
            {
                case 1:
                    zombieSpeed = 3;
                    MoveLevel1();
                    break;
                case 2:
                    zombieSpeed = 3;                  
                    MoveLevel2();
                    break;
                case 3:
                    zombieSpeed = 3;
                    MoveLevel1();
                    timerBullet.Enabled = true;
                    break;
                case 4:
                    zombieSpeed = 3;
                    MoveLevel2();
                    timerBullet.Enabled = true;
                    break;
                case 5:
                    zombieSpeed = 4;
                    MoveLevel2();
                    timerBullet.Enabled = true;
                    break;
                default:
                    zombieSpeed += 1;
                    MoveLevel2();
                    timerBullet.Enabled = true;
                    break;
            }
        }

        private void MoveLevel2()
        {
            if (num > 3)
            {
                num = 1;
            }

            Bitmap image = null;

            if (zombie.Top > player.Top)
            {
                zombie.Top -= zombieSpeed;
                image = new Bitmap(dir + "EWU" + num + ".png");
            }

            if (zombie.Top < player.Top)
            {
                zombie.Top += zombieSpeed;
                image = new Bitmap(dir + "EWD" + num + ".png");
            }

            if (zombie.Left < player.Left)
            {
                zombie.Left += zombieSpeed;
                image = new Bitmap(dir + "EWR" + num + ".png");
            }
            if (zombie.Left > player.Left)
            {
                zombie.Left -= zombieSpeed;
                image = new Bitmap(dir + "EWL" + num + ".png");
            }

            zombie.Image = image;
            zombie.BackgroundImageLayout = ImageLayout.Stretch;
            zombie.BackColor = Color.Transparent;
            num++;
        }

        private void MoveLevel1()
        {
            if (num > 3)
            {
                num = 1;
            }
            Bitmap image = new Bitmap(Properties.Resources.EWD1);

            switch (facing1)
            {
                case "up":
                    zombie.Top -= zombieSpeed;
                    image = new Bitmap(dir + "EWU" + num + ".png");
                    if (zombie.Top <= 60)
                    {
                        facing1 = "down";
                    }
                    break;
                case "down":
                    zombie.Top += zombieSpeed;
                    image = new Bitmap(dir + "EWD" + num + ".png");
                    if (zombie.Top >= map.Height - 80)
                    {
                        facing1 = "up";
                    }
                    break;
                case "right":
                    zombie.Left += zombieSpeed;
                    image = new Bitmap(dir + "EWR" + num + ".png");
                    if (zombie.Left >= map.Width - 65)
                    {
                        facing1 = "left";
                    }
                    break;
                case "left":
                    zombie.Left -= zombieSpeed;
                    image = new Bitmap(dir + "EWL" + num + ".png");
                    if (zombie.Left <= 20)
                    {
                        facing1 = "right";
                    }
                    break;
            }
            zombie.Image = image;
            zombie.BackgroundImageLayout = ImageLayout.Stretch;
            zombie.BackColor = Color.Transparent;

            num++;
        }
        int num = 1;
        private void MoveLevel_1()
        {
            if (num > 3)
            {
                num = 1;
            }
            Bitmap image = new Bitmap(Properties.Resources.EWD1);
            if (Direct)
            {
                if (facing == true)
                {
                    zombie.Top += zombieSpeed;
                    image = new Bitmap(dir + "EWD" + num + ".png");
                }
                if (zombie.Top >= map.Height - 80)
                {
                    facing = false;
                }

                if (zombie.Top <= 60)
                {
                    facing = true;
                }

                if (facing == false)
                {
                    zombie.Top -= zombieSpeed;
                    image = new Bitmap(dir + "EWU" + num + ".png");
                }
            }
            else
            {
                if (facing == true)
                {
                    zombie.Left += zombieSpeed;
                    image = new Bitmap(dir + "EWR" + num + ".png");
                }
                if (zombie.Left >= map.Width - 65)
                {
                    facing = false;
                }

                if (zombie.Left <= 20)
                {
                    facing = true;
                }

                if (facing == false)
                {
                    zombie.Left -= zombieSpeed;
                    image = new Bitmap(dir + "EWL" + num + ".png");
                }
            }
            zombie.Image = image;
            zombie.BackgroundImageLayout = ImageLayout.Stretch;
            zombie.BackColor = Color.Transparent;

            num++;
        }

        int i = 0;
        private void TimerSpawner_Tick(object sender, EventArgs e)
        {
            if (i < list.Count())
            {
                zombie.Image = list[i];
                
                
            }
            i++;
            if (i == list.Count())
            {
                zombie.Image = Properties.Resources.EWD1;
                map.Controls.Add(zombie);
                timerSpawner.Enabled = false;
            }
        }
        
    }
}
