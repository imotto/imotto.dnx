using iMotto.Adapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using iMotto.Service;

namespace iMotto.Api.Controllers
{
    [Produces("application/json")]
    public class AdapterController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private readonly IObjectStorageService _objectStorageService;
        private readonly AdapterFactory _adapterFactory;

        public AdapterController(ILoggerFactory loggerFactory, 
            IHostingEnvironment env,
            IObjectStorageService objectStorageService,
            AdapterFactory adapterFactory)
        {
            _logger = loggerFactory.CreateLogger<AdapterController>();
            _env = env;
            _objectStorageService = objectStorageService;
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

                if (Request.ContentType.IndexOf("multipart/form-data", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    Request.Form.Files.Count > 0)
                {
                    await TryFillFile(Request.Form.Files, model);
                }

                return await handler.Handle(model);
            }
        }

        private async Task TryFillFile(IFormFileCollection files, HandleRequest model)
        {
            foreach (var file in files)
            {
                var pName = file.Name;

                var prop = model.GetType().GetProperty(pName);
                if (prop == null)
                {
                    continue;
                }

                var path = Path.GetTempPath();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(path, fileName);

                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }
                try
                {
                    var url = _objectStorageService.UploadFile($"{pName}/{fileName}", filePath);

                    prop.SetValue(model, url);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Save file to oss raised an error:{0}", ex.ToString());
                }
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