
namespace Demo_Battle_Field
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.timerGame = new System.Windows.Forms.Timer(this.components);
            this.map = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarLevel = new System.Windows.Forms.ProgressBar();
            this.progressBarShield = new System.Windows.Forms.ProgressBar();
            this.progressBarHealth = new System.Windows.Forms.ProgressBar();
            this.map.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerGame
            // 
            this.timerGame.Interval = 200;
            this.timerGame.Tick += new System.EventHandler(this.ReStartGame);
            // 
            // map
            // 
            this.map.BackgroundImage = global::Demo_Battle_Field.Properties.Resources.BG;
            this.map.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.map.Controls.Add(this.label1);
            this.map.Controls.Add(this.progressBarLevel);
            this.map.Controls.Add(this.progressBarShield);
            this.map.Controls.Add(this.progressBarHealth);
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(974, 740);
            this.map.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(399, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pause";
            // 
            // progressBarLevel
            // 
            this.progressBarLevel.BackColor = System.Drawing.Color.Gainsboro;
            this.progressBarLevel.ForeColor = System.Drawing.Color.Green;
            this.progressBarLevel.Location = new System.Drawing.Point(118, 75);
            this.progressBarLevel.Maximum = 10;
            this.progressBarLevel.Name = "progressBarLevel";
            this.progressBarLevel.Size = new System.Drawing.Size(46, 17);
            this.progressBarLevel.TabIndex = 3;
            // 
            // progressBarShield
            // 
            this.progressBarShield.BackColor = System.Drawing.Color.Gainsboro;
            this.progressBarShield.ForeColor = System.Drawing.Color.Blue;
            this.progressBarShield.Location = new System.Drawing.Point(127, 53);
            this.progressBarShield.Name = "progressBarShield";
            this.progressBarShield.Size = new System.Drawing.Size(232, 13);
            this.progressBarShield.TabIndex = 1;
            // 
            // progressBarHealth
            // 
            this.progressBarHealth.BackColor = System.Drawing.Color.Gainsboro;
            this.progressBarHealth.ForeColor = System.Drawing.Color.Red;
            this.progressBarHealth.Location = new System.Drawing.Point(127, 26);
            this.progressBarHealth.Name = "progressBarHealth";
            this.progressBarHealth.Size = new System.Drawing.Size(250, 13);
            this.progressBarHealth.TabIndex = 0;
            this.progressBarHealth.Value = 100;
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 740);
            this.ControlBox = false;
            this.Controls.Add(this.map);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battle Field";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyIsDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyIsUp);
            this.map.ResumeLayout(false);
            this.map.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerGame;
        private System.Windows.Forms.Panel map;
        private System.Windows.Forms.ProgressBar progressBarShield;
        private System.Windows.Forms.ProgressBar progressBarHealth;
        private System.Windows.Forms.ProgressBar progressBarLevel;
        private System.Windows.Forms.Label label1;
    }
}

