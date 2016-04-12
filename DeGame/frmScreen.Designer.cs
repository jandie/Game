namespace DeGame
{
    partial class frmScreen
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
            this.tmrMoveBots = new System.Windows.Forms.Timer(this.components);
            this.tmrMovePlayer = new System.Windows.Forms.Timer(this.components);
            this.pnlScreen = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tmrMoveBots
            // 
            this.tmrMoveBots.Interval = 500;
            this.tmrMoveBots.Tick += new System.EventHandler(this.tmrMoveBots_Tick);
            // 
            // tmrMovePlayer
            // 
            this.tmrMovePlayer.Interval = 50;
            this.tmrMovePlayer.Tick += new System.EventHandler(this.tmrMovePlayer_Tick);
            // 
            // pnlScreen
            // 
            this.pnlScreen.Location = new System.Drawing.Point(0, 0);
            this.pnlScreen.Name = "pnlScreen";
            this.pnlScreen.Size = new System.Drawing.Size(1400, 900);
            this.pnlScreen.TabIndex = 0;
            this.pnlScreen.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlScreen_Paint);
            // 
            // frmScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 861);
            this.Controls.Add(this.pnlScreen);
            this.Name = "frmScreen";
            this.Text = "The Game";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScreen_FormClosing);
            this.SizeChanged += new System.EventHandler(this.frmScreen_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmScreen_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmScreen_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmScreen_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrMoveBots;
        private System.Windows.Forms.Timer tmrMovePlayer;
        private System.Windows.Forms.Panel pnlScreen;
    }
}

