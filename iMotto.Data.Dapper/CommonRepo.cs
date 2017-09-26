using Dapper;
using iMotto.Data.Entities;
using System;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class CommonRepo : ICommonRepo
    {
        private IConnectionProvider _connProvider;
        public CommonRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }

        public int RegisterDevice(DeviceReg info, out UpdateInfo uinfo)
        {
            uinfo = null;
            int rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {

                    info.LoginTime = DateTime.Now;

                    rowAffected = conn.Execute(@"Update Devices set LoginTime=@LoginTime where DevIdenNo=@DevIdenNo", new
                    {
                        LoginTime = info.LoginTime,
                        DevIdenNo = info.DevIdenNo
                    }, tran);

                    if (rowAffected <= 0)
                    {
                        rowAffected = conn.Execute(@"INSERT INTO Devices(Brand,Model,OprSystem,SystemVer,ScreenSize,ResolutionRatio,ScreenDesity,
                        DevIdenNo,SystemID,TerminalVersion,CurrentVersion,UserId,LoginTime,DeviceSig,DeviceType,Note)
                        VALUES(@Brand,@Model,@OprSystem,@SystemVer,@ScreenSize,@ResolutionRatio,@ScreenDesity,
                        @DevIdenNo,@SystemID,@TerminalVersion,@CurrentVersion,@UserId,@LoginTime,@DeviceSig,@DeviceType,@Note)",
                            info, tran);
                    }
                    else
                    {
                        info.DeviceSig = conn.ExecuteScalar<string>("select DeviceSig from Devices where DevIdenNo=@DevIdenNo", new { info.DevIdenNo }, tran);
                    }

                    //检查是否需要更新
                    if (info.DeviceType.Equals("A") || info.DeviceType.Equals("B") || info.DeviceType.Equals("C"))
                    {
                        if (int.TryParse(info.TerminalVersion, out var vcode))
                        {
                            uinfo = conn.QueryFirstOrDefault<UpdateInfo>(@"select VersionName as DisplayVersion,`Force` as ForceUpdate,Summary as PubDesc, Url from Updates 
                            where DeviceType=@deviceType and VersionCode>@vcode order by VersionCode desc limit 0, 1",
                                new
                                {
                                    DeviceType = info.DeviceType,
                                    Vcode = vcode
                                });
                        }
                    }

                    tran.Commit();

                    return rowAffected;
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch
                    {
                        //ignore
                    }
                    throw;
                }
            }
        }

        public Task<UpdateInfo> TryUpdateAsync(string sign, string type, int version)
        {
            throw new NotImplementedException();
        }
    }
}
