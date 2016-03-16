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

        private Dictionary<string, Image> images;

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

            images = new Dictionary<string, Image>();
            images.Add("Grass", Properties.Resources.grass);
            images.Add("Wall", Properties.Resources.wall);

            gr = CreateGraphics();
            directionKeys = new bool[4];
            world = new World();
            world.LoadMap();
            DrawCellsPlayerAndBots();
            DrawPlayer();
            //MakeFullScreen();
            tmrMoveBots.Interval = 800;
            tmrMoveBots.Start();
            tmrMovePlayer.Interval = 1;
            tmrMovePlayer.Start();
            this.Size = new Size(windowX, windowY);
            this.Text = "The Game - Level: " + world.CurrentLevel;
        }

        private void frmScreen_Paint(object sender, PaintEventArgs e)
        {
            DrawCellsPlayerAndBots();
            DrawPlayer();
            CheckPlayerAndBots();
        }

        private void DrawCellsPlayerAndBots()
        {
            Image image;
            Point plaats;

            List<Cel> cells = world.GetDrawableCells(windowX , windowY);
            Player player = world.GetPlayer();
            List<Bot> bots = world.GetBots();
            
            foreach (Cel cel in cells)
            {
                switch (cel.GetTypeCel())
                {
                    case Enums.Object.Wall:
                        image = images["Wall"];
                        break;
                    case Enums.Object.Destination:
                        image = DeGame.Properties.Resources.destination;
                        break;
                    case Enums.Object.StartPoint:
                        image = DeGame.Properties.Resources.startpoint;
                        break;
                    default:
                    case Enums.Object.Grass:
                        image = images["Grass"];
                        break;
                }

                plaats = new Point(cel.GetX() - world.GetOverheadX(), cel.GetY() - world.GetOverheadY());
                gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);

                if (cel.GetX() == player.LocationX && cel.GetY() == player.LocationY)
                {
                    DrawPlayer();
                }

                if (cel.GetPowerUp() != null && cel.GetPowerUp().PickedUp == false)
                {
                    switch (cel.GetPowerUp().TypePowerUp)
                    {
                        case Enums.TypePowerUp.MarioStar:
                            image = DeGame.Properties.Resources.MarioStar;
                            gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);
                            break;
                    }
                }
                
                foreach (Bot bot in bots)
                {
                    if (cel.GetX() == bot.LocationX && cel.GetY() == bot.LocationY)
                    {
                        image = DeGame.Properties.Resources.bot;
                        plaats = new Point(bot.LocationX - world.GetOverheadX(), bot.LocationY - world.GetOverheadY());
                        gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);
                    }
                }
            }
        }

        private void DrawPlayer()
        {
            Image image = DeGame.Properties.Resources.player;
            Player player = world.GetPlayer();
            Point plaats = new Point(player.LocationX - world.GetOverheadX(), player.LocationY - world.GetOverheadY());
            
            gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);
        }

        private void frmScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            Player player;
            player = world.GetPlayer();
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
                DrawCellsPlayerAndBots();
                DrawPlayer();
                CheckPlayerAndBots();
            }
        }

        private void CheckPlayerAndBots()
        {
            world.ClearCellsToUpdate();

            Enums.PlayerStatus playerAlive;
            playerAlive = world.CheckPlayerPosition();
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
            DrawCellsPlayerAndBots();
        }

        private void frmScreen_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void frmScreen_SizeChanged(object sender, EventArgs e)
        {
            frmScreen screen;
            screen = sender as frmScreen;
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
                DrawCellsPlayerAndBots();
                DrawPlayer();
                CheckPlayerAndBots();
            }
            
        }
    }
}
