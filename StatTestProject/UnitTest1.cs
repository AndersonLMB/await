using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stat;
using Stat.Places;
using System.Threading.Tasks;
using System.Diagnostics;

namespace StatTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            var china = new Nation();
            china.Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/";
            china.Name = "中华人民共和国";
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
                                Trace.WriteLine(tryFullname);
                            }
                            //Trace.WriteLine(tryTown.GetFullname());
                        }
                        //Trace.WriteLine(tryCounty.GetFullname());
                    }
                    //Trace.WriteLine(tryCity.GetFullname());
                }
            }
        }

        [TestMethod]
        public async Task ProvinceTestAsync()
        {
            var hebei = new Province
            {
                Name = "河北省",
                Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/13.html"
            };
            await hebei.GetMembersAsync();
        }

        [TestMethod]
        public async Task CityTestAsync()
        {
            var shenyang = new City
            {
                Url = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/21/2101.html",
                Name = "沈阳市"
            };
            await shenyang.GetMembersAsync();
        }
    }


}
