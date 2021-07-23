using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Demo_Battle_Field
{
    public partial class FrmMain : Form
    {
        Player player;
        private List<Zombie> listZoombies = new List<Zombie>();
        private List<Item> listItems = new List<Item>();
        private bool direct = true;
        bool gameOver = true;
        private SoundPlayer music = new SoundPlayer("../../Resources/burst.wav");

        private Random rd = new Random();

        public FrmMain()
        {
            InitializeComponent();
            progressBarHealth.Style = ProgressBarStyle.Continuous;
            progressBarShield.Style = ProgressBarStyle.Continuous;
            if (StatusGame())
            {
                LoadInit();
            }
            else
            {
                InitPlayerAndZombie();
            }
            SetStatusFalse();

            timerGame.Enabled = true;

        }

        private void SetStatusFalse()
        {
            TextWriter tw = new StreamWriter("../../Resources/Status.txt", false);
            tw.Write(string.Empty);
            tw.Close();
            using (StreamWriter writer = new StreamWriter("../../Resources/Status.txt", true)) //// true to append data to the file
            {
                writer.WriteLine("false");
            }
        }

        public void LoadInit()
        {
            player = new Player(map);
            string[] playerInfo = File.ReadAllLines("../../Resources/player.txt");
            player.MakePlayer(new Point(Int32.Parse(playerInfo[0]), Int32.Parse(playerInfo[1])));
            player.facing = playerInfo[2];
            player.level = Int32.Parse(playerInfo[3]);
            player.speed = Int32.Parse(playerInfo[4]);

            progressBarHealth.Value = Int32.Parse(playerInfo[5]);
            progressBarShield.Value = Int32.Parse(playerInfo[6]);
            progressBarLevel.Value = Int32.Parse(playerInfo[7]);

            listZoombies.Clear();

            for (int i = 0; i < 4; i++)
            {
                string[] lines = File.ReadAllLines("../../Resources/" + i + ".txt");

                Zombie zombie1 = new Zombie(map, player.player);
                Point x = new Point(Int32.Parse(lines[0]), Int32.Parse(lines[1]));

                zombie1.facing1 = lines[2];
                zombie1.MakeZombie(x);
                zombie1.Move(player.level);
                listZoombies.Add(zombie1);
            }

            listItems.Clear();

            string[] amountItems = File.ReadAllLines("../../Resources/Items.txt");
            int amount = Int32.Parse(amountItems[0]);

            for (int i = 0; i < amount; i++)
            {
                string[] lines = File.ReadAllLines("../../Resources/Items" + i + ".txt");
                int index = Int32.Parse(lines[0]);
                Point local = new Point(Int32.Parse(lines[1]), Int32.Parse(lines[2]));
                Item item = new Item(map);
                if (item.MakeItem(local, index))
                {
                    listItems.Add(item);
                }
            }
        }

        private bool StatusGame()
        {
            string[] lines = File.ReadAllLines("../../Resources/Status.txt");
            try
            {
                bool status = bool.Parse(lines[0]);
                if (status == false)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }

            return true;
        }

        private void InitPlayerAndZombie()
        {
            player = new Player(map);
            player.MakePlayer(new Point(map.Width / 2, map.Height / 2));
            //lblAmmo.Text = player.ammo.ToString();
            listZoombies.Clear();
            for (int i = 0; i < 4; i++)
            {
                MakeAZombie();
            }

        }


        private string[] listFacing
        {
            get
            {
                return new string[]
                {
                    "up",
                    "left",
                    "down",
                    "right"
                };
            }
        }
        private void MakeAZombie()
        {
            Zombie zombie = new Zombie(map, player.player);
            Point x = new Point(rd.Next(50, this.Width - 110), rd.Next(80, this.Height - 130));
            zombie.MakeZombie(x);

            zombie.facing1 = listFacing[rd.Next(0, 4)].ToString();
            //zombie.Direct = direct;
            //direct = !direct;
            zombie.Move(player.level);
            listZoombies.Add(zombie);
        }

        int countMakeItem = 1;
        private void ReStartGame(object sender, EventArgs e)
        {
            if (progressBarLevel.Value == progressBarLevel.Maximum)
            {
                AnimationLevelUp();
                player.level++;
                progressBarLevel.Value = 0;
            }
            progressBarShield.Value = progressBarShield.Value - 1 < 0 ? 0 : progressBarShield.Value - 1;
            if (countMakeItem % 20 == 0)
            {
                MakeItems();
            }
            if (progressBarHealth.Value < 20)
                progressBarHealth.ForeColor = Color.Red;

            if (progressBarHealth.Value < 1)
            {
                player.timer.Enabled = false;
                player.player.Image = Properties.Resources.Death1;
                foreach (Zombie z in listZoombies.ToList())
                {
                    z.timerZombie.Enabled = false;
                    z.timerBullet.Enabled = false;
                }
                SetStatusFalse();
                timerGame.Stop();
                player.bulletMusic.Dispose();
                music.Stop();
                gameOver = false;

                updateScore();
                if (MessageBox.Show("Do you want to continue?", "notice!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    newGame();
                }
                else
                {
                    this.Dispose();
                }

            }

            ZombieIntersectZombie();
            ZombieIntersectBullets();
            ZombieIntersectPlayer();
            ZombieIntersectItems();
            PlayerIntersectItems();
            PlayerIntersectBullet();
            countMakeItem++;
        }

        private void PlayerIntersectBullet()
        {
            foreach (Control bullet in map.Controls)
            {
                if (bullet is PictureBox && Equals(bullet.Tag, "bulletZb"))
                {
                    if (bullet.Bounds.IntersectsWith(player.player.Bounds))
                    {
                        PictureBox bl = bullet as PictureBox;
                        player.timerFlicker.Enabled = true;
                        //zombie.timerFlicker.Enabled = true;
                        bl.Dispose();
                        bl = null;
                        map.Controls.Remove(bl);
                        if (progressBarShield.Value > 0)
                        {
                            return;
                        }
                        
                        int speedHealth = 3;
                        progressBarHealth.Value = progressBarHealth.Value <= speedHealth ? 0 : progressBarHealth.Value - speedHealth;
                        //SetTransparent(zombie, player.Avatar);
                    }
                }
                
            }
        }

        private void ZombieIntersectItems()
        {
            foreach (Zombie z in listZoombies.ToList())
            {
                foreach (Item i in listItems.ToList())
                {
                    if (z.zombie.Bounds.IntersectsWith(i.item.Bounds))
                    {
                        i.item.Dispose();
                        i.timer.Enabled = false;
                        map.Controls.Remove(i.item);
                        listItems.Remove(i);
                    }
                }
            }
        }

        private void updateScore()
        {
            List<int> list = new List<int>();
            string[] lines = File.ReadAllLines("../../Resources/FileScore.txt");
            try
            {
                foreach (string line in lines)
                {
                    int num = Int32.Parse(line);
                    list.Add(num);
                }
            }
            catch (Exception)
            {

            }
            list.Add(player.level);
            list.Sort();
            TextWriter tw = new StreamWriter("../../Resources/FileScore.txt", false);
            tw.Write(string.Empty);
            tw.Close();
            using (StreamWriter writer = new StreamWriter("../../Resources/FileScore.txt", true)) //// true to append data to the file
            {
                for (int i = list.Count - 1; i >= 1; i--)
                {
                    writer.WriteLine(list[i]);
                }
            }
            if ((list.Count - list.IndexOf(player.level)) <= 4)
            {
                MessageBox.Show("Congratulations to you ranked " + (list.Count - list.IndexOf(player.level)));
            }
            else
            {
                MessageBox.Show("End game! Congratulations to you level: " + player.level);
            }


        }

        private void ZombieIntersectZombie()
        {
            for (int i = 0; i < listZoombies.Count - 1; i++)
            {
                PictureBox z1 = listZoombies[i].zombie;
                for (int j = i + 1; j < listZoombies.Count; j++)
                {
                    PictureBox z2 = listZoombies[j].zombie;

                    if (z2.Bounds.IntersectsWith(z1.Bounds))
                    {
                        Zombie zb = listZoombies[j];
                        zb.zombie.Dispose();
                        zb.timerBullet.Enabled = false;
                        zb.timerBullet.Dispose();
                        zb.timerZombie.Enabled = false;
                        zb.zombie.Tag = null;
                        map.Controls.Remove(zb.zombie);
                        listZoombies.Remove(zb);



                        MakeAZombie();
                        //switch (listZoombies[i].facing1)
                        //{
                        //    case "up":
                        //        listZoombies[i].facing1 = "down";
                        //        if (listZoombies[j].facing1 == "down")
                        //        {
                        //            listZoombies[j].facing1 = "up";
                        //        }
                        //        else if (listZoombies[j].facing1 == "right")
                        //        {
                        //            listZoombies[j].facing1 = "left";
                        //        }
                        //        else if (listZoombies[j].facing1 == "left")
                        //        {
                        //            listZoombies[j].facing1 = "right";
                        //        }
                        //        break;
                        //    case "down":
                        //        listZoombies[i].facing1 = "up";
                        //        if (listZoombies[j].facing1 == "up")
                        //        {
                        //            listZoombies[j].facing1 = "down";
                        //        }
                        //        else if (listZoombies[j].facing1 == "right")
                        //        {
                        //            listZoombies[j].facing1 = "left";
                        //        }
                        //        else if (listZoombies[j].facing1 == "left")
                        //        {
                        //            listZoombies[j].facing1 = "right";
                        //        }
                        //        break;
                        //    case "right":
                        //        listZoombies[i].facing1 = "left";
                        //        if (listZoombies[j].facing1 == "left")
                        //        {
                        //            listZoombies[j].facing1 = "right";
                        //        }
                        //        else if (listZoombies[j].facing1 == "up")
                        //        {
                        //            listZoombies[j].facing1 = "down";
                        //        }
                        //        else if (listZoombies[j].facing1 == "down")
                        //        {
                        //            listZoombies[j].facing1 = "up";
                        //        }
                        //        break;
                        //    case "left":
                        //        listZoombies[i].facing1 = "right";
                        //        if (listZoombies[j].facing1 == "right")
                        //        {
                        //            listZoombies[j].facing1 = "left";
                        //        }
                        //        else if (listZoombies[j].facing1 == "up")
                        //        {
                        //            listZoombies[j].facing1 = "down";
                        //        }
                        //        else if (listZoombies[j].facing1 == "down")
                        //        {
                        //            listZoombies[j].facing1 = "up";
                        //        }
                        //        break;
                        //}
                    }
                }
            }
        }

        private void AnimationLevelUp()
        {
            AnimationLevelUp animation = new AnimationLevelUp(map);
            animation.MakeAnimation();
        }

        private void newGame()
        {
            foreach (Zombie z in listZoombies)
            {
                z.timerZombie.Enabled = false;
                z.timerZombie.Dispose();
                z.zombie.Dispose();
                map.Controls.Remove(z.zombie);
            }
            foreach (Item i in listItems)
            {
                i.timer.Dispose();
                i.item.Dispose();
                map.Controls.Remove(i.item);
            }
            listItems.Clear();
            listZoombies.Clear();

            player.player.Dispose();
            map.Controls.Remove(player.player);

            InitPlayerAndZombie();
            progressBarHealth.Value = 100;
            progressBarLevel.Value = 0;
            gameOver = true;
            timerGame.Enabled = true;
        }

        private void PlayerIntersectItems()
        {

            foreach (Item item in listItems.ToList())
            {
                if (item.item.Bounds.IntersectsWith(player.player.Bounds))
                {
                    switch (item.item.Tag)
                    {
                        case "itemHp":
                            progressBarHealth.Value = progressBarHealth.Value + 10 > 100 ? 100 : (progressBarHealth.Value + 10);
                            item.item.Dispose();
                            item.timer.Enabled = false;
                            map.Controls.Remove(item.item);
                            listItems.Remove(item);

                            break;
                        case "itemScore":
                            progressBarLevel.Value = progressBarLevel.Value + 3 > progressBarLevel.Maximum ? progressBarLevel.Maximum : progressBarLevel.Value + 3;
                            item.item.Dispose();
                            item.timer.Enabled = false;
                            map.Controls.Remove(item.item);
                            listItems.Remove(item);

                            break;
                        case "itemShield":
                            progressBarShield.Value = 100;
                            item.item.Dispose();
                            item.timer.Enabled = false;
                            map.Controls.Remove(item.item);
                            listItems.Remove(item);
                            break;
                    }

                }
            }

        }

        private void MakeItems()
        {
            Point local = new Point(rd.Next(50, this.Width - 110), rd.Next(80, this.Height - 130));
            Item item = new Item(map);
            //item.MakeItem(local);
            if (item.MakeItem(local))
            {
                listItems.Add(item);
            }

        }


        private void ZombieIntersectPlayer()
        {
            foreach (Zombie zombie in listZoombies.ToList())
            {
                PictureBox zb = zombie.zombie;
                if (zb.Bounds.IntersectsWith(player.player.Bounds))
                {
                    //Thread b = new Thread(flicker);
                    player.timerFlicker.Enabled = true;
                    //zombie.timerFlicker.Enabled = true;
                    if (progressBarShield.Value > 0)
                    {
                        return;
                    }
                    int speedHealth;
                    switch (player.level)
                    {
                        case 1:
                            speedHealth = 1;
                            break;
                        case 2:
                            speedHealth = 2;
                            break;
                        case 3:
                            speedHealth = 4;
                            break;
                        default:
                            speedHealth = 6;
                            break;
                    }
                    progressBarHealth.Value = progressBarHealth.Value <= speedHealth ? 0 : progressBarHealth.Value - speedHealth;
                    //SetTransparent(zombie, player.Avatar);
                }
            }
        }

        private void ZombieIntersectBullets()
        {

            foreach (Zombie z in listZoombies.ToList())
            {
                PictureBox zombie = z.zombie;
                foreach (Control bullet in map.Controls)
                {
                    #region Zombie va chạm với đạn
                    if (bullet is PictureBox && Equals(bullet.Tag, "bullet") && (Equals(zombie.Tag, "zombie")))
                    {
                        if (bullet.Bounds.IntersectsWith(zombie.Bounds))
                        {
                            //Score++;
                            music.Play();
                            progressBarLevel.Value = progressBarLevel.Value + 1 > progressBarLevel.Maximum ? progressBarLevel.Maximum : progressBarLevel.Value + 1;
                            PictureBox bl = bullet as PictureBox;

                            bl.Dispose();
                            bl = null;
                            map.Controls.Remove(bl);
                            zombie.Dispose();
                            z.timerZombie.Enabled = false;
                            z.timerBullet.Enabled = false;
                            z.timerBullet.Dispose();
                            z.timerZombie.Dispose();
                            zombie.Tag = null;
                            listZoombies.Remove(z);
                            map.Controls.Remove(zombie);

                            AnimationEvent(zombie.Left, zombie.Top);
                            MakeAZombie();
                        }
                    }
                    #endregion
                }
            }
        }

        private void AnimationEvent(int x, int y)
        {
            Animation animation = new Animation(map);
            animation.MakeAnimation(x, y, player.level);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (!gameOver)
            {
                return;
            }
            if (e.KeyCode == Keys.P)
            {
                PauseGame();
            }
            player.KeyIsUp(sender, e);
        }



        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (!gameOver)
            {
                return;
            }
            player.KeyIsDown(sender, e);
        }

        private void PauseGame()
        {
            if (label1.Text == "Pause")
            {
                label1.Text = "Continue";
                timerGame.Enabled = false;
                foreach (Zombie z in listZoombies.ToList())
                {
                    z.timerZombie.Enabled = false;
                    z.timerBullet.Enabled = false;
                }

                pustDataToFile();
            }
            else
            {
                label1.Text = "Pause";
                timerGame.Enabled = true;
                foreach (Zombie z in listZoombies.ToList())
                {
                    z.timerZombie.Enabled = true;
                    z.timerBullet.Enabled = true;
                }
            }
        }

        private void pustDataToFile()
        {
            //Status game
            TextWriter tw = new StreamWriter("../../Resources/Status.txt", false);
            tw.Write(string.Empty);
            tw.Close();
            using (StreamWriter writer = new StreamWriter("../../Resources/Status.txt", true)) //// true to append data to the file
            {
                writer.WriteLine("true");
            }

            //Player
            TextWriter tw1 = new StreamWriter("../../Resources/Player.txt", false);
            tw1.Write(string.Empty);
            tw1.Close();
            using (StreamWriter writer = new StreamWriter("../../Resources/Player.txt", true)) //// true to append data to the file
            {
                writer.WriteLine(player.player.Location.X);
                writer.WriteLine(player.player.Location.Y);
                writer.WriteLine(player.facing);
                writer.WriteLine(player.level);
                writer.WriteLine(player.speed);
                writer.WriteLine(progressBarHealth.Value);
                writer.WriteLine(progressBarShield.Value);
                writer.WriteLine(progressBarLevel.Value);
            }


            //Zombies
            for (int i = 0; i < listZoombies.Count; i++)
            {
                TextWriter tw2 = new StreamWriter("../../Resources/" + i + ".txt", false);
                tw2.Write(string.Empty);
                tw2.Close();
                using (StreamWriter writer = new StreamWriter("../../Resources/" + i + ".txt", true)) //// true to append data to the file
                {
                    writer.WriteLine(listZoombies[i].zombie.Location.X);
                    writer.WriteLine(listZoombies[i].zombie.Location.Y);
                    writer.WriteLine(listZoombies[i].facing1);
                }
            }

            //Items
            TextWriter tw4 = new StreamWriter("../../Resources/Items.txt", false);
            tw4.Write(string.Empty);
            tw4.Close();
            using (StreamWriter writer = new StreamWriter("../../Resources/Items.txt", true)) //// true to append data to the file
            {
                writer.WriteLine(listItems.Count);
            }
            for (int i = 0; i < listItems.Count; i++)
            {
                TextWriter tw3 = new StreamWriter("../../Resources/Items" + i + ".txt", false);
                tw3.Write(string.Empty);
                tw3.Close();
                using (StreamWriter writer = new StreamWriter("../../Resources/Items" + i + ".txt", true)) //// true to append data to the file
                {
                    writer.WriteLine(listItems[i].Index);
                    writer.WriteLine(listItems[i].item.Location.X);
                    writer.WriteLine(listItems[i].item.Location.Y);
                }
            }

        }
    }
}
