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
        private bool[] directionKeys;
        private Graphics gr;
        
        public frmScreen()
        {
            InitializeComponent();

            

            gr = CreateGraphics();
            directionKeys = new bool[4];
            world = new World();

            world.LoadMap();
            world.Draw(gr, windowX, windowY);

            tmrMoveBots.Interval = 800;
            tmrMoveBots.Start();
            tmrMovePlayer.Interval = 1;
            tmrMovePlayer.Start();

            this.Size = new Size(windowX, windowY);
            this.Text = "The Game - Level: " + world.CurrentLevel;
        }

        private void frmScreen_Paint(object sender, PaintEventArgs e)
        {
            world.Draw(gr, windowX, windowY);
            CheckPlayerAndBots();
        }

        private void frmScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            Player player = world.GetPlayer();
            bool otherThanDirection = false;

            switch (Convert.ToString(e.KeyChar).ToLower())
            {
                case "w":
                    directionKeys[0] = true;
                    break;
                case "a":
                    directionKeys[1] = true;
                    break;
                case "s":
                    directionKeys[2] = true;
                    break;
                case "d":
                    directionKeys[3] = true;
                    break;
                case "z":
                    world.PlaceObject(Enums.Object.Grass, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "x":
                    world.PlaceObject(Enums.Object.Wall, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "c":
                    world.PlaceObject(Enums.Object.StartPoint, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "v":
                    world.PlaceObject(Enums.Object.Destination, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "r":
                    world.Reset();
                    Refresh();
                    break;
            }

            if (otherThanDirection){
                world.Draw(gr, windowX, windowY);
                CheckPlayerAndBots();
            }
        }

        private void CheckPlayerAndBots()
        {
            Enums.PlayerStatus playerAlive = world.CheckPlayerPosition();

            switch (playerAlive)
            {
                case Enums.PlayerStatus.Dead:
                    world.Reset();
                    Refresh();
                    this.Text = "The Game - Level: " + world.CurrentLevel;
                    break;
                case Enums.PlayerStatus.Win:
                    world.NextLevel();
                    world.Reset();
                    Refresh();
                    this.Text = "The Game - Level: " + world.CurrentLevel;
                    break;
            }
        }

        private void tmrMoveBots_Tick(object sender, EventArgs e)
        {
            world.MoveAllBots();
            CheckPlayerAndBots();
            world.Draw(gr, windowX, windowY);
        }

        private void frmScreen_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void frmScreen_SizeChanged(object sender, EventArgs e)
        {
            frmScreen screen = sender as frmScreen;

            windowX = screen.Width;
            windowY = screen.Height;
        }

        private void tmrMovePlayer_Tick(object sender, EventArgs e)
        {
            bool refresh = false;

            for (int i = 0; i < directionKeys.Count(); i++)
            {
                if (directionKeys[i])
                {
                    refresh = true;
                }
            }

            if (refresh)
            {
                world.MovePlayer(directionKeys);

                for (int i = 0; i < directionKeys.Count(); i++)
                {
                    directionKeys[i] = false;
                    
                }
                world.Draw(gr, windowX, windowY);
                CheckPlayerAndBots();
            }
        }
    }
}
