using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeGame.Enums;
using DeGame.Classes;
using System.Drawing;

namespace DeGame
{
    public class Map
    {
        private Dictionary<string, Image> images;

        private List<Cel> cells;
        private List<Bot> bots;
        /// <summary>
        /// size of one cell in pixels
        /// </summary>
        private int cellSize;
        /// <summary>
        /// amount of cells in height and width
        /// </summary>
        private int mapSize; //amount of
        private int amountOfBots;

        private Player player;
        private static readonly Random random = new Random();

        public int OverheadX { get; private set; }
        public int OverheadY { get; private set; }
        public List<Bot> Bots { get { return bots; } }

        public Map(int cellSize, int mapSize, int amountOfBots)
        {
            cells = new List<Cel>();
            bots = new List<Bot>();
            player = new Player();

            images = new Dictionary<string, Image>();
            images.Add("Grass", Properties.Resources.grass);
            images.Add("Wall", Properties.Resources.wall);

            this.cellSize = cellSize;
            this.mapSize = mapSize;
            this.amountOfBots = amountOfBots;

            MakeDefaultMap();

            foreach (Cel cel in cells)
            {
                if (cel.GetTypeCel() == Enums.Object.StartPoint)
                {
                    player.Move(cel.GetX(), cel.GetY() + 1 - 1);
                }
            }
        }

        public void DrawCellsPlayerAndBots(Graphics gr, int windowX, int windowY)
        {
            Image image;
            Point plaats;

            List<Cel> cellsToDraw = GetCells(windowX, windowY);
            List<Bot> botsToDraw = GetBots(windowX, windowY);


            foreach (Cel cel in cellsToDraw)
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

                plaats = new Point(cel.GetX() - OverheadX, cel.GetY() - OverheadY);
                gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);

                if (cel.GetX() == player.LocationX && cel.GetY() == player.LocationY)
                {
                    image = DeGame.Properties.Resources.player;
                    plaats = new Point(player.LocationX - OverheadX, player.LocationY - OverheadY);

                    gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);
                }

