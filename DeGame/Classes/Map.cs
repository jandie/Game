﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DeGame.Enums;
using Object = DeGame.Enums.Object;

namespace DeGame.Classes
{
    public class Map
    {
        private readonly Dictionary<string, Image> _images;
        
        private List<Cel> _cells;
        private List<Bot> _bots;
        private List<Bullet> _bullets; 
        /// <summary>
        /// size of one cell in pixels
        /// </summary>
        
        private Player _player;
        private static readonly Random _Random = new Random();
        private int _framesDrawn;

        public int OverheadX { get; private set; }
        public int OverheadY { get; private set; }
        public List<Bot> Bots { get { return _bots; } }
        public int CellSize { get; set; }
        /// <summary>
        /// amount of _cells in height and width
        /// </summary>
        public int MapSize { get; set; }
        public int AmountOfBots { get; set; }
        public int Score { get; private set; }

        public Map(int cellSize, int mapSize, int amountOfBots, bool loadedMap)
        {
            _cells = new List<Cel>();
            _bots = new List<Bot>();
            _player = new Player();
            _bullets = new List<Bullet>();

            _images = new Dictionary<string, Image>
            {
                {"Grass", Properties.Resources.grass},
                {"Wall", Properties.Resources.wall}
            };

            this.CellSize = cellSize;
            this.MapSize = mapSize;
            this.AmountOfBots = amountOfBots;
            _framesDrawn = 0;
            _framesDrawn = 0;

            if (loadedMap == false)
            {
                MakeDefaultMap();
                MovePlayerToBegin();
            }
            
        }

        public void MovePlayerToBegin()
        {
            foreach (Cel cel in _cells)
            {
                if (cel.GetTypeCel() == Enums.Object.StartPoint)
                {
                    _player.Move(cel.GetX(), cel.GetY() + 1 - 1);
                }
            }
        }


        /// <summary>
        /// Draws all the entities and objects on the main screen.
        /// </summary>
        /// <param name="gr">Graphics object of the main screen.</param>
        /// <param name="windowX">Width of the main screen.</param>
        /// <param name="windowY">Heigth of the main screen.</param>
        public void DrawCellsAndEntities(Graphics gr, int windowX, int windowY)
        {
            List<Cel> cellsToDraw = GetCells(windowX, windowY);
            List<Bot> botsToDraw = GetBots(windowX, windowY);
            Image image;
            Point plaats;
            SizeF size;
            Brush brush = new SolidBrush(Color.Gold);
            Brush brushBot = new SolidBrush(Color.Black);
            Font font = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold);

            Bitmap screen = new Bitmap(windowX, windowY);
            Graphics screenGraphics = Graphics.FromImage(screen);

            foreach (Cel cel in cellsToDraw)
            {
                switch (cel.GetTypeCel())
                {
                    case Enums.Object.Wall:
                        image = _images["Wall"];
                        break;
                    case Enums.Object.Destination:
                        image = DeGame.Properties.Resources.destination;
                        break;
                    case Enums.Object.StartPoint:
                        image = DeGame.Properties.Resources.startpoint;
                        break;
                    default:
                    case Enums.Object.Grass:
                        image = _images["Grass"];
                        break;
                }

                plaats = new Point(cel.GetX() - OverheadX, cel.GetY() - OverheadY);
                screenGraphics.DrawImage(image, plaats.X, plaats.Y, CellSize, CellSize);

                //Draw _player
                if (cel.GetX() == _player.LocationX && cel.GetY() == _player.LocationY)
                {
                    switch (_player.Direction)
                    {
                        case Direction.Right:
                            image = DeGame.Properties.Resources.player_right;
                            break;
                        case Direction.Left:
                            image = DeGame.Properties.Resources.player_left;
                            break;
                        case Direction.Up:
                            image = DeGame.Properties.Resources.player_up;
                            break;
                        case Direction.Down:
                            image = DeGame.Properties.Resources.player_down;
                            break;
                    }

                    plaats = new Point(_player.LocationX - OverheadX, _player.LocationY - OverheadY);
                    size = screenGraphics.MeasureString(_player.Hitpoints.ToString(), font);

                    screenGraphics.DrawImage(image, plaats.X, plaats.Y, CellSize, CellSize);
                    screenGraphics.FillRectangle(brushBot, plaats.X, plaats.Y - size.Height, size.Width, size.Height);
                    screenGraphics.DrawString(_player.Hitpoints.ToString(), font, brush, plaats.X, plaats.Y - size.Height);
                }

                //Draw _bots

                if (cel.GetPowerUp() != null && cel.GetPowerUp().PickedUp == false)
                {
                    switch (cel.GetPowerUp().TypePowerUp)
                    {
                        case Enums.TypePowerUp.MarioStar:
                            image = DeGame.Properties.Resources.MarioStar;
                            screenGraphics.DrawImage(image, plaats.X, plaats.Y, CellSize, CellSize);
                            break;
                    }
                }
            }

