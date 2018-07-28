using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Stat.Places
{
    public class Place
    {
        private int requestTimes = 0;
        public DateTime RequestDT { get; set; }
        public DateTime ResponseDT { get; set; }
        private PlaceType placeType;
        //http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/
        private string url;
        private string code;
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get => url; set => url = value; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 上一级
        /// </summary>
        public Place Father { get => father; set => father = value; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get => code; set => code = value; }

        /// <summary>
        /// 是否自动存储成员到数据库
        /// </summary>
        public bool AutoStoreMembersToDB = true;

        /// <summary>
        /// 成员是否自动获取成员
        /// </summary>
        public bool MembersAutoGetMembers = false;

        public DbConnection Connection = PlacesConfig.SQLiteConnection;
        /// <summary>
        /// 地方类型
        /// </summary>
        public PlaceType PlaceType { get => placeType; set => placeType = value; }
        private string cxType;
        public string CxType { get => cxType; set => cxType = value; }
        public int RequestTimes { get => requestTimes; set => requestTimes = value; }

        private Place father;
        private string name;

        //public static int Delay = 100;

        /// <summary>
        /// 成员
        /// </summary>
        public List<Place> Members;
        /// <summary>
        /// 获取全称
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 异步获取页面
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPageContentAsync()
        {
            if (RequestTimes == 0)
            {
                RequestDT = DateTime.Now;
                UpdateRequestTimesToDBAsync(Connection);
            }
            else
            {
                ;
            }
            RequestTimes++;
            //Thread.Sleep(100);
            Thread.Sleep(PlacesConfig.Delay);
            WebClient webClient = new WebClient();
            try
            {
                //webClient.DownloadString(new Uri(Url));
                var res = await webClient.DownloadStringTaskAsync(new Uri(Url));
                ResponseDT = DateTime.Now;
                UpdateResponseTimesToDBAsync(this.Connection);
                return res;
            }
            catch (Exception ex)
            {
                var res = await GetPageContentAsync();
                return res;
                //throw;
            }
        }



        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="member"></param>
        public async Task AddMemberAsync(Place member)
        {
            member.Father = this;
            Members.Add(member);
            if (AutoStoreMembersToDB)
            {
                await member.StoreSelfToDBAsync(this.Connection);

                var result = Connection.Query("SELECT * FROM places;");
                //StoreToDB(this.Connection);
            }
            //if (MembersAutoGetMembers)
            //{

            //    member.GetMembersAsync();
            //}
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name.ToString();
        }

        public Task GetMembersAsync()
        {
            Console.WriteLine(GetFullname());
            return null;
            //throw new NotImplementedException();
        }

        public async Task StoreSelfToDBAsync(DbConnection dbConnection)
        {
            var param = new
            {
                Code = Code,
                Name = Name,
                Url = Url,
                Parent = Father.Code,
                PlaceType = PlaceType.ToString(),
                CxType = CxType,
            };
            dbConnection.Execute(@"INSERT INTO places(code,name,url,parent,placetype,cxtype) values(@Code, @Name,@Url,@Parent,@PlaceType,@CxType);",
                param);
            //if (this.PlaceType == PlaceType.Village)
            //{
            //    var result = dbConnection.Query("SELECT * FROM places;");
            //}
            //throw new NotImplementedException();
        }
        public async Task UpdateResponseTimesToDBAsync(DbConnection dbConnection)
        {
            var parem = new
            {
                //RequestDT = RequestDT,
                ResponseDT = ResponseDT,
                Code = Code
            };
            var sql = @"UPDATE places SET responsedt =@ResponseDT  WHERE code  = @Code;";

            dbConnection.Execute(sql, parem);
            //return null;

        }
        public async Task UpdateRequestTimesToDBAsync(DbConnection dbConnection)
        {
            var parem = new
            {
                RequestDT = RequestDT,
                //ResponseDT = ResponseDT,
                Code = Code
            };
            var sql = @"UPDATE places SET requestdt =@RequestDT  WHERE code  = @Code;";

            dbConnection.Execute(sql, parem);
            //return null;

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

