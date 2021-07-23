using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo_Battle_Field
{
    public partial class FrmHighScore : Form
    {
        List<int> list = new List<int>();
        public FrmHighScore()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            list = getData();
            label1.Text = "1st. " + list[3].ToString();
            label2.Text = "2nd. " + list[2].ToString();
            label3.Text = "3rd. " + list[1].ToString();
            label4.Text = "4th. " + list[0].ToString();
        }

        private List<int> getData()
        {
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
            list.Sort();
            return list;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
