using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsQuery;

namespace Stat.Places
{
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
                        //Father = this,
                        Code = String.Format("{0:D2}0000000000", aa.GetAttribute("href").Split('.')[0])
                    };

                    AddMember(newProvince);
                    //Members.Add(newProvince);
                }
            }
        }



    }

}

