using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotto.Data.Entities;

namespace iMotto.Data
{
    public interface IManageRepo : IRepository
    {
        Task<int> AddInviteCode(string uid, int source, List<string> inviteCodes);
    }
}
