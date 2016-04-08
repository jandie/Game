using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using DeGame.Classes;
using DeGame.Enums;
using DeGame;

namespace DeGame
{
    public class Database : Repository.IGameRepo 
    {
        private readonly string databaseFilename = "DeGame.sqlite";
        private SQLiteConnection _connection;

        public string Query
        {
            set
            {
                PrepareConnection();
                Command = new SQLiteCommand(value, _connection);
            }
        }

        public SQLiteCommand Command { get; private set; }

        public string DatabaseFilename
        {
            get { return databaseFilename; }
        }
        
        public void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }


        public void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public void PrepareConnection()
        {
            bool createNew = !File.Exists(databaseFilename);

            if (createNew)
            {
                SQLiteConnection.CreateFile(databaseFilename);
            }
            
            if (_connection == null)
            {
                _connection = new SQLiteConnection("Data Source=" + databaseFilename + ";Version=3");
            }

            if (createNew)
            {
                CreateDummyData();
            }
        }

        private void CreateDummyData()
        {
            OpenConnection();

            try
            {
                Query = "CREATE TABLE `MAP` (`Map_ID` INTEGER UNSIGNED AUTO_INCREMENT PRIMARY KEY, `CellSize` INTEGER, `MapSize` INTEGER, `AmountOfBots` INTEGER);";
                Command.ExecuteNonQuery();
                Query = "CREATE TABLE `CELL` (`Cell_ID` INTEGER UNSIGNED AUTO_INCREMENT PRIMARY KEY, `Map_ID` INTEGER, `Type` VARCHAR(20), `X` INTEGER, `Y` INTEGER);";
                Command.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                string error = e.ToString();
            }

            CloseConnection();
        }

        public List<Map> LoadAllMaps()
        {
            try
            {
                PrepareConnection();
                Query = "SELECT * FROM Map";
                OpenConnection();

                SQLiteDataReader reader = Command.ExecuteReader();

                List<Map> Maps = new List<Map>();

                while (reader.Read())
                {
                    Maps.Add(new Map(Convert.ToInt32(reader["CellSize"]),
                        Convert.ToInt32(reader["MapSize"]),
                        Convert.ToInt32(reader["AmountOfBots"]), true));
                }

                List<Cel> Cells = new List<Cel>();

                for (int i = 0; i < 10; i++)
                {
                    Query = "SELECT * FROM Cell WHERE `Map_ID` = " + i.ToString();
                    OpenConnection();

                    reader = Command.ExecuteReader();

                    while (reader.Read())
                    {
                        string typestring = Convert.ToString(reader["Type"]);
                        Enums.Object type = (Enums.Object)Enum.Parse(typeof(Enums.Object),typestring);
                        int x = Convert.ToInt32(reader["X"]);
                        int y = Convert.ToInt32(reader["Y"]);
                        Cells.Add(new Cel(type,
                            x,
                            y));
                    }

                    if (Cells.Count != 0)
                    {
                        Maps[i].SetCells(new List<Cel>(Cells));
                    }

                    Cells.Clear();
                }

                CloseConnection();

                if (Maps.Count == 0)
                {
                    return null;
                }
                else
                {
                    return Maps;
                }
                
            }
            catch(Exception e)
            {
                string exception = e.ToString();

                return null;
            }
        }

        public bool SaveAllMaps(List<Map> _maps)
        {
            int ID = 0;
            bool success = true;

            PrepareConnection();

            OpenConnection();
            Query = "DELETE FROM MAP";
            Command.ExecuteNonQuery();
            Query = "DELETE FROM CELL";
            Command.ExecuteNonQuery();
            CloseConnection();

            OpenConnection();

            foreach (Map map in _maps)
            {
                Query = "INSERT INTO MAP (CellSize, MapSize, AmountOfBots) values (@CellSize, @MapSize, @AmountOfBots);";
                Command.Parameters.AddWithValue("CellSize", map.CellSize);
                Command.Parameters.AddWithValue("MapSize", map.MapSize);
                Command.Parameters.AddWithValue("AmountOfBots", map.AmountOfBots);

                try
                {
                    Command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    string error = e.ToString();

                    success = false;
                }

                List<Cel> cells = map.GetAllCells();

                foreach (Cel cel in cells)
                {
                    Query = "INSERT INTO CELL (Map_ID, Type, X, Y) values (@Map_ID, @Type, @X, @Y);";
                    Command.Parameters.AddWithValue("Map_ID", ID);
                    Command.Parameters.AddWithValue("Type", cel.GetTypeCel().ToString());
                    Command.Parameters.AddWithValue("X", cel.GetX());
                    Command.Parameters.AddWithValue("Y", cel.GetY());

                    try
                    {
                        Command.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        string error = e.ToString();

                        success = false;
                    }
                }

                ID++;
            }

            CloseConnection();

            return success;
        }
    }
}