            foreach (Bot bot in botsToDraw)
            {
                image = DeGame.Properties.Resources.bot;
                plaats = new Point(bot.LocationX - OverheadX, bot.LocationY - OverheadY);

                size = screenGraphics.MeasureString(bot.Hitpoints.ToString(), font);
                
                screenGraphics.FillRectangle(brushBot, plaats.X, plaats.Y - size.Height, size.Width, size.Height);

                screenGraphics.DrawImage(image, plaats.X, plaats.Y, CellSize, CellSize);
                screenGraphics.DrawString(bot.Hitpoints.ToString(), font, brush, plaats.X, plaats.Y - size.Height);
            }

            foreach (Bullet bullet in _bullets.Where(b => b.LocationX >= OverheadX && b.LocationY >= OverheadY && b.LocationX <= OverheadX + windowX && b.LocationY <= windowY + OverheadY && !b.Destroyed))
            {
                screenGraphics.FillEllipse(brush, bullet.LocationX - OverheadX + (CellSize / 2), bullet.LocationY - OverheadY + (CellSize / 2), CellSize / 4, CellSize / 4);
            }

            size = screenGraphics.MeasureString("Score :" + Score, font);

            screenGraphics.FillRectangle(brushBot, 50, 50, size.Width, size.Height);

            screenGraphics.DrawString("Score :" + Score, font, brush, 50, 50);

            gr.DrawImage(screen, 0, 0, windowX, windowY);

            _framesDrawn++;
            if (_framesDrawn > 10)
            {
                System.GC.Collect();
                _framesDrawn = 0;
            }
        }

        public void SetCells(List<Cel> cells)
        {
            this._cells = cells;

            GenerateBots();
            GeneratePowerUps();
            MovePlayerToBegin();
        }

        public List<Cel> GetAllCells()
        {
            return _cells;
        }

        /// <summary>
        /// Resets the _player
        /// </summary>
        public void ResetPlayer()
        {
            _player = new Player();
            foreach (Cel cel in _cells)
            {
                if (cel.GetTypeCel() == Enums.Object.StartPoint)
                {
                    _player.Move(cel.GetX(), cel.GetY());
                    break;
                }
            }
        }

        /// <summary>
        /// Resets all _bots.
        /// </summary>
        public void ResetBots()
        {
            _bots.Clear();
            GenerateBots();
        }

        /// <summary>
        /// Resets all powerUps
        /// </summary>
        public void ResetPowerUps()
        {
            foreach (Cel cel in _cells)
            {
                cel.ReSetPowerUp();
            }
            GeneratePowerUps();
        }

        /// <summary>
        /// Replaces the object of a cel.
        /// </summary>
        /// <param name="typeCel">The new object of the cell</param>
        /// <param name="x">The x coördinate of the cell</param>
        /// <param name="y">The y coördinate of the cell</param>
        public void PlaceObject(Enums.Object typeCel, int x, int y)
        {
            int celX = x + OverheadX;
            int celY = y + OverheadY;
            celX = Convert.ToInt32(Math.Floor(Convert.ToDecimal(celX)/Convert.ToDecimal(CellSize)))*CellSize;
            celY = Convert.ToInt32(Math.Floor(Convert.ToDecimal(celY)/Convert.ToDecimal(CellSize)))*CellSize;
            if (typeCel == Enums.Object.StartPoint || typeCel == Enums.Object.Destination)
            {
                foreach (Cel cel in _cells)
                {
                    if (cel.GetTypeCel() == typeCel)
                    {
                        GetSingleCell(cel.GetX(), cel.GetY()).SetObject(Enums.Object.Grass);
                    }
                }
            }
            GetSingleCell(celX, celY).SetObject(typeCel);
        }