                foreach (Bot bot in botsToDraw)
                {
                    if (bot.LocationX == cel.GetX() && bot.LocationY == cel.GetY())
                    {
                        image = DeGame.Properties.Resources.bot;
                        plaats = new Point(bot.LocationX - OverheadX, bot.LocationY - OverheadY);

                        gr.DrawImage(image, plaats.X, plaats.Y, 100, 100);
                    }

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
            }
        }

        public void ResetPlayer()
        {
            player = new Player();
            foreach (Cel cel in cells)
            {
                if (cel.GetTypeCel() == Enums.Object.StartPoint)
                {
                    player.Move(cel.GetX(), cel.GetY());
                    break;
                }
            }
        }

        public void ResetBots()
        {
            bots = new List<Bot>();
            GenerateBots();
        }

        public void ResetPowerUps()
        {
            foreach (Cel cel in cells)
            {
                cel.ReSetPowerUp();
            }
            GeneratePowerUps();
        }

        public void PlaceObject(Enums.Object typeCel, int x, int y)
        {
            int celX = x + OverheadX;
            int celY = y + OverheadY;
            celX = Convert.ToInt32(Math.Floor(Convert.ToDecimal(celX) / Convert.ToDecimal(cellSize))) * cellSize;
            celY = Convert.ToInt32(Math.Floor(Convert.ToDecimal(celY) / Convert.ToDecimal(cellSize))) * cellSize;
            if (typeCel == Enums.Object.StartPoint || typeCel == Enums.Object.Destination)
            {
                foreach (Cel cel in cells)
                {
                    if (cel.GetTypeCel() == typeCel)
                    {
                        GetSingleCell(cel.GetX(), cel.GetY()).SetObject(Enums.Object.Grass);
                    }
                }
            }
            GetSingleCell(celX, celY).SetObject(typeCel);
        }

        public void MoveBots()
        {
            int direction;
            int botX = 0;
            int botY = 0;
            bool botMove = true;
            for (int a = 0; a < amountOfBots - 1; a++)
            {
                botMove = true;
                if (DetectPlayer(bots[a].LocationX, bots[a].LocationY - cellSize))
                {
                    botY = bots[a].LocationY - cellSize;
                    botX = bots[a].LocationX;
                }
                else if (DetectPlayer(bots[a].LocationX,bots[a].LocationY + cellSize))
                {
                    botY = bots[a].LocationY + cellSize;
                    botX = bots[a].LocationX;
                }
                else if (DetectPlayer(bots[a].LocationX - cellSize, bots[a].LocationY))
                {
                    botY = bots[a].LocationY;
                    botX = bots[a].LocationX - cellSize;
                }
                else if (DetectPlayer( bots[a].LocationX + cellSize,bots[a].LocationY))
                {
                    botY = bots[a].LocationY;
                    botX = bots[a].LocationX + cellSize;
                }
                else
                {
                    direction = random.Next(1, 5);
                    switch (direction)
                    {
                        case 1:
                            botY = bots[a].LocationY - cellSize;
                            botX = bots[a].LocationX;
                            break;
                        case 2:
                            botY = bots[a].LocationY;
                            botX = bots[a].LocationX - cellSize;
                            break;
                        case 3:
                            botY = bots[a].LocationY + cellSize;
                            botX = bots[a].LocationX;
                            break;
                        case 4:
                            botY = bots[a].LocationY;
                            botX = bots[a].LocationX + cellSize;
                            break;
                        default:
                            botY = bots[a].LocationY - cellSize;
                            botX = bots[a].LocationX;
                            break;
                    }
                }
                if (botX > cellSize * (mapSize - 1) || botX < 0 || botY > cellSize * (mapSize - 1) || botY < 0)
                {
                    botMove = false;
                }
                else if (GetSingleCell(botX, botY).GetTypeCel() != Enums.Object.Grass || DetectBot(botX, botY))
                {
                    botMove = false;
                }
                if (botMove)
                {
                    bots[a].Move(botX, botY);
                }
            }
        }

        public Enums.PlayerStatus CheckBotOnPlayer()
        {
            foreach (Bot bot in bots)
            {
                if (bot.LocationX == player.LocationX && bot.LocationY == player.LocationY && bot.IsKilled() == false)
                {
                    // check if player has star powerup
                    if (player.powerUp == Enums.TypePowerUp.MarioStar)
                    {
                        bot.Kill();
                        return PlayerStatus.Alive;
                    }
                    return player.RemoveHitpoints(bot.Strength);
                }
            }

            foreach (Cel cel in cells)
            {
                if (cel.GetX() == player.LocationX && cel.GetY() == player.LocationY && cel.GetTypeCel() == Enums.Object.Destination)
                {
                    return PlayerStatus.Win;
                }
            }

            return PlayerStatus.Alive;
        }

        public Cel GetSingleCell(int x, int y)
        {
            foreach (Cel cel in cells)
            {
                if (cel.GetX() == x && cel.GetY() == y)
                {
                    return cel;
                }
            }

            return null;
        }

        public void MovePlayer(bool[] directionKeys)
        {
            bool playerMove = true;
            int newX = player.LocationX;
            int newY = player.LocationY;

            if (directionKeys[0]) { newY -= cellSize; }

            if (directionKeys[1]) { newX -= cellSize; }

            if (directionKeys[2]) { newY += cellSize; }

            if (directionKeys[3]) { newX += cellSize; }

            foreach (Cel cel in cells)
            {
                if (cel.GetX() == newX && cel.GetY() == newY)
                {
                    if (cel.GetTypeCel() == Enums.Object.Wall)
                    {
                        playerMove = false;
                    }
                    break;
                }
            }

            if (newX > cellSize * (mapSize - 1) || newX < 0 || newY > cellSize * (mapSize - 1) || newY < 0)
            {
                playerMove = false;
            }

            if (playerMove)
            {
                player.Move(newX, newY);
                if (player.powerUp == Enums.TypePowerUp.None)
                {
                    player.powerUp = CheckPlayerOnPowerUp(newX, newY);
                }
                
            }
            
        }

        private Enums.TypePowerUp CheckPlayerOnPowerUp(int x, int y)
        {
            Cel cel = GetSingleCell(x, y);

            if (cel.GetPowerUp() != null)
            {
                if (cel.GetPowerUp().TypePowerUp != Enums.TypePowerUp.None)
                {
                    cel.GetPowerUp().PickedUp = true;
                    return cel.GetPowerUp().TypePowerUp;
                }
            }
            
            return Enums.TypePowerUp.None;
        }

        public Player GetPlayer()
        {
            return player;
        }
        public void CalculateOverhead(int windowX,int windowY)
        {
            if (player.LocationX > 500)
            {
                OverheadX = player.LocationX - 500;

                if (OverheadX + windowX > cellSize * mapSize)
                {
                    OverheadX = (cellSize * mapSize) - windowX;
                }
            }
            else {
                OverheadX = 0;
            }

            if (player.LocationY > 500)
            {
                OverheadY = player.LocationY - 500;
                if (OverheadY + windowY > cellSize * mapSize)
                {
                    OverheadY = (cellSize * mapSize) - windowY;
                }
            }
            else { OverheadY = 0; }
        }

        public List<Cel> GetCells(int windowX, int windowY)
        {
            List<Cel> windowCells = new List<Cel>();

            CalculateOverhead(windowX, windowY);

            foreach (Cel cel in cells)
            {
                if(cel.GetX() >= OverheadX && cel.GetX() <= windowX + cellSize + OverheadX && cel.GetY() >= OverheadY && cel.GetY() <= windowY + cellSize + OverheadY)
                {
                    windowCells.Add(cel);
                }
            }

            return windowCells;
        }

        public List<Bot> GetBots(int windowX, int windowY)
        {
            List<Bot> windowsBots = new List<Bot>();

            CalculateOverhead(windowX, windowY);

            foreach (Bot bot in bots)
            {
                if (bot.LocationX >= OverheadX && bot.LocationX <= windowX + cellSize + OverheadX && 
                    bot.LocationY >= OverheadY && bot.LocationY <= windowY + cellSize + OverheadY && 
                    bot.IsKilled() == false)
                {
                    windowsBots.Add(bot);
                }
            }

            return windowsBots;
        }

        public bool DetectBot(int x, int y)
        {
            foreach (Bot bot in bots)
            {
                if (bot.LocationX == x && bot.LocationY == y)
                {
                    return true;
                }
            }

            return false;
        }
        public bool DetectPlayer(int x, int y)
        {
            
            if (player.LocationX == x && player.LocationY == y)
            {
                return true;
            }
            
            return false;
        }
        public void MakeDefaultMap()
        {
            for (int i = 0; i < mapSize * mapSize; i++)
            {
                int y = Convert.ToInt32(Math.Floor(Convert.ToDecimal(i) / Convert.ToDecimal(mapSize)));
                int x = (i) - (y * mapSize);

                switch (i)
                {
                    default:
                        if (random.Next(0, 7) == 5)
                        {
                            cells.Add(new Cel(Enums.Object.Wall, x * cellSize, y * cellSize));
                        }
                        else
                        {
                            cells.Add(new Cel(Enums.Object.Grass, x * cellSize, y * cellSize));
                        }
                        break;
                }
            }

            cells[random.Next(1, mapSize * mapSize)].SetObject(Enums.Object.Destination);
            cells[random.Next(1, mapSize * mapSize)].SetObject(Enums.Object.StartPoint);

            GenerateBots();
            GeneratePowerUps();
        }

        private void GenerateBots()
        {
            int botX;
            int botY;

            for (int a = 0; a < amountOfBots - 1; a++)
            {
                do
                {
                    botX = random.Next(cellSize * mapSize);
                    botY = random.Next(cellSize * mapSize);

                    botX = Convert.ToInt32(Math.Floor(Convert.ToDecimal(botX) / Convert.ToDecimal(cellSize))) * cellSize;
                    botY = Convert.ToInt32(Math.Floor(Convert.ToDecimal(botY) / Convert.ToDecimal(cellSize))) * cellSize;
                } while (GetSingleCell(botX, botY).GetTypeCel() != Enums.Object.Grass || DetectBot(botX, botY));

                bots.Add(new Bot());
                bots[a].Move(botX, botY);
            }
        }

        private void GeneratePowerUps()
        {
            int amountOfPowerUps = random.Next(6);
            int powerUpCellIndex;

            Enums.TypePowerUp typePowerUp = Enums.TypePowerUp.MarioStar;

            for (int i = 0; i < amountOfPowerUps; i++)
            {
                do
                {
                    powerUpCellIndex = random.Next(mapSize * mapSize);
                }
                while (cells[powerUpCellIndex].GetTypeCel() != Enums.Object.Grass || cells[powerUpCellIndex].GetPowerUp() != null);

                cells[powerUpCellIndex].SetPowerUp(new PowerUp(typePowerUp));
            }
        }
    }
}