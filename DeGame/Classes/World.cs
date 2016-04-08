using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace DeGame.Classes
{
    [DataContract]
    public class World
    {
        private readonly bool[] _directionKeys;
        private List<Map> _maps;
        private int _currentMap;
        [DataMember]
        private int _score;
        private int _windowX;
        private int _windowY;
        private Graphics _gr;
        readonly Database _database;

        public int CurrentLevel => _currentMap;
        public int CurrentScore { get { return _score; } set{ _score = value; } }

        public World(Graphics gr, World world, int windowX, int windowY)
        {
            _maps = new List<Map>();
            _directionKeys = new bool[4];
            _database = new Database();

            _currentMap = 0;
            this._gr = gr;
            this._windowX = windowX;
            this._windowY = windowY;
        }

        /// <summary>
        /// Updates the coördinates of the window in the world class.
        /// </summary>
        /// <param name="windowX">Window width</param>
        /// <param name="windowY">Window height</param>
        public void UpdateWindow(int windowX, int windowY)
        {
            this._windowX = windowX;
            this._windowY = windowY;
        }

        public void KeyUp(string key)
        {
            switch (key.ToLower())
            {
                case "w":
                    _directionKeys[0] = false;
                    break;
                case "a":
                    _directionKeys[1] = false;
                    break;
                case "s":
                    _directionKeys[2] = false;
                    break;
                case "d":
                    _directionKeys[3] = false;
                    break;
            }
        }

        /// <summary>
        /// Checks a key update to determine and do what needs to happen.
        /// </summary>
        /// <param name="key">key pressed</param>
        /// <param name="mouseX">X position of the mouse</param>
        /// <param name="mouseY">Y position of the mouse</param>
        public void KeyDown(string key, int mouseX, int mouseY)
        {
            Player player = _maps[_currentMap].GetPlayer();
            bool otherThanDirection = false;

            switch (key.ToLower())
            {
                case "w":
                    _directionKeys[0] = true;
                    break;
                case "a":
                    _directionKeys[1] = true;
                    break;
                case "s":
                    _directionKeys[2] = true;
                    break;
                case "d":
                    _directionKeys[3] = true;
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
                case "space":
                    _maps[_currentMap].PlayerShoots();
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
            Enums.PlayerStatus playerAlive = _maps[_currentMap].CheckBotOnPlayer();

            switch (playerAlive)
            {
                case Enums.PlayerStatus.Dead:
                    Reset();
                    _score -= 300;
                    break;
                case Enums.PlayerStatus.Win:
                    _score += 1000;
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
            _maps[_currentMap].DrawCellsAndEntities(_gr, _windowX, _windowY);
        }

        /// <summary>
        /// Increases currentLevel by 1. Maximum currentLevel is 9.
        /// </summary>
        public void NextLevel()
        {
            _currentMap++;

            if (_currentMap > 9)
            {
                _currentMap = 0;
            }
        }

        /// <summary>
        /// Attempts to load all maps from the database.
        /// If there are no maps in the database it generates new maps.
        /// </summary>
        public void LoadMap()
        {
            _maps = _database.LoadAllMaps();

            if (_maps == null)
            {
                _maps = new List<Map>();

                for (int i = 0; i < 10; i++)
                {
                    Map map = new Map(100, 50, 100, false);

                    _maps.Add(map);
                }

                //Database.SaveAllMaps(maps);
            }

            Draw();
        }

        /// <summary>
        /// Resets the players, bots and powerups in the current map.
        /// </summary>
        public void Reset()
        {
            _maps[_currentMap].ResetPlayer();
            _maps[_currentMap].ResetBots();
            _maps[_currentMap].ResetPowerUps();
        }

        /// <summary>
        /// Replaces the object of a cel in the current man.
        /// </summary>
        /// <param name="typeCel">The new object of the cell</param>
        /// <param name="x">The x coördinate of the cell</param>
        /// <param name="y">The y coördinate of the cell</param>
        public void PlaceObject(Enums.Object typeCel, int x, int y)
        {
            _maps[_currentMap].PlaceObject(typeCel, x, y);
        }

        /// <summary>
        /// Moves all the bots in the current map.
        /// </summary>
        public void MoveAllBots()
        {
            _maps[_currentMap].MoveBots();
            CheckPlayerAndBots();
        }

        /// <summary>
        /// Checks if player movement is neccessary and then moves the player.
        /// </summary>
        public void MovePlayer()
        {
            _maps[_currentMap].MovePlayer(_directionKeys);

            if (_directionKeys.Any(d => d))
            {
                CheckPlayerAndBots();
            }

            Draw();
        }

        public void MoveAllBullets()
        {
            _maps[_currentMap].MoveBullets();
        }
    }
}