        /// <summary>
        /// Moves all the _bots to the _player if the _player is in a cel next to the bot.
        /// If _player is not in a cel next to the bot, the bot moves randomly.
        /// </summary>
        public void MoveBots()
        {
            int direction;
            int botX = 0;
            int botY = 0;
            bool botMove = true;

            for (int a = 0; a < AmountOfBots - 1; a++)
            {
                botMove = true;
                if (DetectPlayer(_bots[a].LocationX, _bots[a].LocationY - CellSize))
                {
                    botY = _bots[a].LocationY - CellSize;
                    botX = _bots[a].LocationX;
                }
                else if (DetectPlayer(_bots[a].LocationX, _bots[a].LocationY + CellSize))
                {
                    botY = _bots[a].LocationY + CellSize;
                    botX = _bots[a].LocationX;
                }
                else if (DetectPlayer(_bots[a].LocationX - CellSize, _bots[a].LocationY))
                {
                    botY = _bots[a].LocationY;
                    botX = _bots[a].LocationX - CellSize;
                }
                else if (DetectPlayer(_bots[a].LocationX + CellSize, _bots[a].LocationY))
                {
                    botY = _bots[a].LocationY;
                    botX = _bots[a].LocationX + CellSize;
                }
                else
                {
                    direction = _Random.Next(1, 5);
                    switch (direction)
                    {
                        case 1:
                            botY = _bots[a].LocationY - CellSize;
                            botX = _bots[a].LocationX;
                            break;
                        case 2:
                            botY = _bots[a].LocationY;
                            botX = _bots[a].LocationX - CellSize;
                            break;
                        case 3:
                            botY = _bots[a].LocationY + CellSize;
                            botX = _bots[a].LocationX;
                            break;
                        case 4:
                            botY = _bots[a].LocationY;
                            botX = _bots[a].LocationX + CellSize;
                            break;
                        default:
                            botY = _bots[a].LocationY - CellSize;
                            botX = _bots[a].LocationX;
                            break;
                    }
                }

                Cel cel = GetSingleCell(botX, botY);

                if (botX > CellSize*(MapSize - 1) || botX < 0 || botY > CellSize*(MapSize - 1) || botY < 0)
                {
                    botMove = false;
                }
                else if (cel.GetTypeCel() != Enums.Object.Grass || DetectBot(botX, botY))
                {
                    botMove = false;
                }
                if (botMove)
                {
                    _bots[a].Move(cel.GetX(), cel.GetY());
                }
            }
        }

        /// <summary>
        /// Checks if a bot is in the same cell as a _player.
        /// </summary>
        /// <returns>Returns the status of a _player</returns>
        public Enums.PlayerStatus CheckBotOnPlayer()
        {
            PlayerStatus status;

            foreach (Bot bot in _bots.Where(bot => bot.LocationX == _player.LocationX && bot.LocationY == _player.LocationY && bot.IsKilled() == false))
            {
                if (_player.PowerUp != Enums.TypePowerUp.MarioStar)
                {
                    status = _player.RemoveHitpoints(bot.Strength);

                    if (status == PlayerStatus.Dead) Score -= 500;

                    return status;
                }

                bot.Kill();
                Score += 500;

                return PlayerStatus.Alive;
            }

            status = _cells.Any(cel => cel.GetX() == _player.LocationX && cel.GetY() == _player.LocationY && cel.GetTypeCel() == Enums.Object.Destination)
                ? PlayerStatus.Win : PlayerStatus.Alive;

            if (status == PlayerStatus.Win)
            {
                Score += 1000;
            }

            return status;
        }

        /// <summary>
        /// Searches for a cell with coördinates.
        /// </summary>
        /// <param name="x">X coördinate.</param>
        /// <param name="y">Y coördinate.</param>
        /// <returns>Cel found or null</returns>
        public Cel GetSingleCell(int x, int y)
        {
            return _cells.FirstOrDefault(cel => cel.GetX() == x && cel.GetY() == y);
        }

