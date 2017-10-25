using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Service
{
    public interface ISmsService
    {
        Task<bool> SendMsg(string dest, string template, Dictionary<string, string> para);
    }
}
