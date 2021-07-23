using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Demo_Battle_Field
{
    class Bullet
    {
        public string direction;

        public int speed = 10;
        public int levelBullet = 1;
        private PictureBox bullet = new PictureBox(); // create a picture box 
        private Timer timer = new Timer();
        
        private Panel map;

        public Bullet(Panel map)
        {
            this.map = map;
            timer.Interval = 20;
            timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            bullet.BackgroundImageLayout = ImageLayout.Stretch;
            bullet.SizeMode = PictureBoxSizeMode.Zoom;

            if (direction == "left")
            {
                if (strTag == "bullet")
                {
                    bullet.Image = new Bitmap(Properties.Resources.BulletRight);
                }
                else
                {
                    bullet.Image = new Bitmap(Properties.Resources.Rock2);
                }
                bullet.Left -= speed; // move bullet towards the left of the screen
            }
            if (direction == "right")
            {
                if (strTag == "bullet")
                {
                    bullet.Image = new Bitmap(Properties.Resources.BulletRight);
                }
                else
                {
                    bullet.Image = new Bitmap(Properties.Resources.Rock2);
                }
                bullet.Left += speed; // move bullet towards the right of the screen
            }
            // if direction is up
            if (direction == "up")
            {
                if (strTag == "bullet")
                {
                    bullet.Image = new Bitmap(Properties.Resources.BulletUp);
                }
                else
                {
                    bullet.Image = new Bitmap(Properties.Resources.Rock2);
                }
                bullet.Top -= speed; 
            }
            // if direction is down
            if (direction == "down")
            {
                if (strTag == "bullet")
                {
                    bullet.Image = new Bitmap(Properties.Resources.BulletUp);
                }
                else
                {
                    bullet.Image = new Bitmap(Properties.Resources.Rock2);
                }
                bullet.Top += speed; 
            }

           
            if (bullet.Left < 20 || bullet.Left > map.Width - 50 || bullet.Top < 60 || bullet.Top > map.Height - 65)
            {
                timer.Stop();
                timer.Dispose(); 
                bullet.Dispose(); 
                timer = null; 
                bullet = null;
                map.Controls.Remove(bullet);
            }
        }

        public string strTag = "";
        internal void makeBullet(PictureBox player, string tag)
        {
            strTag = tag;
            bullet.Size = new Size(30, 30); 

            bullet.Tag = tag; // set the tag to bullet
            bullet.Location = new Point(player.Left + 3, player.Top);
            bullet.BackColor = Color.Transparent;
            bullet.BackgroundImageLayout = ImageLayout.Stretch;
            bullet.BringToFront();
            map.Controls.Add(bullet);

            timer.Enabled = true;
        }
    }
}