        /// <summary>
        /// Moves _player
        /// </summary>
        /// <param name="directionKeys">Direction the _player want to go.</param>
        public void MovePlayer(bool[] directionKeys)
        {
            bool playerMove = true;
            int newX = _player.LocationX;
            int newY = _player.LocationY;

            if (directionKeys[0])
            {
                newY -= CellSize;
                _player.Direction = Direction.Up;
            }

            if (directionKeys[1])
            {
                newX -= CellSize;
                _player.Direction = Direction.Left;
            }

            if (directionKeys[2])
            {
                newY += CellSize;
                _player.Direction = Direction.Down;
            }

            if (directionKeys[3])
            {
                newX += CellSize;
                _player.Direction = Direction.Right;
            }

            foreach (Cel cel in _cells.Where(cel => cel.GetX() == newX && cel.GetY() == newY))
            {
                if (cel.GetTypeCel() == Enums.Object.Wall)
                {
                    playerMove = false;
                }
                break;
            }

            if (newX > CellSize*(MapSize - 1) || newX < 0 || newY > CellSize*(MapSize - 1) || newY < 0)
            {
                playerMove = false;
            }

            if (!playerMove) return;

            _player.Move(newX, newY);

            if (_player.PowerUp == Enums.TypePowerUp.None)
            {
                _player.PowerUp = CheckPlayerOnPowerUp(newX, newY);
            }
        }

        /// <summary>
        /// Checks if _player is in the same position as a powerup.
        /// </summary>
        /// <param name="x">X position of _player.</param>
        /// <param name="y">Y posiiton of _player.</param>
        /// <returns>What type of powerup the _player stays on.</returns>
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

        /// <summary>
        /// Gets _player.
        /// </summary>
        /// <returns>The _player.</returns>
        public Player GetPlayer()
        {
            return _player;
        }

        /// <summary>
        /// Calculates if the screen needs to be adjusted in order for the _player to remain visible on screen.
        /// </summary>
        /// <param name="windowX">Width of window</param>
        /// <param name="windowY">Heigth of window</param>
        public void CalculateOverhead(int windowX, int windowY)
        {
            if (_player.LocationX > 500)
            {
                OverheadX = _player.LocationX - 500;

                if (OverheadX + windowX > CellSize*MapSize)
                {
                    OverheadX = (CellSize*MapSize) - windowX;
                }
            }
            else
            {
                OverheadX = 0;
            }

            if (_player.LocationY > 500)
            {
                OverheadY = _player.LocationY - 500;
                if (OverheadY + windowY > CellSize*MapSize)
                {
                    OverheadY = (CellSize*MapSize) - windowY;
                }
            }
            else
            {
                OverheadY = 0;
            }
        }

        /// <summary>
        /// Gets the _cells that need to be shown on the screen.
        /// </summary>
        /// <param name="windowX">Width of window</param>
        /// <param name="windowY">Heigth of window</param>
        /// <returns></returns>
        public List<Cel> GetCells(int windowX, int windowY)
        {
            CalculateOverhead(windowX, windowY);

            return _cells.Where(cel => 
                cel.GetX() >= OverheadX && 
                cel.GetX() <= windowX + CellSize + OverheadX && 
                cel.GetY() >= OverheadY && 
                cel.GetY() <= windowY + CellSize + OverheadY)
                .ToList();
        }

        /// <summary>
        /// Gets _bots inside window
        /// </summary>
        /// <param name="windowX"></param>
        /// <param name="windowY"></param>
        /// <returns>List with _bots in window.</returns>
        public List<Bot> GetBots(int windowX, int windowY)
        {
            CalculateOverhead(windowX, windowY);

            return _bots.Where(bot => 
                bot.LocationX >= OverheadX && 
                bot.LocationX <= windowX + CellSize + OverheadX && 
                bot.LocationY >= OverheadY && 
                bot.LocationY <= windowY + CellSize + OverheadY && 
                bot.IsKilled() == false)
                .ToList();
        }

        /// <summary>
        /// Checks where the _bots are.
        /// </summary>
        /// <param name="x">X coördinate</param>
        /// <param name="y">Y coördinate</param>
        /// <returns>If a bot was found on this position</returns>
        public bool DetectBot(int x, int y)
        {
            return _bots.Any(bot => bot.LocationX == x && bot.LocationY == y);
        }

        /// <summary>
        /// Checks where the _player is.
        /// </summary>
        /// <param name="x">X coördinate</param>
        /// <param name="y">Y coördinate</param>
        /// <returns>If the _player was found on this position</returns>
        public bool DetectPlayer(int x, int y)
        {
            return _player.LocationX == x && _player.LocationY == y;
        }

