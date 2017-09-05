using System.Data;

namespace iMotto.Data
{
    public interface IConnectionProvider
    {
        IDbConnection GetConnection();
    }
}
