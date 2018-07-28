using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using CsQuery;

namespace Stat.Places
{
    public class Town : Place, IPlace
    {
        public Town()
        {
            Members = new List<Place>();
            PlaceType = PlaceType.Town;
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
                var cq = CQ.CreateDocument(pageContent);
                var villageElements = cq[".villagetr"];
                foreach (var villageElement in villageElements)
                {
                    var village = new Village();
                    village.Code = villageElement.ChildNodes[0].InnerText;
                    village.CxType = villageElement.ChildNodes[1].InnerText;
                    village.Name = villageElement.ChildNodes[2].InnerText;
                    //village.Father = this;
                    //Members.Add(village);
                    AddMember(village);
                }
            }

            base.GetMembersAsync();
            //throw new NotImplementedException();
        }

        public void AddMember(Village village)
        {
            base.AddMemberAsync(village);
            if (MembersAutoGetMembers)
            {
                village.MembersAutoGetMembers = true;
                village.GetMembersAsync();
            }
                
        }


        public Task StoreToDB(DbConnection dbConnection)
        {
            throw new System.NotImplementedException();
        }
    }

}

