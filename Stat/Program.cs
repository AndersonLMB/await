using System;
using System.Threading;
using System.Diagnostics;
using Stat.Places;
using Npgsql;
using Dapper;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.Configuration;
using SpatialiteSharp;
using System.IO;

namespace Stat
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //RunAsync();
            Console.ReadLine();
            ReadFromDBFile();
        }

        public static int GetPageContentDelay;

        public static async void RunAsync(int delay)
        {

            Stat.Places.PlacesConfig.Delay = delay;

            //var sql = String.Format("SELECT \"NAME\",\"URL\",\"CODE\",\"PLACETYPE\" FROM public.stat WHERE \"NAME\"='{0}'", "中华人民共和国");
            //command.Connection = PgConnection;
            //var china = PlacesConfig.PgConnection.QueryFirstOrDefault<Nation>(sql, new Nation()
            //{
            //});

            var fi = new FileInfo(@"DB\sptest1.sqlite");
            fi.Delete();
            var fib = new FileInfo(@"DB\sptest1back.sqlite");
            File.Copy(@"DB\sptest1back.sqlite", @"DB\sptest1.sqlite");

            PlacesConfig.SetSqliteConnection(ConfigurationManager.ConnectionStrings["SqliteCon"].ConnectionString);
            var command = new SQLiteCommand();
            var connection = PlacesConfig.SQLiteConnection;
            connection.Open();
            command.Connection = connection;
            var sql = String.Format("SELECT * FROM PLACES WHERE NAME = '{0}';  ", "中华人民共和国");
            var result = connection.Query("SELECT * FROM PLACES;");
            var china = connection.QueryFirstOrDefault<Nation>(sql);
            china.AutoStoreMembersToDB = true;

            //connection.Query<Nation>("SELECT * FROM PLACES WHERE NAME = '{0}'  ", "z"     )
            //command.Connection = PlacesConfig.SQLiteConnection;
            //command.Connection.Open();



            //new SQLiteConnection("")

            //var china = new Nation();
            //china.Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/";
            //china.Name = "中华人民共和国";

            //await china.GetMembersAsync();
            await china.GetMembersAsync();
            foreach (var province in china.Members)
            {
                //Trace.WriteLine(province.Name);   
                //break;
                var tryProvince = (Province)province;
                tryProvince.AutoStoreMembersToDB = true;
                await tryProvince.GetMembersAsync();
                foreach (var city in province.Members)
                {
                    //Trace.WriteLine(city.GetFullname());
                    var tryCity = (City)city;
                    tryCity.AutoStoreMembersToDB = true;
                    await tryCity.GetMembersAsync();
                    //var tryFullname = tryCity.GetFullname();
                    //Trace.WriteLine(tryFullname);
                    foreach (var county in tryCity.Members)
                    {
                        var tryCounty = county as County;
                        tryCounty.AutoStoreMembersToDB = true;
                        await tryCounty.GetMembersAsync();
                        foreach (var town in county.Members)
                        {
                            var tryTown = town as Town;
                            tryTown.AutoStoreMembersToDB = true;
                            await tryTown.GetMembersAsync();
                            foreach (var village in tryTown.Members)
                            {
                                var tryVillage = village as Village;
                                var tryFullname = tryVillage.GetFullname();
                                //Trace.WriteLine(tryFullname);
                                Console.WriteLine(tryFullname);
                            }
                            //Trace.WriteLine(tryTown.GetFullname());
                        }
                        //Trace.WriteLine(tryCounty.GetFullname());
                    }
                    //Trace.WriteLine(tryCity.GetFullname());
                }
            }
        }

        /// <summary>
        /// 读取DBFile
        /// </summary>
        public static void ReadFromDBFile()
        {

            var connection = new SQLiteConnection(@"Data Source=DB\sptest1.sqlite;Version=3;");
            var command = new SQLiteCommand(connection);

            connection.Open();
            SpatialiteSharp.SpatialiteLoader.Load(connection);
            connection.Execute("CREATE TABLE places (code text, name text, url text, parent text, placetype text)");
            connection.Execute(@"INSERT INTO places(code,name,url,placetype) values(@Code,@Name,@Url,@PlaceType)", new Place()
            {
                Code = "",
                Name = "中华人民共和国",
                Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/",

                PlaceType = PlaceType.Undefined
            });
            var result = connection.Query<Place>("SELECT * FROM PLACES;");
        }
    }

}

