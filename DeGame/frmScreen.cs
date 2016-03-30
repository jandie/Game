using DeGame.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;

namespace DeGame
{
    public partial class frmScreen : Form
    {
        private World world;
        private int mouseX;
        private int mouseY;
        private int windowX = 1400;
        private int windowY = 900;
        private Graphics gr;
        
        public frmScreen()
        {
            InitializeComponent();
            
            gr = CreateGraphics();
            world = new World(gr, world, windowX, windowY);
            LoadScore();

            world.LoadMap();
            
            tmrMoveBots.Interval = 800;
            tmrMoveBots.Start();
            tmrMovePlayer.Interval = 1;
            tmrMovePlayer.Start();

            this.Size = new Size(windowX, windowY);
            RefreshStats();
        }

        void RefreshStats()
        {
            this.Text = "The Game - Level: " + world.CurrentLevel + " - Score: " + world.CurrentScore;
        }

        private void frmScreen_Paint(object sender, PaintEventArgs e)
        {
            world.Draw();
        }

        private void frmScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            world.KeyUpdate(e.KeyChar, mouseX, mouseY);
            RefreshStats();
        }

        private void tmrMoveBots_Tick(object sender, EventArgs e)
        {
            world.MoveAllBots();
            world.Draw();
        }

        private void frmScreen_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void frmScreen_SizeChanged(object sender, EventArgs e)
        {
            frmScreen screen = sender as frmScreen;

            world.UpdateWindow(screen.Width, screen.Height);
        }

        private void tmrMovePlayer_Tick(object sender, EventArgs e)
        {
            world.MovePlayer();
        }

        /// <summary>
        /// Saves the current score.
        /// </summary>
        void SaveScore()
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(World));

            using (FileStream f = new FileStream("file.xml",
                   FileMode.Create, FileAccess.Write))
            {
                dcs.WriteObject(f, world);            // Wegschrijven

                f.Close();
            }
        }

        /// <summary>
        /// Loads last score
        /// </summary>
        void LoadScore()
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(World));

            try
            {
                using (FileStream f = new FileStream("file.xml",
                                  FileMode.Open, FileAccess.Read))
                {
                    World tempWorld = dcs.ReadObject(f) as World; // Uitlezen

                    world.CurrentScore = tempWorld.CurrentScore;
                    
                    f.Close();
                }
            }
            catch { }
            
            
        }

        private void frmScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveScore();
        }
    }
}
