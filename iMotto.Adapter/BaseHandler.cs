using System;
using System.Threading.Tasks;

namespace iMotto.Adapter
{
    public abstract class BaseHandler<T> : IHandler where T : HandleRequest, new()
    {
        protected bool NeedSign { get; set; }

        protected bool NeedVerifyUser { get; set; }

        protected string NeedRole { get; set; }

        public Type ReqType => typeof(T);

        public HandleRequest ObtainModel()
        {
            return new T();
        }

        public async Task<HandleResult> Handle(HandleRequest model)
        {
            var request = model as T;

            if (request == null)
            {
                throw new ArgumentException();
            }

            if (NeedSign)
            {
                if (!SignatureStore.PrepareSignature(request.Sign))
                {
                    await Task.Delay(5000);
                    return new HandleResult
                    {
                        Msg = "非法请求来源.",
                        State = HandleStates.InvalidSource
                    };
                }
            }

            if (NeedVerifyUser)
            {
                var authedReq = model as AuthedRequest;

                if (authedReq == null)
                {
                    await Task.Delay(5000);
                    return new HandleResult
                    {
                        State = HandleStates.InvalidData,
#if DEBUG
                        Msg = "Handler无法处理的请求。"
#else
                        Msg= "服务器一脸蒙逼，请重新登录再试吧"
#endif
                    };
                    //throw new Exception("请求的操作与请求的类型不匹配");
                }

                var signature = authedReq.Sign;

                if (!SignatureStore.VerifyUserToken(authedReq.Token, authedReq.UserId, signature))
                {
                    await Task.Delay(5000);
                    return new HandleResult
                    {
#if DEBUG
                        Msg = "Handler无法处理的请求。",
#else
                          Msg= "好像出了点问题，请重新登录再试吧",
#endif
                        State = HandleStates.NotLoginYet
                    };
                }

                if (!string.IsNullOrEmpty(NeedRole))
                {
                    //验证用户是具有指定角色
                    if (!SignatureStore.AssertUserHasRole(authedReq.UserId, NeedRole))
                    {
                        await Task.Delay(5000);
                        return new HandleResult
                        {
                            Msg = "用户不具有相关角色权限",
                            State = HandleStates.InvalidRole
                        };
                    }
                }
            }

            return await HandleCoreAsync(request);
        }

        protected abstract Task<HandleResult> HandleCoreAsync(T request);
    }
}






//using log4net;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http.ModelBinding;

//namespace iMotto.Adapter
//{
//    public abstract class BaseHandler<T> : IHandler where T : HandleRequest
//    {
//        private static Dictionary<Type, PropertyInfo[]> reqTypeProperties = new Dictionary<Type, PropertyInfo[]>();
//        private static List<MediaTypeFormatter> specialFormFormatter;
//        private static object lockHelper = new object();
//        private ILog logger;
//        private bool needVerifyUser = false;
//        private bool needSignature = true;
//        private string needRole = string.Empty;
//        private string signature;
//        public string Code { get; set; }

//        /// <summary>
//        /// 是否需要 验证用户身份(默认:false)
//        /// </summary>
//        protected bool NeedVerifyUser
//        {
//            get { return needVerifyUser; }
//            set
//            {
//                needVerifyUser = value;
//            }
//        }

//        /// <summary>
//        /// 是否需要 认证用户(默认:string.Empty)
//        /// </summary>
//        protected string NeedRole
//        {
//            get { return needRole; }
//            set
//            {
//                needRole = value;
//                if (!string.IsNullOrEmpty(needRole))
//                {
//                    needVerifyUser = true;
//                }
//            }
//        }

//        /// <summary>
//        /// 是否需要 设备签名(默认:true)
//        /// </summary>
//        protected bool NeedSign
//        {
//            get { return needSignature; }
//            set { needSignature = value; }
//        }

//        public BaseHandler(string code)
//        {
//            logger = LogManager.GetLogger(this.GetType());
//            Code = code;
//        }

//        protected async virtual Task<T> Extract(HttpContent req)
//        {
//            T t;

//            if (req.Headers.ContentType != null &&
//                req.Headers.ContentType.MediaType.Equals("application/x-www-form-urlencoded"))
//            {
//                if (specialFormFormatter == null)
//                {
//                    lock (lockHelper)
//                    {
//                        if (specialFormFormatter == null)
//                        {
//                            specialFormFormatter = new List<MediaTypeFormatter>();
//                            specialFormFormatter.Add(new JQueryMvcFormUrlEncodedFormatter());
//                        }
//                    }
//                }

//                t = await req.ReadAsAsync<T>(specialFormFormatter);
//            }
//            else if (req.IsMimeMultipartContent())
//            {
//                var root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");
//                var provider = new MultipartFormDataStreamProvider(root);

//                await req.ReadAsMultipartAsync(provider);

//                var type = typeof(T);
//                t = CreateRequestInstance(type, provider);
//            }
//            else
//            {
//                t = await req.ReadAsAsync<T>();
//            }

//            if (t != null)
//            {
//                t.Code = Code;
//                if (logger.IsInfoEnabled)
//                {
//                    logger.InfoFormat("receive request data:{0}", JsonConvert.SerializeObject(t));
//                }
//            }

//            return t;
//        }

//        private T CreateRequestInstance(Type type, MultipartFormDataStreamProvider provider)
//        {
//            var x = Activator.CreateInstance(type);
//            PropertyInfo[] properties;

//            if (!reqTypeProperties.ContainsKey(type))
//            {
//                lock (lockHelper)
//                {
//                    if (!reqTypeProperties.ContainsKey(type))
//                    {
//                        var props = type.GetProperties();
//                        reqTypeProperties.Add(type, props);
//                    }
//                }
//            }

