using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using CsQuery;

namespace Stat.Places
{
    public class Province : Place, IPlace
    {
        public Province()
        {
            this.Members = new List<Place>();
            this.PlaceType = PlaceType.Province;
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
                //city.Father = this;
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
            base.GetMembersAsync();
        }

        public void AddMember(City city)
        {
            base.AddMemberAsync(city);
            if (MembersAutoGetMembers)
            {
                city.MembersAutoGetMembers = true;
                city.GetMembersAsync();
            }
        }


        //public override Task StoreSelfToDB(DbConnection dbConnection)
        //{
        //    var self = this;


        //    var param = new
        //    {
        //        Code = Code,
        //        Name = Name,
        //        Url = Url,
        //        Parent = Father.Code,
        //        PlaceType = PlaceType.ToString()
        //    };
        //    dbConnection.Execute(@"INSERT INTO places(code,name,url,parent,placetype) values(@Code, @Name,@Url,@Parent,@PlaceType);",
        //        param);
        //    var result = dbConnection.Query("SELECT * FROM places;");
        //    return null;
        //    //throw new NotImplementedException();
        //}
    }

}