        /// <summary>
        /// Generates a new random map.
        /// </summary>
        public void MakeDefaultMap()
        {
            for (int i = 0; i < MapSize*MapSize; i++)
            {
                int y = Convert.ToInt32(Math.Floor(Convert.ToDecimal(i)/Convert.ToDecimal(MapSize)));
                int x = (i) - (y*MapSize);

                switch (i)
                {
                    default:
                        if (_Random.Next(0, 2) == 1)
                        {
                            _cells.Add(new Cel(Enums.Object.Wall, x*CellSize, y*CellSize));
                        }
                        else
                        {
                            _cells.Add(new Cel(Enums.Object.Grass, x*CellSize, y*CellSize));
                        }
                        break;
                }
            }

            _cells[_Random.Next(1, MapSize*MapSize)].SetObject(Enums.Object.Destination);
            _cells[_Random.Next(1, MapSize*MapSize)].SetObject(Enums.Object.StartPoint);

            GenerateBots();
            GeneratePowerUps();
        }

        /// <summary>
        /// Generates _bots randomly.
        /// </summary>
        private void GenerateBots()
        {
            Cel cel;
            int botX;
            int botY;

            for (int a = 0; a < AmountOfBots - 1; a++)
            {
                do
                {
                    botX = _Random.Next(CellSize*MapSize);
                    botY = _Random.Next(CellSize*MapSize);

                    botX = Convert.ToInt32(Math.Floor(Convert.ToDecimal(botX)/Convert.ToDecimal(CellSize)))*CellSize;
                    botY = Convert.ToInt32(Math.Floor(Convert.ToDecimal(botY)/Convert.ToDecimal(CellSize)))*CellSize;
                    cel = GetSingleCell(botX, botY);
                } while (cel.GetTypeCel() != Enums.Object.Grass || DetectBot(botX, botY));

                _bots.Add(new Bot());
                _bots[a].Move(cel.GetX(), cel.GetY());
            }
        }

        /// <summary>
        /// Generates powerups randomly
        /// </summary>
        private void GeneratePowerUps()
        {
            int amountOfPowerUps = _Random.Next(6);
            int powerUpCellIndex;

            Enums.TypePowerUp typePowerUp = Enums.TypePowerUp.MarioStar;

            for (int i = 0; i < amountOfPowerUps; i++)
            {
                do
                {
                    powerUpCellIndex = _Random.Next(MapSize*MapSize);
                } while (_cells[powerUpCellIndex].GetTypeCel() != Enums.Object.Grass || _cells[powerUpCellIndex].GetPowerUp() != null);

                _cells[powerUpCellIndex].SetPowerUp(new PowerUp(typePowerUp));
            }
        }

        public void PlayerShoots()
        {
            _bullets.Add( new Bullet(_player.Direction, _player.LocationX, _player.LocationY, CellSize));

            CheckBulletsCollision();
        }

        private void CheckBulletsCollision()
        {
            foreach (Bullet bullet in _bullets.Where(bullet => !bullet.Destroyed))
            {
                foreach (Cel cel in _cells.Where(cel => cel.GetX() == bullet.LocationX && cel.GetY() == bullet.LocationY && cel.GetTypeCel() == Object.Wall))
                {
                    cel.SetObject(Object.Grass);
                    bullet.Destroy();
                }

                foreach (Bot bot in _bots.Where(bot =>bot.LocationX == bullet.LocationX && bot.LocationY == bullet.LocationY  && bot.IsKilled() == false))
                {
                    Score += bot.Damage(bullet.ImpactStrength);
                    bullet.Destroy();
                }

                if (bullet.LocationX < 0 || bullet.LocationX > CellSize*MapSize || bullet.LocationY < 0 ||
                    bullet.LocationY > MapSize*CellSize)
                {
                    bullet.Destroy();
                }
            }
        }

        public void MoveBullets()
        {
            foreach (Bullet bullet in _bullets)
            {
                int x = bullet.LocationX;
                int y = bullet.LocationY;

                switch (bullet.Direction)
                {
                    case Direction.Up:
                        y -= CellSize;
                        break;
                    case Direction.Down:
                        y += CellSize;
                        break;
                    case Direction.Left:
                        x -= CellSize;
                        break;
                    case Direction.Right:
                        x += CellSize;
                        break;
                }

                bullet.Move(x, y);
            }

            CheckBulletsCollision();
        }
    }
}