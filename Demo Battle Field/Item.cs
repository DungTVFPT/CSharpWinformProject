using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Demo_Battle_Field
{
    class Item
    {
        private Panel map;
        public PictureBox item = new PictureBox();
        public Timer timer = new Timer();
        private int index;
        private bool status = true;

        public int Index { get => index; set => index = value; }

        public Item(Panel map)
        {
            this.map = map;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 100;
        }

        //int count = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (status)
            {
                item.Top = item.Top - 5;
                status = !status;
            }
            else
            {
                item.Top = item.Top + 5;
                status = !status;
            }
            //if (count == 40)
            //{
            //    timer.Enabled = false;
            //    timer.Dispose();
            //    item.Dispose();
            //    map.Controls.Remove(item);
            //}
            //count++;
        }

        internal bool MakeItem(Point local)
        {
            item.Size = new Size(40, 40);
            item.SizeMode = PictureBoxSizeMode.Zoom;
            item.BackgroundImageLayout = ImageLayout.Stretch;
            item.BackColor = Color.Transparent;
            item.Location = local;
            

            Random rd = new Random();
            this.Index = rd.Next(0, 100);
            if (Index <= 10)
            {
                item.Tag = "itemHp";
                item.Image = Properties.Resources.ItemHp;
            }
                
            if (Index > 10 && Index <= 20)
            {
                item.Tag = "itemScore";
                item.Image = Properties.Resources.ItemAmmo;
            }
                
            if (Index > 20 && Index <= 30)
            {
                item.Tag = "itemShield";
                item.Image = Properties.Resources.ItemShield;
            }
            if (Index > 30)
            {
                return false;
            }
                

            //item.Image = list[index];

            item.Parent = map;

            map.Controls.Add(item);

            timer.Enabled = true;
            return true;
        }

        internal bool MakeItem(Point local, int index)
        {
            item.Size = new Size(40, 40);
            item.SizeMode = PictureBoxSizeMode.Zoom;
            item.BackgroundImageLayout = ImageLayout.Stretch;
            item.BackColor = Color.Transparent;
            item.Location = local;
            this.Index = index;
            if (Index <= 10)
            {
                item.Tag = "itemHp";
                item.Image = Properties.Resources.ItemHp;
            }

            if (Index > 10 && Index <= 20)
            {
                item.Tag = "itemScore";
                item.Image = Properties.Resources.ItemAmmo;
            }

            if (Index > 20 && Index <= 30)
            {
                item.Tag = "itemShield";
                item.Image = Properties.Resources.ItemShield;
            }
            if (Index > 30)
            {
                return false;
            }


            //item.Image = list[index];

            item.Parent = map;

            map.Controls.Add(item);

            timer.Enabled = true;

            return true;
        }
    }
}
