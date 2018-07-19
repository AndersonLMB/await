using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Npgsql;
using Dapper;

namespace Stat.Places
{
    public static class PlacesConfig
    {
        public static int Delay = 100;

        public static NpgsqlConnection PgConnection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=123;");

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
            //command.Connection = PgConnection;
            var nation = PgConnection.QueryFirstOrDefault<Nation>(sql, new { });
            //var reader = command.ExecuteReader(); ;
            //var gcs = reader.GetColumnSchema();

            PgConnection.Close();

            //while (reader.Read())
            //{
            //}

        }
    }
    public class Place
    {
        private PlaceType placeType;
        //http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/
        private string url;
        private string code;
        public string Url { get => url; set => url = value; }
        public string Name { get => name; set => name = value; }
        public Place Father { get => father; set => father = value; }
        public string Code { get => code; set => code = value; }
        /// <summary>
        /// 地方类型
        /// </summary>
        public PlaceType PlaceType { get => placeType; set => placeType = value; }

        private Place father;
        private string name;

        //public static int Delay = 100;

        public List<Place> Members;
        public string GetFullname()
        {
            string returnFullname = Name;
            Place tryFather = Father;
            if (tryFather != null)
            {
                returnFullname = String.Format("{0}{1}", tryFather.GetFullname(), returnFullname);
            }
            //tryFather = Father.Father;
            return returnFullname;
        }

        public async Task<string> GetPageContentAsync()
        {
            //Thread.Sleep(100);
            Thread.Sleep(PlacesConfig.Delay);
            WebClient webClient = new WebClient();
            try
            {
                //webClient.DownloadString(new Uri(Url));
                var res = await webClient.DownloadStringTaskAsync(new Uri(Url));
                return res;
            }
            catch (Exception ex)
            {
                var res = await GetPageContentAsync();
                return res;
                //throw;
            }


        }

        public void AddMember(Place member)
        {
            member.Father = this;
            Members.Add(member);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    /// <summary>
    /// 地方类型
    /// </summary>
    public enum PlaceType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        Undefined,
        /// <summary>
        /// 国家
        /// </summary>
        Nation,
        /// <summary>
        /// 省
        /// </summary>
        Province,
        /// <summary>
        /// 市
        /// </summary>
        City,
        /// <summary>
        /// 县
        /// </summary>
        County,
        /// <summary>
        /// 镇
        /// </summary>
        Town,
        /// <summary>
        /// 村
        /// </summary>
        Village
    }

}

