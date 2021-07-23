using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo_Battle_Field
{
    class AnimationLevelUp
    {
        int i = 0;
        PictureBox animation = new PictureBox();
        Timer tm = new Timer();
        private Panel map;

        public AnimationLevelUp(Panel map)
        {
            this.map = map;
        }
        private Image[] list
        {
            get
            {
                return new Image[]{
                    Properties.Resources.LevelUp6,
                    Properties.Resources.LevelUp5,
                    Properties.Resources.LevelUp4,
                    Properties.Resources.LevelUp3,
                    Properties.Resources.LevelUp2,
                    Properties.Resources.LevelUp1
                };
            }
        }

        internal void MakeAnimation()
        {
            animation.Size = new Size(200, 100);
            animation.SizeMode = PictureBoxSizeMode.Zoom;
            animation.BackgroundImageLayout = ImageLayout.Stretch;
            animation.Location = new Point(230, 150);
            tm.Interval = 200;
            tm.Tick += tm_Tick;
            tm.Start();
        }

        private void tm_Tick(object sender, EventArgs e)
        {
            if (i < list.Count())
            {
                animation.Image = list[i];
                animation.BackColor = Color.Transparent;
                animation.Parent = map;
                animation.BringToFront();
                map.Controls.Add(animation);
            }
            i++;
            if (i == list.Count())
            {
                tm.Stop();
                tm.Dispose();
                animation.Dispose();
                tm = null;
                animation = null;
            }
        }
    }
}
