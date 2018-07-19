using System.Collections.Generic;
using System.Threading.Tasks;
using CsQuery;

namespace Stat.Places
{
    public class Town : Place, IPlace
    {
        public Town()
        {
            Members = new List<Place>();
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
                    village.Father = this;
                    //Members.Add(village);
                    AddMember(village);
                }
            }

            //throw new NotImplementedException();
        }
    }

}

