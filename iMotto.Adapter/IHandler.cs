using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace iMotto.Adapter
{
    public interface IHandler
    {        
        Type ReqType { get; }

        HandleRequest ObtainModel();

        Task<HandleResult> Handle(HandleRequest model);
    }
}