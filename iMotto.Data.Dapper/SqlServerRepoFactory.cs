using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    //class SqlServerRepoFactory : RepoFactory
    //{
    //    private static Dictionary<Type, IRepository> repos = new Dictionary<Type, IRepository>();
    //    private static object lockHelper = new object();

    //    static SqlServerRepoFactory()
    //    {
    //        repos.Add(typeof(ICommonRepo), new CommonRepo());            
    //        repos.Add(typeof(IBanRepo), new BanRepo());
    //        repos.Add(typeof(IFollowRepo), new FollowRepo());
    //        repos.Add(typeof(IMottoRepo), new MottoRepo());
    //        repos.Add(typeof(IRecruitRepo), new RecruitRepo());
    //        repos.Add(typeof(IReportRepo), new ReportRepo());
    //        repos.Add(typeof(IStatisticsRepo), new StatisticsRepo());
    //        repos.Add(typeof(ICollectionRepo), new CollectionRepo());
    //        repos.Add(typeof(IUserRepo), new UserRepo());
    //        repos.Add(typeof(IManageRepo), new ManageRepo());
    //        repos.Add(typeof(IMsgRepo), new MsgRepo());
    //        repos.Add(typeof(IGiftRepo), new GiftRepo());
    //    }

    //    protected override TRepo GetRepositoryCore<TRepo>()
    //    {
    //        var t = typeof(TRepo);
    //        if (repos.ContainsKey(t))
    //        {
    //            return (TRepo)repos[t];
    //        }

    //        return default(TRepo);
    //    }
    //}
}
