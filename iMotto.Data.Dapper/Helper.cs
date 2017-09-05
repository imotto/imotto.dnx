
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    class Helper
    {
        public static List<SqlParameter> BuildDynamicParameters<T>(ref string sql, List<T> values)
        {
            var parameters = new List<SqlParameter>(values.Count);

            StringBuilder sb = new StringBuilder();
            
            for(int i=1;i<=values.Count;i++)
            {
                sb.AppendFormat("@p{0},", i);
                parameters.Add(new SqlParameter(string.Format("@p{0}",i), values[i]));
            }

            sql = string.Format(sql, sb.ToString(0, sb.Length - 1));

            return parameters;
        }
    }
}
