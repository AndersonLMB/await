using System;
using System.Threading;
using System.Diagnostics;
using Stat.Places;
using Npgsql;
using Dapper;

namespace Stat
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //RunAsync();
            Console.ReadLine();
        }

        public static int GetPageContentDelay;

        public static async void RunAsync(int delay)
        {

            Stat.Places.PlacesConfig.Delay = delay;

            var sql = String.Format("SELECT \"NAME\",\"URL\",\"CODE\",\"PLACETYPE\" FROM public.stat WHERE \"NAME\"='{0}'", "中华人民共和国");
            //command.Connection = PgConnection;
            var china = PlacesConfig.PgConnection.QueryFirstOrDefault<Nation>(sql, new Nation()
            {
            });

            //var china = new Nation();
            //china.Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/";
            //china.Name = "中华人民共和国";

            await china.GetMembersAsync();
            foreach (var province in china.Members)
            {
                //Trace.WriteLine(province.Name);   
                //break;
                var tryProvince = (Province)province;
                await tryProvince.GetMembersAsync();
                foreach (var city in province.Members)
                {
                    //Trace.WriteLine(city.GetFullname());
                    var tryCity = (City)city;
                    await tryCity.GetMembersAsync();
                    //var tryFullname = tryCity.GetFullname();
                    //Trace.WriteLine(tryFullname);
                    foreach (var county in tryCity.Members)
                    {
                        var tryCounty = county as County;
                        await tryCounty.GetMembersAsync();
                        foreach (var town in county.Members)
                        {
                            var tryTown = town as Town;
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
    }

}

