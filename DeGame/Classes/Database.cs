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
    public static class Database
    {
        private static readonly string databaseFilename = "Figuren.sqlite";
        private static SQLiteConnection connection;

        public static string Query
        {
            set
            {
                PrepareConnection();
                Command = new SQLiteCommand(value, connection);
            }
        }

        public static SQLiteCommand Command { get; private set; }

        public static string DatabaseFilename
        {
            get { return databaseFilename; }
        }
        
        public static void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }


        public static void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public static void PrepareConnection()
        {
            bool createNew = !File.Exists(databaseFilename);

            if (createNew)
            {
                SQLiteConnection.CreateFile(databaseFilename);
            }
            
            if (connection == null)
            {
                connection = new SQLiteConnection("Data Source=" + databaseFilename + ";Version=3");
            }

            if (createNew)
            {
                CreateDummyData();
            }
        }

        private static void CreateDummyData()
        {
            OpenConnection();

            try
            {
                Query = "CREATE TABLE `Figuur` (`nr` INTEGER, `typefiguur` INTEGER, `punt1x` INTEGER, `punt1y` INTEGER, `punt2x` INTEGER, `punt2y` INTEGER, `r` INTEGER, `g`	INTEGER, `b`	INTEGER, `lijndikte` INTEGER, PRIMARY KEY(nr))";
                Command.ExecuteNonQuery();
                
            }
            catch (SQLiteException e)
            {
                string error = e.ToString();
            }

            CloseConnection();
        }
        public static List<Map> LoadAllMaps()
        {
            return null;
        }
        public static bool SaveAllMaps()
        {
            return false;
        }
    }
}