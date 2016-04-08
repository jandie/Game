using DeGame.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;
using DeGame.Properties;

namespace DeGame
{
    public partial class frmScreen : Form
    {
        private World _world;
        private int _mouseX;
        private int _mouseY;
        private int windowX = 1400;
        private int windowY = 900;
        private Graphics _gr;
        
        public frmScreen()
        {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, pnlScreen, new object[] { true });

            _gr = pnlScreen.CreateGraphics();
            _world = new World(_gr, _world, windowX, windowY);
            //LoadScore();

            _world.LoadMap();
            
            tmrMoveBots.Interval = 800;
            tmrMoveBots.Start();
            tmrMovePlayer.Interval = 100;
            tmrMovePlayer.Start();

            this.Size = new Size(windowX, windowY);
            RefreshStats();
        }

        void RefreshStats()
        {
            this.Text = Resources.frmScreen_RefreshStats_The_Game___Level__ + _world.CurrentLevel;
        }

        private void frmScreen_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pnlScreen_Paint(object sender, PaintEventArgs e)
        {
            _world.Draw();
        }

        private void frmScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            //world.KeyDown(e.KeyChar, _mouseX, mouseY);
            //RefreshStats();
        }

        private void tmrMoveBots_Tick(object sender, EventArgs e)
        {
            _world.MoveAllBots();
            _world.Draw();
        }

        private void frmScreen_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseX = e.X;
            _mouseY = e.Y;
        }

        private void frmScreen_SizeChanged(object sender, EventArgs e)
        {
            frmScreen screen = sender as frmScreen;

            if (screen != null) _world.UpdateWindow(screen.Width, screen.Height);
        }

        private void tmrMovePlayer_Tick(object sender, EventArgs e)
        {
            _world.MovePlayer();
            _world.MoveAllBullets();
        }

        /// <summary>
        /// Saves the current score.
        /// </summary>
        //void SaveScore()
        //{
        //    DataContractSerializer dcs = new DataContractSerializer(typeof(World));

        //    using (FileStream f = new FileStream("file.xml",
        //           FileMode.Create, FileAccess.Write))
        //    {
        //        dcs.WriteObject(f, _world);            // Wegschrijven

        //        f.Close();
        //    }
        //}

        /// <summary>
        /// Loads last score
        /// </summary>
        //void LoadScore()
        //{
        //    DataContractSerializer dcs = new DataContractSerializer(typeof(World));

        //    try
        //    {
        //        using (FileStream f = new FileStream("file.xml",
        //                          FileMode.Open, FileAccess.Read))
        //        {
        //            World tempWorld = dcs.ReadObject(f) as World; // Uitlezen

        //            if (tempWorld != null) _world.CurrentScore = tempWorld.CurrentScore;

        //            f.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string error = e.ToString();
        //    }
            
            
        //}

        /// <summary>
        /// saves score when form is being closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveScore();
        }

        private void frmScreen_KeyDown(object sender, KeyEventArgs e)
        {
            _world.KeyDown(e.KeyData.ToString(), _mouseX, _mouseY);
            RefreshStats();
        }

        private void frmScreen_KeyUp(object sender, KeyEventArgs e)
        {
            _world.KeyUp(e.KeyData.ToString());
            RefreshStats();
        }

        
    }
}
