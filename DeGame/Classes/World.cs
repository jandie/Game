﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeGame.Classes;

namespace DeGame
{
    public class World
    {
        private List<Map> maps;
        private int currentMap;
        private int score;
        public int CurrentLevel { get { return currentMap; }}
        public World()
        {
            maps = new List<Map>();
            currentMap = 0;
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
        public List<Bot> GetBots()
        {
            List<Bot> botsAlive = new List<Bot>();
            List<Bot> bots = new List<Bot>();
            bots = maps[currentMap].Bots;
            foreach (Bot bot in bots)
            {
                if (bot.IsKilled() == false)
                {
                    botsAlive.Add(bot);
                }
            }
            return botsAlive;
        }
        public void MoveBots()
        {
            maps[currentMap].MoveBots();
        }
        public List<Cel> GetDrawableCells(int x, int y)
        {
            return maps[currentMap].GetCells(x,y);
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
                    Map map = new Map(100,100,100);
                    maps.Add(map);
                }
            }
        }
        public void Reset()
        {
            maps[currentMap].ResetPlayer();
            maps[currentMap].ResetBots();
        }
        public void PlaceObject(Enums.Object typeCel,int x,int y)
        {
            maps[currentMap].PlaceObject(typeCel, x, y);
        }

        public void MoveAllBots()
        {
            maps[currentMap].MoveBots();
        }

        public void MovePlayer(bool[] directionKeys)
        {
            maps[currentMap].MovePlayer(directionKeys);
        }
        public List<Cel> GetUpdatedCells()
        {
            return maps[currentMap].CellsToUpdate;
        }

        public void ClearCellsToUpdate()
        {
            maps[currentMap].ClearCellsToUpdate();
        }
    }
}