using iMotto.Data;
using System;
using System.Threading.Tasks;
using iMotto.Common;
using iMotto.Events;
using iMotto.Data.Entities;
using iMotto.Cache;
using iMotto.Data.Entities.Models;
using Microsoft.Extensions.Logging;

namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 设备注册
    /// </summary>
    class RegisterDeviceHandler:BaseHandler<RegisterDeviceRequest>
    {
        private ILogger _logger;
        private readonly ICommonRepo _commonRepo;
        private readonly IGiftRepo _giftRepo;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private SpotLight awardSpotlight = null;
        private SpotLight awardeeSpotlight = null;

        public RegisterDeviceHandler(ILoggerFactory loggerFactory, 
            ICommonRepo commRepo, 
            IGiftRepo giftRepo,
            IEventPublisher eventPublisher, 
            ICacheManager cacheManager)
        {
            _logger = loggerFactory.CreateLogger<RegisterDeviceHandler>();
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
            _commonRepo = commRepo;
            _giftRepo = giftRepo;
            NeedSign = false;
        }

        protected override async Task<HandleResult> HandleCoreAsync(RegisterDeviceRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.UniqueId))
            {
                await Task.Delay(0);
                return new HandleResult {
                    State = HandleStates.InvalidData,
                    Msg = "请求数据有误"
                };
            }

            var devSig = Utils.GenId();

            var info = new Data.Entities.DeviceReg
            {
                Brand = reqObj.Brand,
                CurrentVersion = reqObj.Version,
                DevIdenNo = reqObj.UniqueId,
                DeviceSig = devSig,
                DeviceType = reqObj.Type??string.Empty,
                Model = reqObj.Model,
                OprSystem = reqObj.OS,
                ResolutionRatio = reqObj.Resolution,
                ScreenDesity = reqObj.Midu,
                ScreenSize = reqObj.Screen,
                SystemID = reqObj.OSId,
                SystemVer = reqObj.OSVersion,
                TerminalVersion = reqObj.TVersion,
                UserId = "",
                Note = ""
            };

            try
            {
                UpdateInfo uinfo = null;
                var rowAffected = _commonRepo.RegisterDevice(info, out uinfo);

                _eventPublisher.Publish(new DeviceRegEvent
                {
                    Signature = info.DeviceSig
                });
                

                var result = new RegisterDeviceResult {
                    State = HandleStates.Success,
                    Msg = string.Empty,
                    Sign = info.DeviceSig
                };

                if (uinfo != null)
                {
                    result.UpdateFlag = uinfo.ForceUpdate ? 2 : 1;
                    result.UpdateSummary = uinfo.PubDesc;
                    result.NewVersion = uinfo.DisplayVersion;
                    result.DownloadUrl = uinfo.Url;
                }
                else
                {
                    result.UpdateFlag = 0;
                    result.UpdateSummary = string.Empty;
                    result.NewVersion = string.Empty;
                    result.DownloadUrl = string.Empty;
                }

                var today = DateTime.Today;
                if (today.Day <= 20)
                {
                    int theMonth = today.Year * 100 + today.Month;

                    var displayNotice = _cacheManager.GetCache<IDeviceSignatureCache>().ShouldDisplayNotice(result.Sign, theMonth);

                    switch (displayNotice)
                    {
                        case DisplayNotice.Award:
                            if (awardSpotlight != null && awardSpotlight.TheMonth == theMonth)
                            {
                                result.Extra = awardSpotlight;
                            }
                            else
                            {
                                SpotLight award = await _giftRepo.GetAwardSpotlightAsync(theMonth);
                                awardSpotlight = award;
                                result.Extra = award;
                            }

                            break;
                        case DisplayNotice.Awardee:
                            if (awardeeSpotlight != null && awardeeSpotlight.TheMonth == theMonth)
                            {
                                result.Extra = awardeeSpotlight;
                            }
                            else
                            {
                                SpotLight awardee = await _giftRepo.GetAwardeeSpotligthAsync(theMonth);
                                awardeeSpotlight = awardee;
                                result.Extra = awardee;
                            }
                            break;
                    }

                    if (result.Extra != null)
                    {
#if !DEV
                        _eventPublisher.Publish(new DisplayNoticeEvent
                        {
                            Sign = result.Sign,
                            DisplayNotice = displayNotice,
                            TheMonth = theMonth
                        });
#endif
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError("Register device error:{0}", ex);

                return new RegisterDeviceResult
                {
                    State = HandleStates.UnkownError,
                    Msg = ex.Message,
                    Sign = info.DeviceSig
                };
            }           
        }
    }
}