//            properties = reqTypeProperties[type];

//            var todayPath = DateTime.Today.ToString("yyyyMM");
//            var directory = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/{0}", todayPath));

//            if (!System.IO.Directory.Exists(directory))
//            {
//                System.IO.Directory.CreateDirectory(directory);
//            }

//            foreach (var item in properties)
//            {
//                if (provider.FormData.AllKeys.Contains(item.Name))
//                {
//                    item.SetValue(x, HttpUtility.UrlDecode(provider.FormData[item.Name]));
//                }
//                else
//                {
//                    MultipartFileData fdm = null;
//                    var propName = string.Format("\"{0}\"", item.Name);
//                    foreach (var fd in provider.FileData)
//                    {
//                        if (fd.Headers.ContentDisposition.Name.Equals(propName))
//                        {
//                            fdm = fd;
//                        }
//                    }

//                    if (fdm != null)
//                    {
//                        var fileName = fdm.LocalFileName;
//                        var newFileName = Guid.NewGuid().ToString();

//                        var oriName = fdm.Headers.ContentDisposition.FileName.Replace("\"", "").ToLower();

//                        if (oriName.EndsWith(".jpg"))
//                        {
//                            var fileInfo = new FileInfo(fileName);
//                            newFileName += ".jpg";

//                            fileInfo.MoveTo(System.IO.Path.Combine(directory, newFileName));

//                        }
//                        else if (oriName.EndsWith(".png"))
//                        {
//                            var fileInfo = new FileInfo(fileName);
//                            newFileName += ".png";
//                            fileInfo.MoveTo(System.IO.Path.Combine(directory, newFileName));
//                        }
//                        else
//                        {
//                            var fileInfo = new FileInfo(fileName);
//                            newFileName = string.Format("{0}_{1}", newFileName, oriName.Substring(oriName.Length - 3));
//                            fileInfo.MoveTo(System.IO.Path.Combine(directory, newFileName));
//                        }

//                        item.SetValue(x, string.Format("{0}/{1}", todayPath, newFileName));
//                    }
//                }
//            }

//            return x as T;
//        }

//        public async Task<HandleResult> Handle(HttpContent req)
//        {
//            T t;
//            try
//            {
//                t = await Extract(req);
//            }
//            catch (Exception ex)
//            {
//                if (logger.IsErrorEnabled)
//                {
//                    logger.ErrorFormat("Extract request data failed:{0}", ex.ToString());
//                }

//                return new HandleResult
//                {
//                    Code = Code,
//                    State = HandleStates.InvalidData,
//#if DEBUG
//                    Msg = string.Format("Handler无法处理的请求。{0}", ex.InnerException ?? ex)
//#else
//                    Msg= "服务器一脸蒙逼，请换个姿势再试试吧"
//#endif
//                };
//            }

//            if (t == null)
//            {
//                return new HandleResult
//                {
//                    Code = Code,
//                    State = HandleStates.InvalidData,
//#if DEBUG
//                    Msg = "Handler无法处理的请求。"
//#else
//                    Msg= "服务器一脸蒙逼，请换个姿势再试试吧"
//#endif
//                };
//            }

//            if (needSignature)
//            {
//                if (!SignatureStore.PrepareSignature(t.Sign))
//                {
//                    return new HandleResult
//                    {
//                        Code = Code,
//                        Msg = "非法请求来源.",
//                        State = HandleStates.InvalidSource
//                    };
//                }
//            }

//            if (needVerifyUser)
//            {
//                var authedReq = t as AuthedRequest;

//                if (authedReq == null)
//                {
//                    return new HandleResult
//                    {
//                        Code = Code,
//                        State = HandleStates.InvalidData,
//#if DEBUG
//                        Msg = "Handler无法处理的请求。"
//#else
//                        Msg= "服务器一脸蒙逼，请重新登录再试吧"
//#endif
//                    };
//                    //throw new Exception("请求的操作与请求的类型不匹配");
//                }

//                signature = authedReq.Sign;

//                if (!SignatureStore.VerifyUserToken(authedReq.Token,authedReq.UserId, signature))
//                {
//                    return new HandleResult
//                    {
//                        Code = Code,
//#if DEBUG
//                        Msg = "Handler无法处理的请求。",
//#else
//                        Msg= "好像出了点问题，请重新登录再试吧",
//#endif
//                        State = HandleStates.NotLoginYet
//                    };
//                }

//                if (!string.IsNullOrEmpty(needRole))
//                {
//                    //验证用户是具有指定角色
//                    if (!RoleStateStore.AssertUserHasRole(authedReq.UserId, needRole))
//                    {
//                        return new HandleResult
//                        {
//                            Code = Code,
//                            Msg = "用户不具有相关角色权限",
//                            State = HandleStates.InvalidRole
//                        };
//                    }
//                }
//            }

//            try
//            {
//                var result = await HandleCoreAsync(t);
//                return result;

//            }
//            catch (Exception ex)
//            {
//                if (logger.IsErrorEnabled)
//                {
//                    logger.Error(ex.ToString());
//                }

//                return new HandleResult
//                {
//                    Code = Code,
//#if DEBUG
//                    Msg = ex.Message,
//#else
//                    Msg= "服务器一脸蒙逼，请换个姿势再试试吧",
//#endif
//                    State = HandleStates.UnkownError
//                };
//            }
//        }

//        protected abstract Task<HandleResult> HandleCoreAsync(T reqObj);

//    }

   

   
//}
