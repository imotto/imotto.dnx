using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iMotto.Adapter;
using Microsoft.AspNetCore.Diagnostics;

namespace iMotto.Api.Controllers
{
    public class HomeController : Controller
    {
        private const string EzyNodeId = "EZY_NODE_ID";
        private readonly ILogger _logger;
        private readonly string _node;

        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("EzyError");
            _node = Environment.GetEnvironmentVariable(EzyNodeId);
        }

        // GET: /<controller>/
        public string Index()
        {
            return $"hi, man! [{DateTime.Now}]{(string.IsNullOrWhiteSpace(_node) ? "" : $"（{_node}）")}";
        }

        public HandleResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exceptionPath = (feature as ExceptionHandlerFeature)?.Path;
            var error = feature?.Error;

            _logger.LogError("Unhandled Error occured during request to[{0}]：\r\n{1}", exceptionPath, error?.ToString());

            return new HandleResult
            {
                State = HandleStates.UnkownError,
                Msg = $"操作失败：{error?.Message}"
            };  
        }
    }
}