using iMotto.Cache;
using iMotto.Common;
using iMotto.Common.Settings;
using iMotto.Data;
using iMotto.Data.Entities.Models;
using iMotto.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace iMotto.Adapter.Common
{
    class VerifyCodeHandler : BaseHandler<VerifyCodeRequest>
    {
        private readonly ISmsSetting _smsSetting;
        private readonly IUserRepo _userRepo;
        private readonly IVerifyCodeCache _verifyCodeCache;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public VerifyCodeHandler(ISettingProvider settingProvider,
            IUserRepo userRepo,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            ILoggerFactory loggerFactory)
        {
            _smsSetting = settingProvider.GetSmsSetting();
            _userRepo = userRepo;
            _verifyCodeCache = cacheManager.GetCache<IVerifyCodeCache>();
            _eventPublisher = eventPublisher;
            _logger = loggerFactory.CreateLogger<VerifyCodeHandler>();
        }

        protected async override Task<HandleResult> HandleCoreAsync(VerifyCodeRequest reqObj)
        {
            if (!Utils.IsValidMobile(reqObj.Mobile))
            {
                return new VerifyCodeResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "手机号码输入有误"
                };
            }

            if (reqObj.OpCode == VerifyCode.TYPE_REGISTER)
            {
                var user = await _userRepo.GetByPhoneNumberAsync(reqObj.Mobile);

                if (user != null)
                {
                    return new VerifyCodeResult
                    {
                        State = HandleStates.DuplicatedPhone,
                        Msg = "此手机号码已被注册"
                    };
                }

                VerifyCode t = _verifyCodeCache.PopVerifyCodeViaMobile(reqObj.Mobile);

                if (t == null)
                {
                    t = new VerifyCode
                    {
                        Mobile = reqObj.Mobile,
                        Code = Utils.GenRandomCode(),
                        Type = VerifyCode.TYPE_REGISTER
                    };
                }
                if (SendVCode(reqObj.Mobile, t.Code, _smsSetting.RegisterTemplateId))
                {

                    _eventPublisher.Publish<SendVerifyCodeEvent>(new SendVerifyCodeEvent
                    {
                        VerifyCode = t
                    });

                    return new VerifyCodeResult
                    {
                        State = HandleStates.Success,
                        Msg = string.Empty,
                        Content = t.Code //should remove.
                    };
                }
                else
                {
                    return new VerifyCodeResult
                    {
                        State = HandleStates.UnkownError,
                        Msg = "发送验证码时出了点问题，请稍后重试"
                    };
                }

            }
            else if (reqObj.OpCode == VerifyCode.TYPE_RESETPWD)
            {

                var user = await _userRepo.GetByPhoneNumberAsync(reqObj.Mobile);

                if (user == null)
                {
                    return new VerifyCodeResult
                    {
                        State = HandleStates.UserNotExists,
                        Msg = "此手机号码尚未注册"
                    };
                }

                VerifyCode t = _verifyCodeCache.PopVerifyCodeViaMobile(reqObj.Mobile);

                if (t == null)
                {
                    t = new VerifyCode
                    {
                        Mobile = reqObj.Mobile,
                        Code = Utils.GenRandomCode(),
                        Type = VerifyCode.TYPE_RESETPWD
                    };
                }

                if (SendVCode(reqObj.Mobile, t.Code, _smsSetting.ResetPassTemplateId))
                {

                    _eventPublisher.Publish<SendVerifyCodeEvent>(new SendVerifyCodeEvent
                    {
                        VerifyCode = t
                    });

                    return new VerifyCodeResult
                    {
                        State = HandleStates.Success,
                        Msg = string.Empty,
                        Content = string.Empty
                    };
                }
                else
                {
                    return new VerifyCodeResult
                    {
                        State = HandleStates.UnkownError,
                        Msg = "发送验证码时出了点问题，请稍后重试"
                    };
                }
            }


            return new VerifyCodeResult
            {
                State = HandleStates.InvalidData,
                Msg = "请求数据输入有误"
            };


        }


        private bool SendVCode(string mobile, string code, string templateId)
        {

            //ITopClient client = new DefaultTopClient(apiUrl, appKey, appSecret);
            //AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            ////req.Extend = "123456";
            //req.SmsType = "normal";
            //req.SmsFreeSignName = "偶得";
            //req.SmsParam = "{\"vcode\":\"" + code + "\"}";
            //req.RecNum = mobile;
            //req.SmsTemplateCode = templateId;
            //AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);


            _logger.LogInformation("发送验证码到手机[{0}],[{1}],resp:{2}", mobile, templateId, "rsp.Body");
            return false;
        }
    }
}
