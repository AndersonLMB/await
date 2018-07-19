using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Stat.Places
{
    public static class PlacesConfig
    {
        public static int Delay = 100;
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
            Members.Add(member);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

}

