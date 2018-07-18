using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using System.IO;
using CsQuery;
using CsQuery.Engine;
using System.Diagnostics;

namespace Stat
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }


    public interface IPlace
    {
        Task<string> GetPageContentAsync();
        Task GetMembersAsync();
    }


    public class Place
    {
        //http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/
        private string url;
        private string code;
        public string Url { get => url; set => url = value; }
        public string Name { get => name; set => name = value; }
        public Place Father { get => father; set => father = value; }
        public string Code { get => code; set => code = value; }

        private Place father;
        private string name;


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
            WebClient webClient = new WebClient();
            //webClient.DownloadString(new Uri(Url));
            var res = await webClient.DownloadStringTaskAsync(new Uri(Url));

            return res;
        }


        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public class Nation : Place, IPlace
    {
        public Nation()
        {
            this.Members = new List<Place>();
        }

        public void GetPage()
        {

        }

        public async Task GetMembersAsync()
        {
            string pageContent = await GetPageContentAsync();
            CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
            //CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
            var cq = CQ.CreateDocument(pageContent);
            var a = cq[".provincetr td a"];
            foreach (var aa in a)
            {
                if (aa.HasAttribute("href"))
                {
                    var split = Url.Split('/');
                    split[split.Length - 1] = aa.GetAttribute("href");
                    var newurl = String.Join("/", split);
                    var newProvince = new Province
                    {
                        Name = aa.InnerText,
                        Url = newurl,
                        Father = this
                    };
                    Members.Add(newProvince);
                }
            }
        }



    }

    public class Province : Place, IPlace
    {
        public Province()
        {
            this.Members = new List<Place>();
        }

        public async Task GetMembersAsync()
        {
            string pageContent = await this.GetPageContentAsync();
            CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
            //CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
            var cq = CQ.CreateDocument(pageContent);
            var cityElements = cq[".citytr"];
            foreach (var cityElement in cityElements)
            {
                var city = new City();
                city.Code = Code = cityElement.FirstChild.FirstChild.InnerText;
                city.Father = this;
                if (cityElement.FirstChild.FirstChild.HasAttribute("href"))
                {
                    var split = Url.Split('/');
                    split[split.Length - 1] = cityElement.FirstChild.FirstChild.GetAttribute("href");
                    var newurl = String.Join("/", split);
                }
                city.Name = cityElement.ChildNodes[1].FirstChild.InnerText;
                Members.Add(city);
            }
        }
    }

    public class City : Place, IPlace
    {
        public City()
        {
            Members = new List<Place>();
        }

        public async Task GetMembersAsync()
        {
            if (this.Url == null)
            {
                return;
            }
            string pageContent = await this.GetPageContentAsync();
            CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
            //CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
            var cq = CQ.CreateDocument(pageContent);
            var countyElements = cq[".countytr"];
            foreach (var countyElement in countyElements)
            {
                var county = new County();
                if (countyElement.FirstChild.FirstChild.HasAttribute("href"))
                {
                    var split = Url.Split('/');
                    split[split.Length - 1] = countyElement.FirstChild.FirstChild.GetAttribute("href");
                    var newurl = String.Join("/", split);
                    county.Url = newurl;
                    county.Code = countyElement.FirstChild.FirstChild.InnerText;
                    county.Name = countyElement.ChildNodes[1].FirstChild.InnerText;
                }
                else
                {
                    county.Code = countyElement.FirstChild.InnerText;
                    county.Name = countyElement.ChildNodes[1].InnerText;
                }
                county.Father = this;
                this.Members.Add(county);
            }
            //throw new NotImplementedException();
        }
    }

    public class County : Place, IPlace
    {
        public County()
        {
            this.Members = new List<Place>();
        }
        public Task GetMembersAsync()
        {
            throw new NotImplementedException();
        }
    }

}

