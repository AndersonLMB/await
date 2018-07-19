using Stat.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Stat;
namespace StatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var delay = int.Parse(ConfigurationManager.AppSettings["GetPageDelay"]);
            Stat.Program.RunAsync(delay);
            //RunAsync();
            Console.ReadLine();
        }
        //public static async void RunAsync()
        //{
        //    var china = new Nation();
        //    china.Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/";
        //    china.Name = "中华人民共和国";
        //    await china.GetMembersAsync();
        //    foreach (var province in china.Members)
        //    {
        //        //Trace.WriteLine(province.Name);   
        //        //break;
        //        var tryProvince = (Province)province;
        //        await tryProvince.GetMembersAsync();
        //        foreach (var city in province.Members)
        //        {
        //            //Trace.WriteLine(city.GetFullname());
        //            var tryCity = (City)city;
        //            await tryCity.GetMembersAsync();
        //            //var tryFullname = tryCity.GetFullname();
        //            //Trace.WriteLine(tryFullname);
        //            foreach (var county in tryCity.Members)
        //            {
        //                var tryCounty = county as County;
        //                await tryCounty.GetMembersAsync();
        //                foreach (var town in county.Members)
        //                {
        //                    var tryTown = town as Town;
        //                    await tryTown.GetMembersAsync();
        //                    foreach (var village in tryTown.Members)
        //                    {
        //                        var tryVillage = village as Village;
        //                        var tryFullname = tryVillage.GetFullname();
        //                        //Trace.WriteLine(tryFullname);
        //                        Console.WriteLine(tryFullname);
        //                    }
        //                    //Trace.WriteLine(tryTown.GetFullname());
        //                }
        //                //Trace.WriteLine(tryCounty.GetFullname());
        //            }
        //            //Trace.WriteLine(tryCity.GetFullname());
        //        }
        //    }
        //}
    }
}
