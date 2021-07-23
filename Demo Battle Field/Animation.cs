using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo_Battle_Field
{
    class Animation
    {
        Panel panel = new Panel();
        int i = 0;
        PictureBox animation = new PictureBox();
        Timer tm = new Timer();
        private int level;
        public Animation(Panel panel)
        {
            this.panel = panel;
        }
        private Image[] list
        {
            get
            {
                return level == 1 ?
                new Image[]{
                    Properties.Resources.Explode1,
                    Properties.Resources.Explode2,
                    Properties.Resources.Explode3,
                    Properties.Resources.Explode4
                }
                : new Image[]{
                    Properties.Resources.Exp1,
                    Properties.Resources.Exp2,
                    Properties.Resources.Exp3,
                    Properties.Resources.Exp4,
                    Properties.Resources.Exp5 };
            }
        }

        internal void MakeAnimation(int x, int y, int level)
        {
            this.level = level;
            animation.Size = new Size(70, 70);
            animation.SizeMode = PictureBoxSizeMode.Zoom;
            animation.BackgroundImageLayout = ImageLayout.Stretch;
            animation.Location = new Point(x, y);
            tm.Interval = 70;
            tm.Tick += tm_Tick;
            tm.Start();
        }

        private void tm_Tick(object sender, EventArgs e)
        {
            if (i < list.Count())
            {
                animation.Image = list[i];
                animation.BackColor = Color.Transparent;
                animation.Parent = panel;
                animation.BringToFront();
                panel.Controls.Add(animation);
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
