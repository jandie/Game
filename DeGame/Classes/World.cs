using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
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

        public World(Graphics gr, int windowX, int windowY)
        {
            maps = new List<Map>();
            directionKeys = new bool[4];

            currentMap = 0;
            this.gr = gr;
            this.windowX = windowX;
            this.windowY = windowY;
        }

        public void UpdateWindow(int windowX, int windowY)
        {
            this.windowX = windowX;
            this.windowY = windowY;
        }

        public void KeyUpdate(char keyChar, int mouseX, int mouseY)
        {
            Player player = GetPlayer();
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

        private void CheckPlayerAndBots()
        {
            Enums.PlayerStatus playerAlive = CheckPlayerPosition();

            switch (playerAlive)
            {
                case Enums.PlayerStatus.Dead:
                    Reset();
                    break;
                case Enums.PlayerStatus.Win:
                    NextLevel();
                    Reset();
                    break;
            }
        }

        public void Draw()
        {
            maps[currentMap].DrawCellsPlayerAndBots(gr, windowX, windowY);
            CheckPlayerAndBots();
        }

        public Enums.PlayerStatus CheckPlayerPosition()
        {
            return maps[currentMap].CheckBotOnPlayer();
        }

        public void NextLevel()
        {
            currentMap++;

            if (currentMap > 9)
            {
                currentMap = 9;
            }
        }

        public int GetOverheadX()
        {
            return maps[currentMap].OverheadX;
        }

        public int GetOverheadY()
        {
            return maps[currentMap].OverheadY;
        }

        public void MoveBots()
        {
            maps[currentMap].MoveBots();
        }

        public List<Cel> GetDrawableCells(int x, int y)
        {
            return maps[currentMap].GetCells(x,y);
        }

        public List<Bot> GetDrawableBots(int x, int y)
        {
            return maps[currentMap].GetBots(x, y);
        }
        
        public Cel GetSingleCell(int x, int y)
        {
            return maps[currentMap].GetSingleCell(x, y);
        }

        public Player GetPlayer()
        {
            return maps[currentMap].GetPlayer();
        }

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

        public void Reset()
        {
            maps[currentMap].ResetPlayer();
            maps[currentMap].ResetBots();
            maps[currentMap].ResetPowerUps();
        }

        public void PlaceObject(Enums.Object typeCel,int x,int y)
        {
            maps[currentMap].PlaceObject(typeCel, x, y);
        }

        public void MoveAllBots()
        {
            maps[currentMap].MoveBots();
        }

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