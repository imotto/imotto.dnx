using System;

namespace iMotto.Data
{
    [Obsolete]
    public abstract class RepoFactory
    {
        private static string RepoFactorySetting = "";  //ConfigurationManager.AppSettings.Get("UseRepoFactory");
        private static object lockHelper = new object();

        private static RepoFactory factoryInstance;


        protected abstract TRepo GetRepositoryCore<TRepo>() where TRepo : IRepository;

        private static RepoFactory getFactory()
        {
            if (factoryInstance == null)
            {
                lock (lockHelper)
                {
                    if (factoryInstance == null)
                    {   
                        var type = Type.GetType(RepoFactorySetting);
                        var obj = Activator.CreateInstance(type);

                        factoryInstance = obj as RepoFactory;

                        if (factoryInstance == null)
                            throw new InvalidCastException("Can not cast to type: RepoFactory");

                    }

                }
            }

            return factoryInstance;
        }


        public static TRepo GetRepository<TRepo>() where TRepo : class, IRepository
        {
            return getFactory().GetRepositoryCore<TRepo>();
        }
    }
}
