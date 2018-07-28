using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using CsQuery;

namespace Stat.Places
{
    public class City : Place, IPlace
    {
        public City()
        {
            Members = new List<Place>();
            PlaceType = PlaceType.City;
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
                //county.Father = this;
                AddMember(county);
                //this.Members.Add(county);
            }
            //throw new NotImplementedException();
        }

        public Task StoreToDB(DbConnection dbConnection)
        {
            throw new NotImplementedException();
        }
    }

}

