using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace Stat.Places
{
    public interface IPlace
    {
        Task GetMembersAsync();

    }



}

