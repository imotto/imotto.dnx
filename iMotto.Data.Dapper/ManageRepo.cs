using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class ManageRepo : IManageRepo
    {



        public async Task<int> AddInviteCode(string uid, int source, List<string> inviteCodes)
        {
            int rowAffected = 0;
            await Task.Delay(0);
            //using (var conn = new SqlConnection(ConnStr))
            //{
            //    await conn.OpenAsync();
            //    var tran = conn.BeginTransaction();
            //    var cmd = conn.CreateCommand();
            //    cmd.Transaction = tran;
            //    cmd.CommandType = System.Data.CommandType.Text;
            //    cmd.CommandText = @"insert into T_InviteCode(GenUId,Code,CodeState,Source,CreateTime,ExpireTime) 
            //            values(@GenUid,@Code,@CodeState,@Source,@CreateTime,@ExpireTime)";
            //    cmd.Parameters.Add(new SqlParameter("@GenUid", uid));
            //    cmd.Parameters.Add(new SqlParameter("@Code",System.Data.SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@CodeState", (object)InviteCode.STATUS_NEW));
            //    cmd.Parameters.Add(new SqlParameter("@Source", source));
            //    cmd.Parameters.Add(new SqlParameter("@CreateTime", DateTime.Today));
            //    cmd.Parameters.Add(new SqlParameter("@ExpireTime", DateTime.Today.AddDays(7)));

            //    foreach (var code in inviteCodes)
            //    {
            //        cmd.Parameters["@Code"].Value = code;
            //        rowAffected += cmd.ExecuteNonQuery();
            //    }

            //    tran.Commit();
            //}

            return rowAffected;
        }
    }
}
