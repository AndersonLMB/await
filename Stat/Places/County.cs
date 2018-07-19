using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsQuery;

namespace Stat.Places
{
    public class County : Place, IPlace
    {
        public County()
        {
            this.Members = new List<Place>();
        }
        public async Task GetMembersAsync()
        {
            if (this.Url == null)
            {
                return;
            }
            else
            {
                string pageContent = await this.GetPageContentAsync();

                //CsQuery.Config.OutputFormatter = CsQuery.OutputFormatters.HtmlEncodingNone;
                var cq = CQ.CreateDocument(pageContent);
                var townElements = cq[".towntr"];
                foreach (var townElement in townElements)
                {
                    var town = new Town();
                    if (townElement.FirstChild.FirstChild.HasAttribute("href"))
                    {
                        var split = Url.Split('/');
                        split[split.Length - 1] = townElement.FirstChild.FirstChild.GetAttribute("href");
                        var newurl = String.Join("/", split);
                        town.Url = newurl;
                        town.Code = townElement.FirstChild.FirstChild.InnerText;
                        town.Name = townElement.ChildNodes[1].FirstChild.InnerText;
                    }
                    else
                    {
                        town.Code = townElement.FirstChild.InnerText;
                        town.Name = townElement.ChildNodes[1].InnerText;
                    }
                    //town.Father = this;
                    AddMember(town);
                    //Members.Add(town);
                }
            }
            //string pageContent = await this.GetPageContentAsync();
            //throw new NotImplementedException();
        }
    }

}

