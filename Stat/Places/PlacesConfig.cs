using System;
using Npgsql;
using System.Data.SQLite;
using Dapper;

namespace Stat.Places
{
    public static class PlacesConfig
    {
        public static int Delay = 100;

        public static NpgsqlConnection PgConnection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=123;");
        public static SQLiteConnection SQLiteConnection = new SQLiteConnection(@"Data Source=DB\sptest1.sqlite;Version=3;");
        public static void SetPgConnection(string conStr)
        {
            PgConnection.ConnectionString = conStr;
            try
            {
                PgConnection.Open();
                PgConnection.Close();
            }
            catch (Exception)
            {

                throw;
            }
            PgConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.CommandText = "SELECT * FROM public.stat";
            var sql = "SELECT * FROM public.stat";
            var nation = PgConnection.QueryFirstOrDefault<Nation>(sql, new { });
            PgConnection.Close();

        }

        public static void SetSqliteConnection(string conStr)
        {
            if (!String.IsNullOrEmpty(conStr))
            {
                SQLiteConnection.ConnectionString = conStr;
            }
            try
            {
                SQLiteConnection.Open();
                SQLiteConnection.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}

