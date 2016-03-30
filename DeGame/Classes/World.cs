using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DeGame.Classes;

namespace DeGame
{
   
    public class World
    {
        private bool[] directionKeys;
        private List<Map> maps;
        private int currentMap;
        
        private int score;
        private int windowX;
        private int windowY;
        private Graphics gr;

        public int CurrentLevel { get { return currentMap; }}
        public int CurrentScore { get { return score; } }

        public World(Graphics gr, int windowX, int windowY)
        {
            maps = new List<Map>();
            directionKeys = new bool[4];

            currentMap = 0;
            this.gr = gr;
            this.windowX = windowX;
            this.windowY = windowY;
        }

        /// <summary>
        /// Saves the current score.
        /// </summary>
       
        /// <summary>
        /// Updates the coördinates of the window in the world class.
        /// </summary>
        /// <param name="windowX">Window width</param>
        /// <param name="windowY">Window height</param>
        public void UpdateWindow(int windowX, int windowY)
        {
            this.windowX = windowX;
            this.windowY = windowY;
        }

        /// <summary>
        /// Checks a key update to determine and do what needs to happen.
        /// </summary>
        /// <param name="keyChar">Char of the pressed key</param>
        /// <param name="mouseX">X position of the mouse</param>
        /// <param name="mouseY">Y position of the mouse</param>
        public void KeyUpdate(char keyChar, int mouseX, int mouseY)
        {
            Player player = maps[currentMap].GetPlayer();
            bool otherThanDirection = false;

            switch (Convert.ToString(keyChar).ToLower())
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
                    PlaceObject(Enums.Object.Grass, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "x":
                    PlaceObject(Enums.Object.Wall, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "c":
                    PlaceObject(Enums.Object.StartPoint, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "v":
                    PlaceObject(Enums.Object.Destination, mouseX, mouseY);
                    otherThanDirection = true;
                    break;
                case "r":
                    Reset();
                    break;
            }

            if (otherThanDirection)
            {
                Draw();
                CheckPlayerAndBots();
            }
        }

        /// <summary>
        /// Checks if a bot is in the same cell as a player.
        /// If the player dies, the map gets reset.
        /// </summary>
        private void CheckPlayerAndBots()
        {
            Enums.PlayerStatus playerAlive = maps[currentMap].CheckBotOnPlayer();

            switch (playerAlive)
            {
                case Enums.PlayerStatus.Dead:
                    Reset();
                    score -= 300;
                    break;
                case Enums.PlayerStatus.Win:
                    score += 1000;
                    NextLevel();
                    Reset();
                    break;
            }
        }

        /// <summary>
        /// Draws all the entities and objects of the current map on the main screen.
        /// </summary>
        public void Draw()
        {
            maps[currentMap].DrawCellsPlayerAndBots(gr, windowX, windowY);
            CheckPlayerAndBots();
        }

        /// <summary>
        /// Increases currentLevel by 1. Maximum currentLevel is 9.
        /// </summary>
        public void NextLevel()
        {
            currentMap++;

            if (currentMap > 9)
            {
                currentMap = 9;
            }
        }

        /// <summary>
        /// Attempts to load all maps from the database.
        /// If there are no maps in the database it generates new maps.
        /// </summary>
        public void LoadMap()
        {
            maps = Database.LoadAllMaps();

            if (maps == null)
            {
                maps = new List<Map>();

                for (int i = 0; i < 10; i++)
                {
                    Map map = new Map(100,30,30);

                    maps.Add(map);
                }
            }

            Draw();
        }

        /// <summary>
        /// Resets the players, bots and powerups in the current map.
        /// </summary>
        public void Reset()
        {
            maps[currentMap].ResetPlayer();
            maps[currentMap].ResetBots();
            maps[currentMap].ResetPowerUps();
        }

        /// <summary>
        /// Replaces the object of a cel in the current man.
        /// </summary>
        /// <param name="typeCel">The new object of the cell</param>
        /// <param name="x">The x coördinate of the cell</param>
        /// <param name="y">The y coördinate of the cell</param>
        public void PlaceObject(Enums.Object typeCel,int x,int y)
        {
            maps[currentMap].PlaceObject(typeCel, x, y);
        }

        /// <summary>
        /// Moves all the bots in the current map.
        /// </summary>
        public void MoveAllBots()
        {
            maps[currentMap].MoveBots();
        }

        /// <summary>
        /// Checks if player movement is neccessary and then moves the player.
        /// </summary>
        public void MovePlayer()
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
                maps[currentMap].MovePlayer(directionKeys);

                for (int i = 0; i < directionKeys.Count(); i++)
                {
                    directionKeys[i] = false;

                }
                Draw();
                CheckPlayerAndBots();
            }
            
        }
    }
}