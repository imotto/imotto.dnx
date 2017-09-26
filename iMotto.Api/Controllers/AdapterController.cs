using iMotto.Adapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace iMotto.Api.Controllers
{
    [Produces("application/json")]
    public class AdapterController : Controller
    {
        private readonly ILogger _logger;
        private readonly AdapterFactory _adapterFactory;

        public AdapterController(ILoggerFactory loggerFactory, AdapterFactory adapterFactory)
        {
            _logger = loggerFactory.CreateLogger<AdapterController>();
            _adapterFactory = adapterFactory;
        }

        public async Task<HandleResult> Index(string code)
        {
            code = code.ToUpper();
            var ada = _adapterFactory.GetAdapter(code);
            var handler = ada.GetHandler(code);

            if ("application/json".Equals(Request.ContentType, StringComparison.OrdinalIgnoreCase))
            {
                var model = await TryReadModelAsync(handler.ReqType);
                if (model == null)
                {
                    return new HandleResult
                    {
                        State = HandleStates.InvalidData,
                        Msg = "Input error."
                    };
                }

                model.Code = code;
                return await handler.Handle(model);
            }
            else
            {
                var model = handler.ObtainModel();
                
                await TryUpdateModelAsync(model, model.GetType(), string.Empty);
                return await handler.Handle(model);
            }

        }

        private async Task<HandleRequest> TryReadModelAsync(Type type)
        {
            using (var sr = new StreamReader(Request.Body))
            {
                var json = await sr.ReadToEndAsync();

                try
                {
                    var model = JsonConvert.DeserializeObject(json, type);

                    return model as HandleRequest;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Read Model error,[{0}],{1}", Request.Path, ex);
                    return null;
                }

            }

        }
    }
}