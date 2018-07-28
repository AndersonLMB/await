using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Stat.Places
{
    public class Village : Place, IPlace
    {
        private string cxType;
        public Village()
        {
            PlaceType = PlaceType.Village;

        }

        public string CxType { get => cxType; set => cxType = value; }

        public Task GetMembersAsync()
        {
            throw new NotImplementedException();
        }

        public Task StoreToDB(DbConnection dbConnection)
        {
            throw new NotImplementedException();
        }
    }

}

