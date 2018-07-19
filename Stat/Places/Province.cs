using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsQuery;

namespace Stat.Places
{
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
                    city.Url = newurl;
                }
                city.Name = cityElement.ChildNodes[1].FirstChild.InnerText;
                //Members.Add(city);
                AddMember(city);
            }
        }
    }

}

