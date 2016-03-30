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
            world = new World(gr, windowX, windowY);

            world.LoadMap();
            
            tmrMoveBots.Interval = 800;
            tmrMoveBots.Start();
            tmrMovePlayer.Interval = 1;
            tmrMovePlayer.Start();

            this.Size = new Size(windowX, windowY);
            this.Text = "The Game - Level: " + world.CurrentLevel + " - Score: " + world.CurrentScore;
        }

        private void frmScreen_Paint(object sender, PaintEventArgs e)
        {
            world.Draw();
        }

        private void frmScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            world.KeyUpdate(e.KeyChar, mouseX, mouseY);
            this.Text = "The Game - Level: " + world.CurrentLevel + " - Score: " + world.CurrentScore;
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
    }
}
