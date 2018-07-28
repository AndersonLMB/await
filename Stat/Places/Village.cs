using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Stat.Places
{
    public class Village : Place, IPlace
    {
     
        public Village()
        {
            PlaceType = PlaceType.Village;

        }

       

        public Task GetMembersAsync()
        {
            base.GetMembersAsync();
            return null;
            //throw new NotImplementedException();
        }

        public Task StoreToDB(DbConnection dbConnection)
        {
            throw new NotImplementedException();
        }
    }

}

