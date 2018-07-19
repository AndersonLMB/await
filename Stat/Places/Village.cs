using System;
using System.Threading.Tasks;

namespace Stat.Places
{
    public class Village : Place, IPlace
    {
        private string cxType;
        public Village()
        {


        }

        public string CxType { get => cxType; set => cxType = value; }

        public Task GetMembersAsync()
        {
            throw new NotImplementedException();
        }
    }

}

